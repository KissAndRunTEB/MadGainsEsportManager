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
        public List<Matchup> getMatchups(int idFindA = -1, int idFindB = -1)
        {
            var webClient = new WebClient();

            string a = "";
            if (idFindA == -1)
            {
                a = webClient.DownloadString("https://teamelderblood.com/gg/matchups.php");
            }
            else
            {
                a = webClient.DownloadString("https://teamelderblood.com/gg/matchups.php?findA=" + idFindA+"&findB=" + idFindB);
            }

            List<string> lines = a.Split("@@@").ToList();
            lines.RemoveAt(lines.Count - 1);


            List<Matchup> list = new List<Matchup>();

            foreach (string s in lines)
            {
                List<string> values = s.Split(" | ").ToList();

                Matchup d = new Matchup();
                d.Id = Int32.Parse(values.ElementAt(0));

                d.IdDeckA = Int32.Parse(values.ElementAt(1));

                d.IdDeckB = Int32.Parse(values.ElementAt(2));

                d.Description = values.ElementAt(3);

                d.Date = values.ElementAt(4);

                d.Author = Int32.Parse(values.ElementAt(5));



                list.Add(d);
            }


            return list;
        }

        public void AddMatchup(int idDeckA, int idDeckB, string description, string date, int author)
        {
            var webClient = new WebClient();
            string url = "https://teamelderblood.com/gg/matchups.php?idDeckA=" + idDeckA + "&idDeckB=" + idDeckB + "&description=" + description + "&date=" + date + "&author=" + author;
            string a = webClient.DownloadString(url);


        }

        public void deleteMatchup(int id)
        {
            var webClient = new WebClient();
            string url = "https://teamelderblood.com/gg/matchups.php?delete=" + id;
            string a = webClient.DownloadString(url);
        }

        internal void UpdateMatchup(int id, int idDeckA, int idDeckB, string description, string date, int author)
        {

            var webClient = new WebClient();
            string url = "https://teamelderblood.com/gg/matchups.php?update=" + id + "&idDeckA=" + idDeckA + "&idDeckB=" + idDeckB + "&description=" + description + "&date=" + date + "&author=" + author;
            string a = webClient.DownloadString(url);
        }

    }
}
