using MadGains.Data;
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
    public partial class Positions : Window
    {
            List<TeamMates> ourEsportTeam = new List<TeamMates>();
            List<TeamMates> ourNonEsportTeam = new List<TeamMates>();

        public Positions()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //TeamMates a = new TeamMates("KissAndRun", "[TEB] KissAndRun");
            //ourEsportTeam.Add(a);

            ourEsportTeam = ElderBloodTeam.listofEsport();

            ourNonEsportTeam = ElderBloodTeam.listofNotEsport();



 
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

                tm.Player = PlayGwent.pullStatistics(tm.GogNick);


                Dispatcher.Invoke(() =>
                {
                    progressBar.Value = progressBar.Value + 1;
                });
            }

            ourEsportTeam = ourEsportTeam.OrderBy(o => o.Player.LadderPosition).ToList();
            ourEsportTeam = ourEsportTeam.OrderBy(o => o.Player.LadderPosition).ToList();
            ourEsportTeam = ourEsportTeam.OrderBy(o => o.Player.LadderPosition).ToList();

            addTextLine("Top "+ElderBloodTeam.where_cut_off.ToString()+" players");
            addTextLine("Stats from: "+DateTime.Now.ToString("yyyy-dd-MM hh:ss"));
            addTextLine("");

            saveTeamStatsToFile("Esport section", ourEsportTeam, true);


            foreach (TeamMates tm in ourNonEsportTeam)
            {
                tm.Player = PlayGwent.pullStatistics(tm.GogNick);

                Dispatcher.Invoke(() =>
                {
                    progressBar.Value = progressBar.Value + 1;
                });


            }

            ourNonEsportTeam = ourNonEsportTeam.OrderBy(o => o.Player.LadderPosition).ToList();
            ourNonEsportTeam = ourNonEsportTeam.OrderBy(o => o.Player.LadderPosition).ToList();
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
        private void addTextWithoutLine(string text)
        {
            Dispatcher.Invoke(() =>
            {
                rankingsTextField.Text = rankingsTextField.Text + text;
            });
        }
        private void saveTeamStatsToFile(string teamName, List<TeamMates> team, bool tag)
        {
            addTextLine(teamName);

            //addTextLine(DateTime.Now.ToString("yyyy-dd-MM"));
            addTextLine("");

            foreach (TeamMates tm in team)
            {
                tm.Player = PlayGwent.pullStatistics(tm.GogNick);



                if (tm.Player.LadderPosition!=0 && tm.Player.LadderPosition < (ElderBloodTeam.where_cut_off +1 ))
                    {
                        double a = tm.Player.Wins;
                        double b = tm.Player.Wins + tm.Player.Losses + tm.Player.Draws;
                        double wr = a / b;

                        wr = wr * 100;

                        double aaa = Math.Round(wr, 0);

                        string tagAT = "";
                        if (tag)
                        {
                            tagAT = "@";
                        }

                   // addTextLine(tagAT + tm.DiscordNick + ": " + tm.Player.LadderPosition + "      WR: " + aaa + "% ( T4WR: " + tm.Top4factionWR + " %" + ", LEI: " + tm.Lei + "%)");


                    addTextLine(tagAT + tm.DiscordNick + ": " + tm.Player.LadderPosition + "      WR: " + aaa + "%");

                    ExtendedStats staty = new ExtendedStats("https://www.playgwent.com/en/profile/" + tm.GogNick);

                    addTextLine(staty.WriteInfo());
                }

            }



                addTextLine("");
                addTextLine("Not found in top "+ElderBloodTeam.where_cut_off+": ");


            //writing who is not in prorank
            foreach (TeamMates tm in team)
            {
                tm.Player = PlayGwent.pullStatistics(tm.GogNick);



                    if (tm.Player.LadderPosition > ElderBloodTeam.where_cut_off || tm.Player.LadderPosition==0)
                    {
                    addTextWithoutLine(tm.DiscordNick + ", ");
                    }
     
            }



                addTextLine("");
                addTextLine("");
     
        }


    }
}
