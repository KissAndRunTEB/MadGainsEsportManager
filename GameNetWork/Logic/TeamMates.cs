using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace MadGains.Logic
{
    class TeamMates
    {
        string gogNick;
        string discordNick;

        Player player;

        string top4factionWR;
        string lei;

        public TeamMates(string gog, string discord)
        {
            this.GogNick = gog;
            this.DiscordNick = discord;

            this.Player = new Player(gog);

            this.Top4factionWR = "Top FactionWR Error";
            this.Lei = "LEI Error";

          //  this.getStats();
        }

        public string GogNick { get => gogNick; set => gogNick = value; }
        public string DiscordNick { get => discordNick; set => discordNick = value; }
        public Player Player { get => player; set => player = value; }
        public string Top4factionWR { get => top4factionWR; set => top4factionWR = value; }
        public string Lei { get => lei; set => lei = value; }

        public void getStats()
            {
            string url = "https://www.gwentdata.com/player/"+this.GogNick;
            var html = @url;

             string aaa= GetPagePhantomJsKissVersion(url);


            string htmlWygenerowany = aaa;

            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(htmlWygenerowany);

            //HtmlWeb web = new HtmlWeb();

            //var htmlDoc = web.Load(html);

            // Player p = new Player(this.GogNick);

            var node = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='player-info-span']");

            if (node != null)
            {
                string innerText = node.InnerText;
                string[] pieces = innerText.Split();


                try
                {

                    // p.LadderPosition = Int32.Parse(pieces[pieces.Length - 1].Replace(",", ""));

                    node = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='l-player-details__table-mmr']");
                    innerText = node.InnerText;
                    pieces = innerText.Split();

                }

                catch (System.FormatException)
                {

                    this.Top4factionWR = "Top FactionWR Error 2";
                    this.Lei = "LEI Error 2";
                }

            }


        }

        public static string GetPagePhantomJsKissVersion(string url)
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                client.DefaultRequestHeaders.ExpectContinue = false;
                var pageRequestJson = new System.Net.Http.StringContent(@"{'url':'" + url + "','renderType':'html','outputAsJson':false }");
                var response = client.PostAsync("https://PhantomJsCloud.com/api/browser/v2/ak-r9hft-vm1x5-qdfxq-s2wnw-m1702/", pageRequestJson).Result;
                return response.Content.ReadAsStringAsync().Result;
            }
        }

        public void getWebsiteTextAfterScripts(string url)
        {
            WebBrowser wb = new WebBrowser();
         //   wb.ScriptErrorsSuppressed = true;

            wb.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(wb_DocumentCompleted);

            wb.Navigate(url);

            while (wb.ReadyState != WebBrowserReadyState.Complete)
            {
                Application.DoEvents();
            }

            //  string html  = "";

            //string html = "";

            //try
            //{
            //    using (WebClient client = new WebClient())
            //    {
            //        client.Headers["User-Agent"] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.1.12) Gecko/20100824 Firefox/3.5.12x";
            //        client.Encoding = Encoding.UTF8;
            //        html = client.DownloadString(url);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    // handle error
            //    Console.WriteLine(ex.Message);
            //}

        }

        private void wb_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            WebBrowser wb = sender as WebBrowser;



            string htmlWygenerowany = wb.DocumentText;

            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(htmlWygenerowany);

            //HtmlWeb web = new HtmlWeb();

            //var htmlDoc = web.Load(html);

            // Player p = new Player(this.GogNick);

            var node = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='player-info-span']");

            if (node != null)
            {
                string innerText = node.InnerText;
                string[] pieces = innerText.Split();


                try
                {

                    // p.LadderPosition = Int32.Parse(pieces[pieces.Length - 1].Replace(",", ""));

                    node = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='l-player-details__table-mmr']");
                    innerText = node.InnerText;
                    pieces = innerText.Split();

                }

                catch (System.FormatException)
                {

                    this.Top4factionWR = "Top FactionWR Error 2";
                    this.Lei = "LEI Error 2";
                }

            }


        }


    }
}
