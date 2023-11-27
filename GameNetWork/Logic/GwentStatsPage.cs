using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace MadGains.Logic
{
    public static class GwentStatsPage
    {
        public static string getWebsiteTextAfterScripts(string url)
        {
            WebBrowser wb = new WebBrowser();
            wb.ScriptErrorsSuppressed = true;

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

            return wb.DocumentText;

        }

        private static void wb_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            WebBrowser wb = sender as WebBrowser;





        }
    }


}
