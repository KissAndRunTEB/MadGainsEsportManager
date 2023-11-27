using MadGains.Logic;
using MadGains.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace MadGains.Data
{
    public partial class DataBase
    {
        public List<Deck> getDecks(bool skipOther=false)
        {
            var webClient = new WebClient();
            string a = webClient.DownloadString("https://teamelderblood.com/gg/decks.php");


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


                if (skipOther)
                {
                    if(d.Tier!="Other")
                    {
                        list.Add(d);
                    }
                }
                else
                {
                    list.Add(d);
                }
            }


            return list;
        }


        public void AddDeck(string name, string faction, string abilitie, int snap, string tier, string star, string adress, string date)
        {
            //name, faction, ability, idSnapshot, tier, stars, link, creationDate

            if (faction == "Scoia'tael")
            {
                faction = "Scoiatael";
            }

            var webClient = new WebClient();
            string url = "https://teamelderblood.com/gg/decks.php?name=" + name + "&faction=" + faction + "&ability=" + abilitie + "&idSnapshot=" + snap.ToString() + "&tier=" + tier + "&stars=" + star + "&link=" + adress + "&creationDate=" + date;
            string a = webClient.DownloadString(url);


        }

        public void deleteDeck(int id)
        {
            var webClient = new WebClient();
            string url = "https://teamelderblood.com/gg/decks.php?delete=" + id;
            string a = webClient.DownloadString(url);
        }

        internal void UpdateDeck(int id, string name, string faction, string abilitie, int snap, string tier, string star, string adress, string date)
        {
            if (faction == "Scoia'tael")
            {
                faction = "Scoiatael";
            }

            var webClient = new WebClient();
            string url = "https://teamelderblood.com/gg/decks.php?update=" + id + "&name=" + name + "&faction=" + faction + "&ability=" + abilitie + "&idSnapshot=" + snap.ToString() + "&tier=" + tier + "&stars=" + star + "&link=" + adress + "&creationDate=" + date;
            string a = webClient.DownloadString(url);
        }

    }
}
