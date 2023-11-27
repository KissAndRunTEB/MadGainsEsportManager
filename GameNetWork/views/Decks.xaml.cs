using MadGains.Data;
using MadGains.Gwent;
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
    /// Logika interakcji dla klasy Decks.xaml
    /// </summary>
    public partial class Decks : Window
    {
        public Decks()
        {
            InitializeComponent();

            setComboboxes();

            refreshListOfDecks();

        }

        private void setComboboxes()
        {
            ComboBox boxFaction = factions;

            Fill.fillFactions(boxFaction);

            snapshot.Items.Add("Team Elder Blood");
            snapshot.Items.Add("Team Leviathan Gaming");
            snapshot.Items.Add("Other");

            snapshot.SelectedIndex = 0;

            tiers.Items.Add("Tier 1");
            tiers.Items.Add("Tier 2");
            tiers.Items.Add("Tier 3");
            tiers.Items.Add("Honorable mentions");

            tiers.SelectedIndex = 0;

            stars.Items.Add("5.0");
            stars.Items.Add("4.75");
            stars.Items.Add("4.5");
            stars.Items.Add("4.25");
            stars.Items.Add("4.0");
            stars.Items.Add("3.75");
            stars.Items.Add("3.5");
            stars.Items.Add("3.25");
            stars.Items.Add("3.00");
            stars.Items.Add("2.75");
            stars.Items.Add("2.5");
            stars.Items.Add("2.25");
            stars.Items.Add("2.0");
            stars.Items.Add("Without stars");

            stars.SelectedIndex = 4;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void list_decks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(list_decks.SelectedIndex!=-1)
            {
                buttonDeleteDeck.IsEnabled = true;
                buttonDeleteDeck.Content = "Delete deck: "+(list_decks.SelectedItem as Deck).Name;

                buttonAddNewDeck.IsEnabled = false;
                buttonBackToNew.IsEnabled = true;
                buttonEditDeck.IsEnabled = true;

                loadDataToEdit();
            }
            else
            {
                buttonDeleteDeck.IsEnabled = false;
                buttonDeleteDeck.Content = "Delete";

                buttonAddNewDeck.IsEnabled = true;
                buttonBackToNew.IsEnabled = false;
                buttonEditDeck.IsEnabled = false;
            }
        }

        private void loadDataToEdit()
        {
            Deck d = list_decks.SelectedItem as Deck;

            nameOfDeck.Text = d.Name;
            factions.SelectedValue = d.Faction; 
                abilities.SelectedValue = d.Ability;
            snapshot.SelectedIndex = Int32.Parse(d.Snapshot);
            tiers.SelectedValue = d.Tier;
            stars.SelectedValue = d.Stars;
            urlField.Text = d.Url;
        }

        private void factions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox boxAbilities = abilities;
            ComboBox boxFactions = factions;

            Fill.fillAbilities(boxAbilities, boxFactions);
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string name = nameOfDeck.Text;
            string faction = factions.SelectedItem.ToString();
            string abilitie = abilities.SelectedItem.ToString();

            int snap = snapshot.SelectedIndex;
            string tier = tiers.SelectedItem.ToString();
            string star = stars.SelectedItem.ToString();

            string url = urlField.Text;

            string date = DateTime.Now.ToString();


            //name, faction, ability, idSnapshot, tier, stars, link, creationDate
            DataBase db = new DataBase();
            db.AddDeck(name,faction,abilitie,snap,tier,star,url,date);


            refreshListOfDecks();
        }

        private void refreshListOfDecks()
        {
            DataBase db = new DataBase();

            List<Deck> list = db.getDecks(true);

            list_decks.ItemsSource = list;

        }

        private void buttonDeleteDeck_Click(object sender, RoutedEventArgs e)
        {
            if(list_decks.SelectedIndex!=-1)
            {
                DataBase db = new DataBase();

                db.deleteDeck((list_decks.SelectedItem as Deck).Id);

                refreshListOfDecks();
            }
        }

        private void buttonBackToNew_Click(object sender, RoutedEventArgs e)
        {
            list_decks.UnselectAll();
        }

        private void buttonEditDeck_Click(object sender, RoutedEventArgs e)
        {
            string name = nameOfDeck.Text;
            string faction = factions.SelectedItem.ToString();
            string abilitie = abilities.SelectedItem.ToString();

            int snap = snapshot.SelectedIndex;
            string tier = tiers.SelectedItem.ToString();
            string star = stars.SelectedItem.ToString();

            string url = urlField.Text;

            string date = DateTime.Now.ToString();

            int id = (list_decks.SelectedItem as Deck).Id;
            //name, faction, ability, idSnapshot, tier, stars, link, creationDate
            DataBase db = new DataBase();
            db.UpdateDeck(id, name, faction, abilitie, snap, tier, star, url, date);


            refreshListOfDecks();
        }
    }
}
