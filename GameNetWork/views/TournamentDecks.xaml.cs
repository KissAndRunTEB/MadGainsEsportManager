using HtmlAgilityPack;
using MadGains.Gwent;
using MadGains.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Logika interakcji dla klasy TournamentDecks.xaml
    /// </summary>
    public partial class TournamentDecks : Window
    {
        List<TournamentDeckEntry> players = new List<TournamentDeckEntry>();

        public TournamentDecks()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string link = linkToTournament.Text;

            players = PlayGwent.pull_deck_linksVersion2(link);


            progressBar.Maximum = players.Count;
            progressBar.Value = 0;

            fieldForLinks.Text = "";

            Thread thread = new Thread(new ThreadStart(checkDecks));
            thread.Start();


            //progressBar.Value = 0;
        }

        private void checkDecks()
        {
            List<string> countingdekcs = new List<string>();
            List<string> countingCountries = new List<string>();
        
            
            addTextLine("<p>");

            foreach (TournamentDeckEntry a in players)
            {
                string text = "";

                ExtendedStats staty = new ExtendedStats("https://www.playgwent.com/en/profile/" + a.Player);

                Player p = staty.GetPlayer(a.Player);

                if(a.Country== "-"|| a.Country == "" || a.Country == null)
                {
                    text += "<strong>" + a.Player + " </strong> ";
                }
                else
                {
                    text += "<strong>" + a.Player + " </strong>" + "[" + a.Country + "] ";
                }
               
            //    text += "WR: "+p.Wr+"%, MMR: "+p.Mmr+"<br/>";
                text += "<a href=###" + a.DeckA + "### target=###_blank### rel=###noreferrer noopener###>" + a.FactionA + "</a>, ";
                text += "<a href=###" + a.DeckB + "### target=###_blank### rel=###noreferrer noopener###>" + a.FactionB + "</a>, ";
                text += "<a href=###" + a.DeckC + "### target=###_blank### rel=###noreferrer noopener###>" + a.FactionC + "</a>, ";
                text += "<a href=###" + a.DeckD + "### target=###_blank### rel=###noreferrer noopener###>" + a.FactionD + "</a>";

                text = text.Replace("###", "\"");

                text = text.Replace(@"\pl\", @"\en\");
                text = text.Replace(@"\ru\", @"\en\");
                text = text.Replace(@"\zh-cn\", @"\en\");
                text = text.Replace(@"\de\", @"\en\");

                text = text.Replace(@"\en\", @"\pl\");

                addTextLine(text);

                Dispatcher.Invoke(() =>
                {
                    progressBar.Value = progressBar.Value + 1;
                });

                countingdekcs.Add(a.FactionA);
                countingdekcs.Add(a.FactionB);
                countingdekcs.Add(a.FactionC);
                countingdekcs.Add(a.FactionD);

                countingCountries.Add(a.Country);

                addTextLine("<br/>");
            }

            addTextLine("</p>");


            countAndShow(countingdekcs);

            addTextLine2("");

            countAndShow(countingCountries);
            //   saveTeamStatsToFile("Esport section", ourEsportTeam, true);

        }

        private void countAndShow(List<string> countingdekcs)
        {
            var q = from x in countingdekcs
                    group x by x into g
                    let count = g.Count()
                    orderby count descending
                    select new { Value = g.Key, Count = count };
            foreach (var x in q)
            {
                addTextLine2(x.Value + ": " + x.Count);
                //Console.WriteLine("Value: " + x.Value + " Count: " + x.Count);
            }
        }

        private void addTextLine(string text)
        {
            Dispatcher.Invoke(() =>
            {
                fieldForLinks.Text = fieldForLinks.Text + text + System.Environment.NewLine;
            });
        }

        private void addTextLine2(string text)
        {
            Dispatcher.Invoke(() =>
            {
                fieldForStats.Text = fieldForStats.Text + text + System.Environment.NewLine;
            });
        }

        private void saveTeamStatsToFile(string teamName, List<TeamMates> team, bool tag)
        {
            addTextLine(teamName);

            addTextLine(DateTime.Now.ToString("yyyy-dd-MM-HH"));


            foreach (TeamMates tm in team)
            {
                tm.Player = PlayGwent.pullAllSeasonsStatistics(tm.GogNick);


                int average = 0;

                        string tagAT = "";
                        if (tag)
                        {
                            tagAT = "@";
                        }


                    string listOfPositions = "";

                    foreach(int positionSingle in tm.Player.LadderPositionThrewSeasons)
                    {
                        if (positionSingle!=0 && positionSingle != 1)
                        {
                            listOfPositions = listOfPositions + positionSingle.ToString() + ", ";

                        average = average + positionSingle;
                        }
                        else{

                            listOfPositions = listOfPositions + "-, ";
                        }
                    }

                average = (int)(average/(tm.Player.LadderPositionThrewSeasons.Count));

                    addTextLine(tagAT + tm.DiscordNick + ": " + listOfPositions + " | Avg. place: "+average);

                    
            }




                addTextLine("");
                addTextLine("");
     
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {


            string link = linkToTournament.Text;

            players = pulldecksfromCSV();


            progressBar.Maximum = players.Count;
            progressBar.Value = 0;

            fieldForLinks.Text = "";

            Thread thread = new Thread(new ThreadStart(checkDecks));
            thread.Start();

        }

        private List<TournamentDeckEntry> pulldecksfromCSV()
        {
            //  PlayerLineup a = new PlayerLineup();

            List<TournamentDeckEntry> listEntries = new List<TournamentDeckEntry>();

  

            string line = "";

            fieldForLinks.Text = "";

            System.IO.StreamReader file =
    new System.IO.StreamReader(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/csv.csv");
            while ((line = file.ReadLine()) != null)
            {
                string[] elementy = line.Split(',');

                TournamentDeckEntry a = new TournamentDeckEntry();

                a.Player = elementy[0];

                a.FactionA = elementy[1];
                a.FactionB = elementy[2];
                a.FactionC = elementy[3];

                a.DeckA = "https://docs.google.com/spreadsheets/d/1vTLN9_yh37TbKqQV01EbLtFm_jegGOnFgYmDib7TO9U/edit#gid=305228669";
                a.DeckB = "https://docs.google.com/spreadsheets/d/1vTLN9_yh37TbKqQV01EbLtFm_jegGOnFgYmDib7TO9U/edit#gid=305228669";
                a.DeckC = "https://docs.google.com/spreadsheets/d/1vTLN9_yh37TbKqQV01EbLtFm_jegGOnFgYmDib7TO9U/edit#gid=305228669";

               // a.DeckA = elementy[4];
                //a.DeckB = elementy[5];
                //a.DeckC = elementy[6];


                a.Country = PlayGwent.getCountry(a.Player);

                listEntries.Add(a);

                //fieldForLinks.Text += "<p><strong>" + a.Player) + " </strong>";
                //fieldForLinks.Text += "<a href=###" + a.LinkA + "### target=###_blank### rel=###noreferrer noopener###>" + a.FrackjaA + "</a>, ";
                //fieldForLinks.Text += "<a href=###" + a.LinkB + "### target=###_blank### rel=###noreferrer noopener###>" + a.FrackjaB + "</a>, ";
                //fieldForLinks.Text += "<a href=###" + a.LinkC + "### target=###_blank### rel=###noreferrer noopener###>" + a.FrackjaC + "</a></p>";

                //fieldForLinks.Text = fieldForLinks.Text.Replace("###", "\"");

                //fieldForLinks.Text = fieldForLinks.Text.Replace(@"\pl\", @"\en\");
                //fieldForLinks.Text = fieldForLinks.Text.Replace(@"\ru\", @"\en\");
                //fieldForLinks.Text = fieldForLinks.Text.Replace(@"\zh-cn\", @"\en\");
                //fieldForLinks.Text = fieldForLinks.Text.Replace(@"\de\", @"\en\");

                //fieldForLinks.Text = fieldForLinks.Text.Replace(@"\en\", @"\pl\");

            }

            listEntries.RemoveAt(0);

            return listEntries;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string link = linkToTournament.Text;

            // players = pulldecksfromCSV();

            players = pulldecksfromHTML();

            progressBar.Maximum = players.Count;
            progressBar.Value = 0;

            fieldForLinks.Text = "";

            Thread thread = new Thread(new ThreadStart(checkDecks));
            thread.Start();
        }

        private List<TournamentDeckEntry> pulldecksfromHTML()
        {
            //  PlayerLineup a = new PlayerLineup();

            List<TournamentDeckEntry> listEntries = new List<TournamentDeckEntry>();



            string line = "";

            fieldForLinks.Text = "";

            System.IO.StreamReader file =
    new System.IO.StreamReader(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/html.html");

            string text = file.ReadToEnd();

            text = text.Substring(text.IndexOf("tbody") + 1);

            string[] aaa = text.Split("<tr");

            aaa = aaa.Skip(1).ToArray();
            aaa = aaa.Skip(1).ToArray();
            aaa = aaa.Skip(1).ToArray();

            foreach (var word in aaa)
            {

                string[] bbb = word.Split("<td");

                string bm= ">";   
                
                string nick = bbb[1].Substring(bbb[1].IndexOf(bm) + 1);
                nick = nick.Remove(nick.IndexOf("<"));

                TournamentDeckEntry a = new TournamentDeckEntry();

                a.Player = nick;

                string fA= between(bbb[2].Substring(bbb[2].IndexOf("\">") + 1), "\">", "</a>");
                string fB = between(bbb[3].Substring(bbb[3].IndexOf("\">") + 1), "\">", "</a>");
                string fC = between(bbb[4].Substring(bbb[4].IndexOf("\">") + 1), "\">", "</a>");

                string dA = between(bbb[2], "href=\"", "\">");
                string dB = between(bbb[3], "href=\"", "\">");
                string dC = between(bbb[4], "href=\"", "\">");

                a.FactionA = fA;
               a.FactionB = fB;
                a.FactionC = fC;

                a.DeckA = dA;
                a.DeckB = dB;
                a.DeckC = dC;


                //a.Country = PlayGwent.getCountry(a.Player);

                listEntries.Add(a);


            }




            return listEntries;
        }

        string between(string text, string fromText, string toText)
        {
            string a = "";

            a = text.Substring(text.IndexOf(fromText) + fromText.Length);
            a = a.Remove(a.IndexOf(toText));

            return a;
        }
    }
}
