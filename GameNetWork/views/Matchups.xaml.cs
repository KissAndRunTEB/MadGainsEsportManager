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
    /// Logika interakcji dla klasy Matchups.xaml
    /// </summary>
    public partial class Matchups : Window
    {
        public Matchups()
        {
            InitializeComponent();

            setComboboxes();

            refreshListOfMatchups();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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


        private void list_matchups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (list_matchups.SelectedIndex != -1)
            {
                buttonDeleteMatchup.IsEnabled = true;
                buttonDeleteMatchup.Content = "Delete matchup: " + (list_matchups.SelectedItem as Matchup).Id;

                buttonAddNewMatchup.IsEnabled = false;
                buttonBackToNew.IsEnabled = true;
                buttonEditMatchup.IsEnabled = true;

                loadDataToEdit();
            }
            else
            {
                buttonDeleteMatchup.IsEnabled = false;
                buttonDeleteMatchup.Content = "Delete";

                buttonAddNewMatchup.IsEnabled = true;
                buttonBackToNew.IsEnabled = false;
                buttonEditMatchup.IsEnabled = false;
            }
        }

        private void loadDataToEdit()
        {
            Matchup d = list_matchups.SelectedItem as Matchup;

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

        private void refreshListOfMatchups()
        {
            DataBase db = new DataBase();

            List<Matchup> list = db.getMatchups();

            list_matchups.ItemsSource = list;

        }

        private void buttonDeleteMatchup_Click(object sender, RoutedEventArgs e)
        {
            if (list_matchups.SelectedIndex != -1)
            {
                DataBase db = new DataBase();

                db.deleteMatch((list_matchups.SelectedItem as Matchup).Id);

                refreshListOfMatchups();
            }
        }

        private void buttonBackToNew_Click(object sender, RoutedEventArgs e)
        {
            list_matchups.UnselectAll();
        }

        private void buttonEditMatchup_Click(object sender, RoutedEventArgs e)
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
