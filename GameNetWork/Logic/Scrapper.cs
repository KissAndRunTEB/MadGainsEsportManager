using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;

namespace MadGains.Logic
{
    internal class Scrapper
    {
        //informatyk płakał jak oglądał


        public static List<string> GetStats(string url)
        {
            _ = new List<string>();
            List<string> stats = WebScrapper(url);
            stats.AddRange(Txt_scrapper(url));
            return stats;

        }

        //current stats such a position, mmr, wins length 8
        public static List<string> Txt_scrapper(string url)
        {
            try
            {
                //html to string
                HttpClient client = new HttpClient();
                HttpResponseMessage response = client.GetAsync(url).Result;
                HttpContent content = response.Content;
                string txt = content.ReadAsStringAsync().Result;
                //array for stuff
                string[] final = new string[6];

                //position info
                int start_pos = txt.IndexOf(">Position:");
                int end_pos = txt.IndexOf("</strong></div><div");
                var position = txt[start_pos..end_pos];
                position = position.Replace(">Position:", "").Replace("<strong>", "").Trim();

                //mmr info
                int start_mmr = txt.IndexOf(">MMR:");
                int end_mmr = txt.IndexOf("</strong></div></div><div c");
                var mmr = txt[start_mmr..end_mmr];
                mmr = mmr.Replace(">MMR:", "").Replace("<strong>", "").Replace(",", "").Trim();

                //faction wins info
                int start_factions = txt.IndexOf("DataCurrent");
                int end_factions = txt.IndexOf("var winsBtn");
                var stats = txt[start_factions..end_factions];
                stats = stats[25..];
                stats = stats.Replace("{", "").Replace("}", "").Replace("[", "").Replace("]", "").Replace(",", "").Replace("\u0022", "").Replace("count", "")
                    .Replace("factions", "").Replace(":", "").Replace(";", "").Trim();
                var wins = stats.Split("slug");

                //making array and transforming it to list
                List<string> replace_list = new List<string>() { "monsters", "nilfgaard", "northernrealms", "scoiatael", "skellige", "syndicate" };
                for (int i = 0; i < 6; i++)
                {
                    final[i] = wins[i + 1].Replace(replace_list[i], "");
                }
                List<string> txt_stats = final.ToList();
                txt_stats.Add(mmr);
                txt_stats.Add(position);
                //mo ng nr sc sk sy mmr position
                return txt_stats;
            }
            catch
            {
                List<string> error = new List<string>() { "error", "error", "error", "error", "error", "error", "error", "error", "error" };
                return error;
            }
        }

        // matches of  mo, ng, nr, sc, sk, sy then global of season wins losses draws
        public static List<string> WebScrapper(string url)
        {
            try
            {
                var web = new HtmlWeb();
                var document = web.Load(url);

                var table = document.QuerySelectorAll("tbody tr").SkipLast(1).Skip(8);

                List<string> replace_list = new List<string>() { "Monsters", "Nilfgaard", "Northern Realms", "Scoia&#039;tael", "Skellige", "Syndicate", "Wins", "Losses", "Draws", };
                string[] web_stats = new string[9];
                foreach (var item in table)
                {
                    var name = item.InnerText.Replace("matches", ""); //
                    for (int j = 0; j < replace_list.Count; j++)
                    {
                        if (name.Contains(replace_list[j]))
                        {
                            web_stats[j] = name.Replace(replace_list[j], "").Trim();
                        }
                    }
                }
                List<string> final_stats = web_stats.ToList();

                return final_stats;
            }
            catch
            {
                List<string> error = new List<string>() { "error", "error", "error", "error", "error", "error", "error" };
                return error;
            }
        }


    }
}
