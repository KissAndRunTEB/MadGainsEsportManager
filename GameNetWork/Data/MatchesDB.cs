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
        public List<Match> getMatches(int idFind=-1)
        {
            var webClient = new WebClient();

            string a = "";
            if (idFind==-1)
            {
                a = webClient.DownloadString("https://teamelderblood.com/gg/matches.php");                
            }
            else
            {
                a = webClient.DownloadString("https://teamelderblood.com/gg/matches.php?find=" + idFind);
            }

            List<string> lines = a.Split("@@@").ToList();
            lines.RemoveAt(lines.Count - 1);


            List<Match> list = new List<Match>();

            foreach (string s in lines)
            {
                List<string> values = s.Split(" | ").ToList();

                Match d = new Match();
                d.Id = Int32.Parse(values.ElementAt(0));

                d.IdPlayer = Int32.Parse(values.ElementAt(1));

                d.GogOponent = values.ElementAt(2);

                d.PlayerDeck = Int32.Parse(values.ElementAt(3));

                d.OponentDeck = Int32.Parse(values.ElementAt(4));

                d.Date = values.ElementAt(5);

                d.Type = values.ElementAt(6);

                d.Win = values.ElementAt(7);



                list.Add(d);
            }


            return list;
        }

        public void AddMatch(int idPlayer, string gogOponent, int playerDeck, int oponentDeck, string date, string type, string win, int coin)
        {
            //idPlayer, gogOponent, playerDeck, idoponentDeckshot, date, types, link, creationDate


            var webClient = new WebClient();
            string url = "https://teamelderblood.com/gg/matches.php?idPlayer=" + idPlayer + "&gogOponent=" + gogOponent + "&playerDeck=" + playerDeck + "&oponentDeck=" + oponentDeck + "&date=" + date + "&type=" + type + "&win=" + win + "&creationDate=" + date + "&coin=" + coin;
            string a = webClient.DownloadString(url);


        }

        public void deleteMatch(int id)
        {
            var webClient = new WebClient();
            string url = "https://teamelderblood.com/gg/matches.php?delete=" + id;
            string a = webClient.DownloadString(url);
        }

        internal void UpdateMatch(int id, string idPlayer, string gogOponent, string playerDeck, int oponentDeck, string date, string type, string win)
        {

            var webClient = new WebClient();
            string url = "https://teamelderblood.com/gg/matches.php?update=" + id + "&idPlayer=" + idPlayer + "&gogOponent=" + gogOponent + "&playerDeck=" + playerDeck + "&idoponentDeckshot=" + oponentDeck.ToString() + "&date=" + date + "&types=" + type + "&link=" + win + "&creationDate=" + date;
            string a = webClient.DownloadString(url);
        }

    }
}
