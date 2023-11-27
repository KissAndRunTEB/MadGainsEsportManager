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
    /// Logika interakcji dla klasy Positions.xaml
    /// </summary>
    public partial class Standings : Window
    {
            List<TeamMates> ourEsportTeam = new List<TeamMates>();
            List<TeamMates> ourNonEsportTeam = new List<TeamMates>();

        public Standings()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //TeamMates a = new TeamMates("KissAndRun", "[TEB] KissAndRun");
            //ourEsportTeam.Add(a);

            TeamMates b = new TeamMates("pawloex", "pawloex");
            ourEsportTeam.Add(b);

            TeamMates c = new TeamMates("gregor__", "[TEB] gregor__");
            ourEsportTeam.Add(c);

            TeamMates d = new TeamMates("Kozix82", "Kozix#5511");
            ourEsportTeam.Add(d);

            TeamMates ee = new TeamMates("Olsmer", "[TEB] Olsmer");
            ourEsportTeam.Add(ee);

            TeamMates f = new TeamMates("Jollyish", "Jollyish#6835");
            ourEsportTeam.Add(f);

            TeamMates g = new TeamMates("9kjer", "9kjer");
            ourEsportTeam.Add(g);

            TeamMates h = new TeamMates("Ibas94", "[TEB] Ibas94");
            ourEsportTeam.Add(h);

            TeamMates i = new TeamMates("Zoltan3344", "[TEB] Zoltan3344");
            ourEsportTeam.Add(i);



            TeamMates k = new TeamMates("darthlothins", "[TEB] darthlothins");
            ourEsportTeam.Add(k);

            TeamMates l = new TeamMates("Gregory_Black", "[TEB] Gregory_Black");
            ourEsportTeam.Add(l);

            TeamMates m = new TeamMates("Rolero_4", "Rolero_4#5633");
            ourEsportTeam.Add(m);

            TeamMates n = new TeamMates("happybirthdayspyro", "[TEB] Noelle69420");
            ourEsportTeam.Add(n);

            TeamMates p = new TeamMates("Tyronin1", "[TEB] Tyronin1");
            ourEsportTeam.Add(p);


            TeamMates s = new TeamMates("patsy_1998", "patsy_1998");
            ourEsportTeam.Add(s);

            TeamMates t = new TeamMates("zurii69", "[TEB] zurii69");
            ourEsportTeam.Add(t);

            TeamMates w = new TeamMates("Truzky", "Bart232#1534");
            ourEsportTeam.Add(w);

            TeamMates u = new TeamMates("Gandalf0271", "[TEB] Gandalf0271");
            ourEsportTeam.Add(u);

            TeamMates z = new TeamMates("mareahlx", "[TEB] enzomarea");
            ourEsportTeam.Add(z);

            TeamMates z2 = new TeamMates("kostur3", "[TEB] Kostur");
            ourEsportTeam.Add(z2);


            TeamMates z3 = new TeamMates("Szamuro", "Szaman#2764");
            ourEsportTeam.Add(z3);



            TeamMates z5 = new TeamMates("RealLorenzOFFICIAL", "[TEB] TheRealLorenzOFFICIAL");
            ourEsportTeam.Add(z5);


            TeamMates z6 = new TeamMates("marcwils", "[TEB] marcwils");
            ourEsportTeam.Add(z6);


            TeamMates z7 = new TeamMates("Killerganon", "[TEB] killerganon#2125");
            ourEsportTeam.Add(z7);

            TeamMates z8 = new TeamMates("rogbros", "[TEB] rogbros");
            ourEsportTeam.Add(z8);



            TeamMates z11 = new TeamMates("Idris_98", "idris_98#5579");
            ourEsportTeam.Add(z11);

            TeamMates z12 = new TeamMates("Stasi0", "[TEB] stasi0");
            ourEsportTeam.Add(z12);

            TeamMates z13 = new TeamMates("ArtNhr", "ArtNhr#9682");
            ourEsportTeam.Add(z13);

            TeamMates z14 = new TeamMates("HotAndrej2001", "[TEB] HotAndrej2001");
            ourEsportTeam.Add(z14);

            TeamMates z15 = new TeamMates("NaDa__", "NaDa");
            ourEsportTeam.Add(z15);



            //////
            /////////
            TeamMates a2 = new TeamMates("ili_DIANN", "[TEB] ili_DIANN");
            ourNonEsportTeam.Add(a2);

            TeamMates b2 = new TeamMates("Drobniak12", "AlanitoBandito#9832");
            ourNonEsportTeam.Add(b2);

            TeamMates c2 = new TeamMates("TroVnut", "TroVNut#6908");
            ourNonEsportTeam.Add(c2);


            //TeamMates e2 = new TeamMates("EmpressBlackSun", "[TEB] EmpressBlackSun");
            //ourNonEsportTeam.Add(e2);


            TeamMates g2 = new TeamMates("Crythene", "[TEB]Crythene");
            ourNonEsportTeam.Add(g2);


            TeamMates h2 = new TeamMates("bwoodhouse_gwent", "[TEB] Brent");
            ourNonEsportTeam.Add(h2);

            TeamMates i2 = new TeamMates("manekk", "LiveCrotch");
            ourNonEsportTeam.Add(i2);

            TeamMates j2 = new TeamMates("BiggieO", "[TEB] BiggieO");
            ourNonEsportTeam.Add(j2);


            TeamMates j3 = new TeamMates("HerrDawix", "[TEB] HerrDawix");
            ourNonEsportTeam.Add(j3);


            TeamMates j4 = new TeamMates("Rykov_", "[TEB] Rykov_");
            ourNonEsportTeam.Add(j4);


            progressBar.Maximum = ourEsportTeam.Count + ourNonEsportTeam.Count;
            progressBar.Value = 0;

            rankingsTextField.Text = "";

            Thread thread = new Thread(new ThreadStart(checkRanking));
            thread.Start();


            progressBar.Value = 0;
        }

        private void checkRanking()
        {



            foreach (TeamMates tm in ourEsportTeam)
            {
               // tm.Player = PlayGwent.pullStatistics(tm.GogNick);
                tm.Player = PlayGwent.pullAllSeasonsStatistics(tm.GogNick);

                Dispatcher.Invoke(() =>
                {
                    progressBar.Value = progressBar.Value + 1;
                });
            }

            ourEsportTeam = ourEsportTeam.OrderBy(o => o.Player.LadderPosition).ToList();

                saveTeamStatsToFile("Esport section", ourEsportTeam, true);


            foreach (TeamMates tm in ourNonEsportTeam)
            {
                tm.Player = PlayGwent.pullAllSeasonsStatistics(tm.GogNick);

                Dispatcher.Invoke(() =>
                {
                    progressBar.Value = progressBar.Value + 1;
                });


            }

            ourNonEsportTeam = ourNonEsportTeam.OrderBy(o => o.Player.LadderPosition).ToList();


                saveTeamStatsToFile("Other sections", ourNonEsportTeam, false);

            Dispatcher.Invoke(() =>
            {
                System.Windows.Clipboard.SetText(rankingsTextField.Text);

            });

        }

        private void addTextLine(string text)
        {
            Dispatcher.Invoke(() =>
            {
                rankingsTextField.Text = rankingsTextField.Text + text + System.Environment.NewLine;
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


    }
}
