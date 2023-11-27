using MadGains.views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using MadGains.Data;
using MadGains.Logic;
using System.Net;
using System.Linq;
using System.Windows.Media;

namespace MadGains.Gwent
{
    public class Fill
    {
        public static void fillFactions(ComboBox boxFaction)
        {
            boxFaction.Items.Add("Northern Realms");
            boxFaction.Items.Add("Scoia'tael");
            boxFaction.Items.Add("Monsters");
            boxFaction.Items.Add("Skellige");
            boxFaction.Items.Add("Nilfgaard");
            boxFaction.Items.Add("Syndicate");

            boxFaction.SelectedIndex = 0;
        }

        public static void fillAbilities(ComboBox boxAbilities, ComboBox boxFactions)
        {
            boxAbilities.Items.Clear();

            switch (boxFactions.SelectedIndex)
            {
                case 0:
                    boxAbilities.Items.Add("Inspired Zeal");
                    boxAbilities.Items.Add("Royal Inspiration");
                    boxAbilities.Items.Add("Mobilization");
                    boxAbilities.Items.Add("Shieldwall");
                    boxAbilities.Items.Add("Stockpile");
                    boxAbilities.Items.Add("Pincer Maneuver");
                    boxAbilities.Items.Add("Uprising");

                    break;

                case 1:
                    boxAbilities.Items.Add("Guerilla tactics");
                    boxAbilities.Items.Add("Invigorate");
                    boxAbilities.Items.Add("Nature’s gift");
                    boxAbilities.Items.Add("Precision strike");
                    boxAbilities.Items.Add("Deadeye ambush");
                    boxAbilities.Items.Add("Call of harmony");
                    boxAbilities.Items.Add("Mahakam forge");

                    break;

                case 2:
                    boxAbilities.Items.Add("Carapace");
                    boxAbilities.Items.Add("Force of Nature");
                    boxAbilities.Items.Add("White frost");
                    boxAbilities.Items.Add("Arachas Swarm");
                    boxAbilities.Items.Add("Fruits of Ysgith");
                    boxAbilities.Items.Add("Blood Scent");
                    boxAbilities.Items.Add("Overwhelming hunger");

                    break;

                case 3:
                    boxAbilities.Items.Add("Onslaught");
                    boxAbilities.Items.Add("Reckless Flurry");
                    boxAbilities.Items.Add("Battle Trance");
                    boxAbilities.Items.Add("Rage of the sea");
                    boxAbilities.Items.Add("Patricidal fury");
                    boxAbilities.Items.Add("Ursine ritual");
                    boxAbilities.Items.Add("Blaze of glory");

                    break;

                case 4:
                    boxAbilities.Items.Add("Imprisonment");
                    boxAbilities.Items.Add("Imperial formation");
                    boxAbilities.Items.Add("Tactical Decision");
                    boxAbilities.Items.Add("Lockdown");
                    boxAbilities.Items.Add("Enslave");
                    boxAbilities.Items.Add("Double Cross");
                    boxAbilities.Items.Add("Imposter");

                    break;



                case 5:
                    boxAbilities.Items.Add("Blood Money");
                    boxAbilities.Items.Add("Jackpot");
                    boxAbilities.Items.Add("Lined pockets");
                    boxAbilities.Items.Add("Off the books");
                    boxAbilities.Items.Add("Congregate");
                    boxAbilities.Items.Add("Pirate’s Cove");
                    boxAbilities.Items.Add("Hidden cache");


                    break;

                default:
                    boxAbilities.Items.Add("Inspired Zeal");
                    boxAbilities.Items.Add("Royal Inspiration");
                    boxAbilities.Items.Add("Mobilization");
                    boxAbilities.Items.Add("Shieldwall");
                    boxAbilities.Items.Add("Stockpile");
                    boxAbilities.Items.Add("Pincer Maneuver");
                    boxAbilities.Items.Add("Uprising");

                    break;
            }

            boxAbilities.SelectedIndex = 0;
        }

        internal static void fillCoins(ComboBox coinField)
        {
            ComboBoxItem c1 = new ComboBoxItem();
            ComboBoxItem c2 = new ComboBoxItem();
            c1.Foreground = Brushes.DarkBlue;
            c1.Content = "Blue coin (I started)";

            c2.Foreground = Brushes.Red;
            c2.Content = "Red coin (I played second)";

            coinField.Items.Add(c1);
            coinField.Items.Add(c2);

            coinField.SelectedIndex = 0;
        }

        internal static void fillReulsts(ComboBox resultField)
        {
            resultField.Items.Add("Win");
            resultField.Items.Add("Lost");
            resultField.Items.Add("Draw");

            resultField.SelectedIndex = 0;
        }

        internal static void fillTypes(ComboBox typeField)
        {

            typeField.Items.Add("Pro rank");
            typeField.Items.Add("Ranked");
            typeField.Items.Add("Non-ranked");
            typeField.Items.Add("Training");
            typeField.Items.Add("Tournament");

            typeField.SelectedIndex = 0;
        }

        public static void fillPossibleDecks(ComboBox yourDecks, ComboBox yourAbility)
        {
            string ability = yourAbility.SelectedItem.ToString();


            List<Deck> decks = getDecksFromAbility(ability);

            yourDecks.ItemsSource = decks;

            yourDecks.SelectedIndex = 0;


        }

        public static List<Deck> getDecksFromAbility(string ability)
        {
            var webClient = new WebClient();
            string a = webClient.DownloadString("https://teamelderblood.com/gg/decks.php?find=" + ability);


            List<string> lines = a.Split("@@@").ToList();
            lines.RemoveAt(lines.Count - 1);


            List<Deck> list = new List<Deck>();

            foreach (string s in lines)
            {
                List<string> values = s.Split(" | ").ToList();

                Deck d = new Deck();
                d.Id = Int32.Parse(values.ElementAt(0));

                d.Name = values.ElementAt(1);

                d.Faction = values.ElementAt(2);

                if (d.Faction == "Scoiatael")
                {
                    d.Faction = "Scoia'tael";
                }

                d.Ability = values.ElementAt(3);

                d.Tier = values.ElementAt(4);

                d.Stars = values.ElementAt(5);

                d.Snapshot = values.ElementAt(6);

                d.Url = values.ElementAt(7);

                list.Add(d);

            }


            return list;
        }

    }
}
