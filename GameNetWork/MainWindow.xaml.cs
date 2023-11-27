using MadGains.Data;
using MadGains.Gwent;
using MadGains.Logic;
using MadGains.views;
using MadGains.views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MadGains
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool reallyClose = false;
        Settings user;

        public MainWindow()
        {
            InitializeComponent();

            System.Reflection.Assembly executingAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            var fieVersionInfo = FileVersionInfo.GetVersionInfo(executingAssembly.Location);
            var version = fieVersionInfo.FileVersion;

            versionField.Header = "Version: "+version;



            Login windowLogin = new Login();

            if(windowLogin.ShowDialog()!=true)
            {
                this.Close();
            }

            user = new Settings();

            if(user.Minimalise)
            {
                this.Hide();
            }

            loginLabel.Content = "Logged in as: "+user.Nick;

            prepareCurrentMatch();

            refreshListOfMineMatches(user.IdUser);
        }

        private void refreshListOfMineMatches(int idUser)
        {
            DataBase db = new DataBase();

            List<Match> list = db.getMatches(idUser);

            list_matches_your.ItemsSource = list;
        }

        private void prepareCurrentMatch()
        {
            Fill.fillFactions(yourFaction);
            Fill.fillFactions(hisFaction);

            Fill.fillAbilities(yourAbility, yourFaction);
            Fill.fillAbilities(hisAbility, hisFaction);

            Fill.fillTypes(typeField);
            Fill.fillReulsts(resultField);

            Fill.fillCoins(coinField);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Decks decks_windows = new Decks();
            decks_windows.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Matches matches_windows = new Matches();
            matches_windows.ShowDialog();
        }

        private void yourFaction_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Fill.fillAbilities(yourAbility, yourFaction);
        }

        private void hisFaction_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Fill.fillAbilities(hisAbility, hisFaction);
        }

        private void yourAbility_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (yourAbility.SelectedIndex!=-1)
            {
                Fill.fillPossibleDecks(yourDeck, yourAbility);
            }
        }

        private void hisAbility_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (hisAbility.SelectedIndex != -1)
            {
                Fill.fillPossibleDecks(hisDeck, hisAbility);
            }
        }

        private void mouse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Info window = new Info();
            window.ShowDialog();
        }



        protected override void OnClosing(CancelEventArgs e)
        {
            if (!reallyClose)
            {
                // setting cancel to true will cancel the close request
                // so the application is not closed
                e.Cancel = true;

                this.Hide();

                base.OnClosing(e);
            }
            else
            {
                base.OnClosing(e);
            }
        }



        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            reallyClose = true;

            this.Close();

        }

        private void buttonSaveResult_Click(object sender, RoutedEventArgs e)
        {
            int idPlayer = user.IdUser;
            string gogOponent = oponentNameField.Text;

            int playerDeck = (yourDeck.SelectedItem as Deck).Id;
            int oponentDeck = (hisDeck.SelectedItem as Deck).Id;


            string date = DateTime.Now.ToString();

            string type = typeField.SelectedItem.ToString();

            string win = resultField.SelectedItem.ToString();



            int coin = coinField.SelectedIndex+1;



            //name, faction, ability, idSnapshot, tier, stars, link, creationDate
            DataBase db = new DataBase();
            db.AddMatch(idPlayer, gogOponent, playerDeck, oponentDeck, date, type, win, coin);


            db.AddMatchup(playerDeck,oponentDeck, descriptionField.Text,date, idPlayer);

            refreshListOfMineMatches(user.IdUser);

            refreshTips();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Matchups matchups_windows = new Matchups();
            matchups_windows.ShowDialog();
        }

        private void hisDeck_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (hisDeck.SelectedItem!=null)
            {
                string link = (hisDeck.SelectedItem as Deck).Url;
                string deckName = (hisDeck.SelectedItem as Deck).Name;

                if (link.Contains("https://www.playgwent.com/"))
                {
                    linkText.Text = "Link to " + deckName;
                    linkToDeck.NavigateUri = new Uri(link);
                    linkToDeck.IsEnabled = true;
                }
                else if (link == "-")
                {
                    linkText.Text = "Current oponent deck doesn't have link";
                    linkToDeck.NavigateUri = new Uri("https://www.google.com/");
                    linkToDeck.IsEnabled = false;
                }
                else
                {
                    linkText.Text = "Not valid link";
                    linkToDeck.NavigateUri = new Uri("https://www.google.com/");
                    linkToDeck.IsEnabled = false;
                }

                refreshTips();

            }
        }

        private void refreshTips()
        {
            int a = (yourDeck.SelectedItem as Deck).Id;
            int b = (hisDeck.SelectedItem as Deck).Id;

            DataBase db = new DataBase();
            List<Matchup> tips = db.getMatchups(a, b);

            matchupDescriptions.ItemsSource = tips;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Positions pos = new Positions();
            pos.ShowDialog();
        }

        private void versionField_Click(object sender, RoutedEventArgs e)
        {
            Info window = new Info();
            window.ShowDialog();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Standings pos = new Standings();
            pos.ShowDialog();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            TournamentDecks tournament = new TournamentDecks();
            tournament.ShowDialog();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {

            var logFile = File.ReadAllLines(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop)+"/Decki.txt");

            List<string> lista = new List<string>(logFile);


            List<string> sorted = lista.OrderBy(q => q).ToList();


            foreach (string s in sorted)
            {
               // pole_na_liste_powt.Text = pole_na_liste_powt.Text + s + System.Environment.NewLine;
            }


            var list = new List<string> { "a", "b", "a", "c", "a", "b" };
            var q = from x in sorted
                    group x by x into g
                    let count = g.Count()
                    orderby count descending
                    select new { Value = g.Key, Count = count };
            foreach (var x in q)
            {
                pole_na_liste_powt.Text = pole_na_liste_powt.Text + x.Value +": "+ x.Count  + System.Environment.NewLine;
            }


        }
    }

    public class ShowMessageCommand : ICommand
    {
        public void Execute(object parameter)
        {
            // MessageBox.Show(parameter.ToString());

            Application.Current.MainWindow.Show();

        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }


}
