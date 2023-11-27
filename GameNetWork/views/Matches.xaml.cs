using MadGains.Data;
using MadGains.Logic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MadGains.views
{
    /// <summary>
    /// Logika interakcji dla klasy Matches.xaml
    /// </summary>
    public partial class Matches : Window
    {
        public Matches()
        {
            InitializeComponent();

            setComboboxes();

            refreshListOfMatches();

        }

        private void setComboboxes()
        {
            //factions.Items.Add("Northern Realms");
            //factions.Items.Add("Scoia'tael");
            //factions.Items.Add("Monsters");
            //factions.Items.Add("Skellige");
            //factions.Items.Add("Nilfgaard");
            //factions.Items.Add("Syndicate");

            //factions.SelectedIndex = 0;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void list_matches_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (list_matches.SelectedIndex != -1)
            {
                buttonDeleteMatch.IsEnabled = true;
                buttonDeleteMatch.Content = "Delete match: " + (list_matches.SelectedItem as Match).IdPlayer;

                buttonAddNewMatch.IsEnabled = false;
                buttonBackToNew.IsEnabled = true;
                buttonEditMatch.IsEnabled = true;

                loadDataToEdit();
            }
            else
            {
                buttonDeleteMatch.IsEnabled = false;
                buttonDeleteMatch.Content = "Delete";

                buttonAddNewMatch.IsEnabled = true;
                buttonBackToNew.IsEnabled = false;
                buttonEditMatch.IsEnabled = false;
            }
        }

        private void loadDataToEdit()
        {
            Match d = list_matches.SelectedItem as Match;

            //nameOfMatch.Text = d.Name;
            //factions.SelectedValue = d.Faction;
            //abilities.SelectedValue = d.Ability;
            //snapshot.SelectedIndex = Int32.Parse(d.Snapshot);
            //tiers.SelectedValue = d.Tier;
            //stars.SelectedValue = d.Stars;
            //urlField.Text = d.Url;
        }

 
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //string name = nameOfMatch.Text;
            //string faction = factions.SelectedItem.ToString();
            //string abilitie = abilities.SelectedItem.ToString();

            //int snap = snapshot.SelectedIndex;
            //string tier = tiers.SelectedItem.ToString();
            //string star = stars.SelectedItem.ToString();

            //string url = urlField.Text;

            //string date = DateTime.Now.ToString();


            ////name, faction, ability, idSnapshot, tier, stars, link, creationDate
            //DataBase db = new DataBase();
            //db.AddMatch(name, faction, abilitie, snap, tier, star, url, date);


            //refreshListOfMatches();
        }

        private void refreshListOfMatches()
        {
            DataBase db = new DataBase();

            List<Match> list = db.getMatches();

            list_matches.ItemsSource = list;

        }

        private void buttonDeleteMatch_Click(object sender, RoutedEventArgs e)
        {
            if (list_matches.SelectedIndex != -1)
            {
                DataBase db = new DataBase();

                db.deleteMatch((list_matches.SelectedItem as Match).Id);

                refreshListOfMatches();
            }
        }

        private void buttonBackToNew_Click(object sender, RoutedEventArgs e)
        {
            list_matches.UnselectAll();
        }

        private void buttonEditMatch_Click(object sender, RoutedEventArgs e)
        {
            //string name = nameOfMatch.Text;
            //string faction = factions.SelectedItem.ToString();
            //string abilitie = abilities.SelectedItem.ToString();

            //int snap = snapshot.SelectedIndex;
            //string tier = tiers.SelectedItem.ToString();
            //string star = stars.SelectedItem.ToString();

            //string url = urlField.Text;

            //string date = DateTime.Now.ToString();

            //int id = (list_matches.SelectedItem as Match).Id;
            ////name, faction, ability, idSnapshot, tier, stars, link, creationDate
            //DataBase db = new DataBase();
            //db.UpdateMatch(id, name, faction, abilitie, snap, tier, star, url, date);


            //refreshListOfMatches();
        }

    }
}
