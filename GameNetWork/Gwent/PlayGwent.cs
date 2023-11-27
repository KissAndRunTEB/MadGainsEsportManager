using HtmlAgilityPack;
using MadGains.Data;
using MadGains.Logic;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MadGains.Gwent
{
    static class PlayGwent
    {
        public static string masters = ElderBloodTeam.masters;
        public static string season = ElderBloodTeam.season;

        // Change second index in url to review a different set of 20 names
        public static string urlPlayersList = "https://masters.playgwent.com/en/rankings/" + masters + "/" + season + "/2/";

        static public string urlPlayer(string nick)
        {
            return "https://www.playgwent.com/en/profile/" + nick;
        }

        // Note nicknames with foreign characters may cause issues.  
        static public List<string> createListOfNicks(int pages)
        {

            List<string> list = new List<string>();
            for (int page = 1; page < pages + 1; page++)
            {
                try
                {
                    string url = urlPlayersList + page.ToString();
                    var html = @url;

                    HtmlWeb web = new HtmlWeb();

                    var htmlDoc = web.Load(html);

                    string doubleTableClassName = "c-ranking__content c-rankings c-rankings--double";
                    var tables = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='" + doubleTableClassName + "']");
                    var proTable = tables
                        .ChildNodes[0]
                        .ChildNodes[0]
                        .ChildNodes[1]
                        .ChildNodes[0]
                        .ChildNodes[0]
                        .ChildNodes[2];

                    foreach (var row in proTable.ChildNodes)
                    {
                        var divNick = row.
                            ChildNodes[0].
                            ChildNodes[1].
                            ChildNodes[1];
                        string nick = divNick.InnerText;
                        list.Add(nick);
                    }
                }
                catch
                {
                    // Reached end of table early before its maximum size
                    break;
                }
            }

            return list;
        }

        static public Player pullStatistics(string nick)
        {
            string url = PlayGwent.urlPlayer(nick);
            var html = @url;

            HtmlWeb web = new HtmlWeb();

            var htmlDoc = web.Load(html);

            Player p = new Player(nick);



            var node = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='l-player-details__table-position']");

            if (node != null)
            {
                string innerText = node.InnerText;
                string[] pieces = innerText.Split();


                try
                {

                    p.LadderPosition = Int32.Parse(pieces[pieces.Length - 1].Replace(",", ""));

                    node = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='l-player-details__table-mmr']");
                    innerText = node.InnerText;
                    pieces = innerText.Split();
                    p.Mmr = Int32.Parse(pieces[pieces.Length - 1].Replace(",", ""));

                    // recover win data by faction for current season
                    string script = htmlDoc.DocumentNode.Descendants()
                                     .Where(n => n.Name == "script")
                                     .Last().InnerText;

                    int i = script.IndexOf("profileDataCurrent");
                    string relevantData = script.Substring(i + 21).Split(';')[0];

                    dynamic data = JObject.Parse(relevantData);

                    p.Wins = data["overall"];
                    foreach (var entry in data["factions"])
                    {
                        if (entry["slug"] == "monsters")
                        {
                            p.MOWins = entry["count"];
                        }
                        if (entry["slug"] == "nilfgaard")
                        {
                            p.NGWins = entry["count"];
                        }
                        if (entry["slug"] == "northernrealms")
                        {
                            p.NRWins = entry["count"];
                        }
                        if (entry["slug"] == "scoiatael")
                        {
                            p.STWins = entry["count"];
                        }
                        if (entry["slug"] == "skellige")
                        {
                            p.SKWins = entry["count"];
                        }
                        if (entry["slug"] == "syndicate")
                        {
                            p.SYWins = entry["count"];
                        }
                    }

                    // recover match data for current season
                    var table = htmlDoc.DocumentNode.SelectSingleNode("//table[@class='c-statistics-table current-ranked']").ChildNodes[1];
                    int index = 0;
                    foreach (var row in table.ChildNodes)
                    {
                        if (index == 10)
                        {
                            break;
                        }
                        string factionString = row.ChildNodes[0].InnerText;

                        int number = 0;
                        if (index > 0)
                        {
                            string parsedString = row.ChildNodes[1].InnerText.Split(' ')[0].Replace(",", "");
                            number = Int32.Parse(parsedString);
                        }
                        else
                        {
                            string parsedString = row.ChildNodes[1].SelectSingleNode("//span[@class='profile-matches']").InnerText.Split(' ')[0].Replace(",", "");
                            number = Int32.Parse(parsedString);
                        }

                        if (index == 0)
                        {
                            p.Matches = number;
                        }
                        else if (index == 2)
                        {
                            p.Losses = number;
                        }
                        else if (index == 3)
                        {
                            p.Draws = number;
                        }

                        if (factionString.Contains("Northern Realms"))
                        {
                            p.NRMatches = number;
                        }
                        else if (factionString.Contains("Skellige"))
                        {
                            p.SKMatches = number;
                        }
                        else if (factionString.Contains("Syndicate"))
                        {
                            p.SYMatches = number;
                        }
                        else if (factionString.Contains("Scoia"))
                        {
                            p.STMatches = number;
                        }
                        else if (factionString.Contains("Nilfgaard"))
                        {
                            p.NGMatches = number;
                        }
                        else if (factionString.Contains("Monsters"))
                        {
                            p.MOMatches = number;
                        }

                        index += 1;
                    }
                }
                catch (System.FormatException)
                {

                    p.LadderPosition = 0;
                }

            }

            else
            {
                p.LadderPosition = 0;
            }

            return p;
        }


        static public Player pullAllSeasonsStatistics(string nick)
        {
            List<string> seasons = new List<string>();
            seasons.Add("season-of-the-wolf");
            seasons.Add("season-of-the-love");
            seasons.Add("season-of-the-bear");
            seasons.Add("season-of-the-elf");
            seasons.Add("season-of-the-viper");
            seasons.Add("season-of-the-magic");
            seasons.Add("season-of-the-griffin");
            seasons.Add("season-of-the-draconid");



            Player p = new Player(nick);


            foreach (string season in seasons)
            {
                string url = "https://masters.playgwent.com/en/rankings/masters-3/" + season + "/1/1/" + nick;



                var html = @url;

                HtmlWeb web = new HtmlWeb();

                var htmlDoc = web.Load(html);



                var node = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='td-number']");

                if (node != null)
                {
                    string innerText = node.InnerText;
                    string[] pieces = innerText.Split();


                    try
                    {

                        //p.LadderPosition = Int32.Parse(pieces[pieces.Length - 1].Replace(",", ""));

                        int position = Int32.Parse(innerText);

                        p.LadderPositionThrewSeasons.Add(position);

                        p.LadderPosition = position;

                    }
                    catch (System.FormatException)
                    {
                        p.LadderPositionThrewSeasons.Add(0);
                        p.LadderPosition = 0;
                    }

                }

                else
                {
                    p.LadderPositionThrewSeasons.Add(0);
                    p.LadderPosition = 0;
                }


            } 

            return p;
        }

        // Retrieve list of deck urls for decks that players have uploaded to the tournament site
        static public List<string> pull_deck_links(string url)
        {
            List<string> answers = new List<string>();

            string html = GetPagePhantomJs(url);

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var htmlBody = htmlDoc.DocumentNode.SelectSingleNode("//body");

            //Console.WriteLine(htmlBody.OuterHtml);

            var table = htmlDoc.DocumentNode.SelectSingleNode("//table[@class='tournament-participants__table table']");

            var node = table.ChildNodes[1];

            //int count = 0;
            foreach (var row in node.ChildNodes)
            {
                //count += 1;
                var node_2 = row.ChildNodes[2];
                //int count_2 = 0;
                foreach (var entry in node_2.ChildNodes)
                {
                    //count_2 += 1;
                    string deck_url = entry.GetAttributeValue("href", "");
                    answers.Add(deck_url);
                }
                //Console.WriteLine(count_2);
            }
            //Console.WriteLine(count);

            return answers;
        }


        static public List<TournamentDeckEntry> pull_deck_linksVersion2(string url)
        {
            List<TournamentDeckEntry> answers = new List<TournamentDeckEntry>();

            string html = GetPagePhantomJs(url);

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var htmlBody = htmlDoc.DocumentNode.SelectSingleNode("//body");

            //Console.WriteLine(htmlBody.OuterHtml);

            var table = htmlDoc.DocumentNode.SelectSingleNode("//table[@class='tournament-participants__table table']");

            var node = table.ChildNodes[1];

            //int count = 0;
            foreach (var row in node.ChildNodes)
            {
                TournamentDeckEntry entry = new TournamentDeckEntry();

                //count += 1;
                //var node_2 = row.ChildNodes[3]; old version
                //var node_2 = row.ChildNodes[2]; less old versioon
                //  var node_2 = row.ChildNodes[3];
                // var node_2 = row.ChildNodes[2];

                var node_2 = row.ChildNodes[2];//3 for qually top16


                int deckNumber = 1;

                entry.Player = row.ChildNodes[1].InnerText;

                entry.Country = getCountry(entry.Player);

                //int count_2 = 0;
                foreach (var deck in node_2.ChildNodes)
                {
                   // HtmlNode deck2 = deck.ChildNodes[1]; //added after they change on playgwent

                    //count_2 += 1;
                    string deck_url = deck.GetAttributeValue("href", "");
                    //   answers.Add(deck_url);

                    string deck_name = deck.GetAttributeValue("title", "");

                    switch (deckNumber)
                    {
                        case 1:
                            entry.DeckA = deck_url;
                            entry.FactionA = deck_name;
                            break;

                        case 2:
                            entry.DeckB = deck_url;
                            entry.FactionB = deck_name;
                            break;

                        case 3:
                            entry.DeckC = deck_url; 
                            entry.FactionC = deck_name;
                            break;

                        case 4:
                            entry.DeckD = deck_url;
                            entry.FactionD = deck_name;
                            break;

                        default:
                            break;
                    }


                    deckNumber++;
                }

                answers.Add(entry);
                //Console.WriteLine(count_2);
            }
            //Console.WriteLine(count);

            return answers;
        }

        public static string getCountry(string player)
        {
            string url = "https://masters.playgwent.com/en/rankings/masters-4/" + season + "/1/1/" + player;

            //  string html = GetPagePhantomJs(url);

            //   string url = "https://masters.playgwent.com/en/rankings/masters-3/" + season + "/1/1/" + nick;



            var html = @url;

            HtmlWeb web = new HtmlWeb();

            var htmlDoc = web.Load(html);




            var htmlBody = htmlDoc.DocumentNode.SelectSingleNode("//body");

            //Console.WriteLine(htmlBody.OuterHtml);

            var table = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='c-ranking-table__tr']");

            var node = table.ChildNodes[1];

            string country = node.InnerHtml;



            String result = cutFromTo("\"", "\"", country);

            country = result.Replace("flag-icon flag-icon-", "");

            country = country.ToUpper();

            Collection<ISO3166Country> collection = BuildCollection();
            string aaa = collection.FirstOrDefault(p => p.Alpha2 == country).Name;


            string nickWyciety = node.InnerHtml;

            nickWyciety = cutFromTo("<strong>", "</strong>", nickWyciety);

            string defValue = "-";

if(nickWyciety.ToLower() != player.ToLower())
            {
                aaa = defValue;
            }

            switch (player)
            {
                case "chase":
                    aaa = "Russia";
                    break;

                case "Gravesh":
                    aaa = "Germany";
                    break;

                case "iluxa228":
                    aaa = "Russia";
                    break;

                case "Infernale":
                    aaa = "Netherland";
                    break;

                case "Kerpeten":
                    aaa = "Turkey";
                    break;

                case "ILBESTIOO":
                    aaa = "Italy";
                    break;


                case "McPoyle":
                    aaa = "Latvia";
                    break;

                case "MrDisbalance":
                    aaa = "Russia";
                    break;

                case "sartndf":
                    aaa = "Russia";
                    break;

                case "TN_Frailcoast":
                    aaa = "Spain";
                    break;

                case "xanaxrehabclub":
                    aaa = "Ukraine";
                    break;

                case "Pajabol":
                    aaa = "Poland";
                    break;


                case "上分":
                    aaa = "Australia";
                    break;

                //case "南瓜":
                //    aaa = "-";
                //    break;


                //case "cq5716289":
                //    aaa = "-";
                //    break;

                //case "licy1005":
                //    aaa = "-";
                //    break;

                case "Puzzle_Express":
                    aaa = "Great Britain";
                    break;



                case "Clauz":
                    aaa = "Italy";
                    break;

                case "elquellora":
                    aaa = "Italy";
                    break;

                case "flav_iio":
                    aaa = "Canada";
                    break;

                case "DARKFANTAA":
                    aaa = "Ukraine";
                    break;

                case "FraNico":
                    aaa = "Espaniol";
                    break;

                case "myamon":
                    aaa = "Japan";
                    break;

                case "Spyro_ZA":
                    aaa = "South Africa";
                    break;



                default:
                    break;
            }

            if (aaa == "")
            {
                aaa = defValue;
            }

            return aaa;
        }

        private static string cutFromTo(string from, string to, string word)
        {
            int pFrom = word.IndexOf(from) + from.Length;
            int pTo = word.LastIndexOf(to);

            return word.Substring(pFrom, pTo - pFrom);
        }

        public class ISO3166Country
        {
            public ISO3166Country(string name, string alpha2, string alpha3, int numericCode)
            {
                this.Name = name;
                this.Alpha2 = alpha2;
                this.Alpha3 = alpha3;
                this.NumericCode = numericCode;
            }

            public string Name { get; private set; }

            public string Alpha2 { get; private set; }

            public string Alpha3 { get; private set; }

            public int NumericCode { get; private set; }
        }

        private static Collection<ISO3166Country> BuildCollection()
        {
            Collection<ISO3166Country> collection = new Collection<ISO3166Country>();

            // This collection built from Wikipedia entry on ISO3166-1 on 9th Feb 2016

            collection.Add(new ISO3166Country("Afghanistan", "AF", "AFG", 4));
            collection.Add(new ISO3166Country("Åland Islands", "AX", "ALA", 248));
            collection.Add(new ISO3166Country("Albania", "AL", "ALB", 8));
            collection.Add(new ISO3166Country("Algeria", "DZ", "DZA", 12));
            collection.Add(new ISO3166Country("American Samoa", "AS", "ASM", 16));
            collection.Add(new ISO3166Country("Andorra", "AD", "AND", 20));
            collection.Add(new ISO3166Country("Angola", "AO", "AGO", 24));
            collection.Add(new ISO3166Country("Anguilla", "AI", "AIA", 660));
            collection.Add(new ISO3166Country("Antarctica", "AQ", "ATA", 10));
            collection.Add(new ISO3166Country("Antigua and Barbuda", "AG", "ATG", 28));
            collection.Add(new ISO3166Country("Argentina", "AR", "ARG", 32));
            collection.Add(new ISO3166Country("Armenia", "AM", "ARM", 51));
            collection.Add(new ISO3166Country("Aruba", "AW", "ABW", 533));
            collection.Add(new ISO3166Country("Australia", "AU", "AUS", 36));
            collection.Add(new ISO3166Country("Austria", "AT", "AUT", 40));
            collection.Add(new ISO3166Country("Azerbaijan", "AZ", "AZE", 31));
            collection.Add(new ISO3166Country("Bahamas", "BS", "BHS", 44));
            collection.Add(new ISO3166Country("Bahrain", "BH", "BHR", 48));
            collection.Add(new ISO3166Country("Bangladesh", "BD", "BGD", 50));
            collection.Add(new ISO3166Country("Barbados", "BB", "BRB", 52));
            collection.Add(new ISO3166Country("Belarus", "BY", "BLR", 112));
            collection.Add(new ISO3166Country("Belgium", "BE", "BEL", 56));
            collection.Add(new ISO3166Country("Belize", "BZ", "BLZ", 84));
            collection.Add(new ISO3166Country("Benin", "BJ", "BEN", 204));
            collection.Add(new ISO3166Country("Bermuda", "BM", "BMU", 60));
            collection.Add(new ISO3166Country("Bhutan", "BT", "BTN", 64));
            collection.Add(new ISO3166Country("Bolivia (Plurinational State of)", "BO", "BOL", 68));
            collection.Add(new ISO3166Country("Bonaire, Sint Eustatius and Saba", "BQ", "BES", 535));
            collection.Add(new ISO3166Country("Bosnia and Herzegovina", "BA", "BIH", 70));
            collection.Add(new ISO3166Country("Botswana", "BW", "BWA", 72));
            collection.Add(new ISO3166Country("Bouvet Island", "BV", "BVT", 74));
            collection.Add(new ISO3166Country("Brazil", "BR", "BRA", 76));
            collection.Add(new ISO3166Country("British Indian Ocean Territory", "IO", "IOT", 86));
            collection.Add(new ISO3166Country("Brunei Darussalam", "BN", "BRN", 96));
            collection.Add(new ISO3166Country("Bulgaria", "BG", "BGR", 100));
            collection.Add(new ISO3166Country("Burkina Faso", "BF", "BFA", 854));
            collection.Add(new ISO3166Country("Burundi", "BI", "BDI", 108));
            collection.Add(new ISO3166Country("Cabo Verde", "CV", "CPV", 132));
            collection.Add(new ISO3166Country("Cambodia", "KH", "KHM", 116));
            collection.Add(new ISO3166Country("Cameroon", "CM", "CMR", 120));
            collection.Add(new ISO3166Country("Canada", "CA", "CAN", 124));
            collection.Add(new ISO3166Country("Cayman Islands", "KY", "CYM", 136));
            collection.Add(new ISO3166Country("Central African Republic", "CF", "CAF", 140));
            collection.Add(new ISO3166Country("Chad", "TD", "TCD", 148));
            collection.Add(new ISO3166Country("Chile", "CL", "CHL", 152));
            collection.Add(new ISO3166Country("China", "CN", "CHN", 156));
            collection.Add(new ISO3166Country("Christmas Island", "CX", "CXR", 162));
            collection.Add(new ISO3166Country("Cocos (Keeling) Islands", "CC", "CCK", 166));
            collection.Add(new ISO3166Country("Colombia", "CO", "COL", 170));
            collection.Add(new ISO3166Country("Comoros", "KM", "COM", 174));
            collection.Add(new ISO3166Country("Congo", "CG", "COG", 178));
            collection.Add(new ISO3166Country("Congo (Democratic Republic of the)", "CD", "COD", 180));
            collection.Add(new ISO3166Country("Cook Islands", "CK", "COK", 184));
            collection.Add(new ISO3166Country("Costa Rica", "CR", "CRI", 188));
            collection.Add(new ISO3166Country("Côte d'Ivoire", "CI", "CIV", 384));
            collection.Add(new ISO3166Country("Croatia", "HR", "HRV", 191));
            collection.Add(new ISO3166Country("Cuba", "CU", "CUB", 192));
            collection.Add(new ISO3166Country("Curaçao", "CW", "CUW", 531));
            collection.Add(new ISO3166Country("Cyprus", "CY", "CYP", 196));
            collection.Add(new ISO3166Country("Czech Republic", "CZ", "CZE", 203));
            collection.Add(new ISO3166Country("Denmark", "DK", "DNK", 208));
            collection.Add(new ISO3166Country("Djibouti", "DJ", "DJI", 262));
            collection.Add(new ISO3166Country("Dominica", "DM", "DMA", 212));
            collection.Add(new ISO3166Country("Dominican Republic", "DO", "DOM", 214));
            collection.Add(new ISO3166Country("Ecuador", "EC", "ECU", 218));
            collection.Add(new ISO3166Country("Egypt", "EG", "EGY", 818));
            collection.Add(new ISO3166Country("El Salvador", "SV", "SLV", 222));
            collection.Add(new ISO3166Country("Equatorial Guinea", "GQ", "GNQ", 226));
            collection.Add(new ISO3166Country("Eritrea", "ER", "ERI", 232));
            collection.Add(new ISO3166Country("Estonia", "EE", "EST", 233));
            collection.Add(new ISO3166Country("Ethiopia", "ET", "ETH", 231));
            collection.Add(new ISO3166Country("Falkland Islands (Malvinas)", "FK", "FLK", 238));
            collection.Add(new ISO3166Country("Faroe Islands", "FO", "FRO", 234));
            collection.Add(new ISO3166Country("Fiji", "FJ", "FJI", 242));
            collection.Add(new ISO3166Country("Finland", "FI", "FIN", 246));
            collection.Add(new ISO3166Country("France", "FR", "FRA", 250));
            collection.Add(new ISO3166Country("French Guiana", "GF", "GUF", 254));
            collection.Add(new ISO3166Country("French Polynesia", "PF", "PYF", 258));
            collection.Add(new ISO3166Country("French Southern Territories", "TF", "ATF", 260));
            collection.Add(new ISO3166Country("Gabon", "GA", "GAB", 266));
            collection.Add(new ISO3166Country("Gambia", "GM", "GMB", 270));
            collection.Add(new ISO3166Country("Georgia", "GE", "GEO", 268));
            collection.Add(new ISO3166Country("Germany", "DE", "DEU", 276));
            collection.Add(new ISO3166Country("Ghana", "GH", "GHA", 288));
            collection.Add(new ISO3166Country("Gibraltar", "GI", "GIB", 292));
            collection.Add(new ISO3166Country("Greece", "GR", "GRC", 300));
            collection.Add(new ISO3166Country("Greenland", "GL", "GRL", 304));
            collection.Add(new ISO3166Country("Grenada", "GD", "GRD", 308));
            collection.Add(new ISO3166Country("Guadeloupe", "GP", "GLP", 312));
            collection.Add(new ISO3166Country("Guam", "GU", "GUM", 316));
            collection.Add(new ISO3166Country("Guatemala", "GT", "GTM", 320));
            collection.Add(new ISO3166Country("Guernsey", "GG", "GGY", 831));
            collection.Add(new ISO3166Country("Guinea", "GN", "GIN", 324));
            collection.Add(new ISO3166Country("Guinea-Bissau", "GW", "GNB", 624));
            collection.Add(new ISO3166Country("Guyana", "GY", "GUY", 328));
            collection.Add(new ISO3166Country("Haiti", "HT", "HTI", 332));
            collection.Add(new ISO3166Country("Heard Island and McDonald Islands", "HM", "HMD", 334));
            collection.Add(new ISO3166Country("Holy See", "VA", "VAT", 336));
            collection.Add(new ISO3166Country("Honduras", "HN", "HND", 340));
            collection.Add(new ISO3166Country("Hong Kong", "HK", "HKG", 344));
            collection.Add(new ISO3166Country("Hungary", "HU", "HUN", 348));
            collection.Add(new ISO3166Country("Iceland", "IS", "ISL", 352));
            collection.Add(new ISO3166Country("India", "IN", "IND", 356));
            collection.Add(new ISO3166Country("Indonesia", "ID", "IDN", 360));
            collection.Add(new ISO3166Country("Iran (Islamic Republic of)", "IR", "IRN", 364));
            collection.Add(new ISO3166Country("Iraq", "IQ", "IRQ", 368));
            collection.Add(new ISO3166Country("Ireland", "IE", "IRL", 372));
            collection.Add(new ISO3166Country("Isle of Man", "IM", "IMN", 833));
            collection.Add(new ISO3166Country("Israel", "IL", "ISR", 376));
            collection.Add(new ISO3166Country("Italy", "IT", "ITA", 380));
            collection.Add(new ISO3166Country("Jamaica", "JM", "JAM", 388));
            collection.Add(new ISO3166Country("Japan", "JP", "JPN", 392));
            collection.Add(new ISO3166Country("Jersey", "JE", "JEY", 832));
            collection.Add(new ISO3166Country("Jordan", "JO", "JOR", 400));
            collection.Add(new ISO3166Country("Kazakhstan", "KZ", "KAZ", 398));
            collection.Add(new ISO3166Country("Kenya", "KE", "KEN", 404));
            collection.Add(new ISO3166Country("Kiribati", "KI", "KIR", 296));
            collection.Add(new ISO3166Country("Korea (Democratic People's Republic of)", "KP", "PRK", 408));
            collection.Add(new ISO3166Country("Korea (Republic of)", "KR", "KOR", 410));
            collection.Add(new ISO3166Country("Kuwait", "KW", "KWT", 414));
            collection.Add(new ISO3166Country("Kyrgyzstan", "KG", "KGZ", 417));
            collection.Add(new ISO3166Country("Lao People's Democratic Republic", "LA", "LAO", 418));
            collection.Add(new ISO3166Country("Latvia", "LV", "LVA", 428));
            collection.Add(new ISO3166Country("Lebanon", "LB", "LBN", 422));
            collection.Add(new ISO3166Country("Lesotho", "LS", "LSO", 426));
            collection.Add(new ISO3166Country("Liberia", "LR", "LBR", 430));
            collection.Add(new ISO3166Country("Libya", "LY", "LBY", 434));
            collection.Add(new ISO3166Country("Liechtenstein", "LI", "LIE", 438));
            collection.Add(new ISO3166Country("Lithuania", "LT", "LTU", 440));
            collection.Add(new ISO3166Country("Luxembourg", "LU", "LUX", 442));
            collection.Add(new ISO3166Country("Macao", "MO", "MAC", 446));
            collection.Add(new ISO3166Country("Macedonia (the former Yugoslav Republic of)", "MK", "MKD", 807));
            collection.Add(new ISO3166Country("Madagascar", "MG", "MDG", 450));
            collection.Add(new ISO3166Country("Malawi", "MW", "MWI", 454));
            collection.Add(new ISO3166Country("Malaysia", "MY", "MYS", 458));
            collection.Add(new ISO3166Country("Maldives", "MV", "MDV", 462));
            collection.Add(new ISO3166Country("Mali", "ML", "MLI", 466));
            collection.Add(new ISO3166Country("Malta", "MT", "MLT", 470));
            collection.Add(new ISO3166Country("Marshall Islands", "MH", "MHL", 584));
            collection.Add(new ISO3166Country("Martinique", "MQ", "MTQ", 474));
            collection.Add(new ISO3166Country("Mauritania", "MR", "MRT", 478));
            collection.Add(new ISO3166Country("Mauritius", "MU", "MUS", 480));
            collection.Add(new ISO3166Country("Mayotte", "YT", "MYT", 175));
            collection.Add(new ISO3166Country("Mexico", "MX", "MEX", 484));
            collection.Add(new ISO3166Country("Micronesia (Federated States of)", "FM", "FSM", 583));
            collection.Add(new ISO3166Country("Moldova (Republic of)", "MD", "MDA", 498));
            collection.Add(new ISO3166Country("Monaco", "MC", "MCO", 492));
            collection.Add(new ISO3166Country("Mongolia", "MN", "MNG", 496));
            collection.Add(new ISO3166Country("Montenegro", "ME", "MNE", 499));
            collection.Add(new ISO3166Country("Montserrat", "MS", "MSR", 500));
            collection.Add(new ISO3166Country("Morocco", "MA", "MAR", 504));
            collection.Add(new ISO3166Country("Mozambique", "MZ", "MOZ", 508));
            collection.Add(new ISO3166Country("Myanmar", "MM", "MMR", 104));
            collection.Add(new ISO3166Country("Namibia", "NA", "NAM", 516));
            collection.Add(new ISO3166Country("Nauru", "NR", "NRU", 520));
            collection.Add(new ISO3166Country("Nepal", "NP", "NPL", 524));
            collection.Add(new ISO3166Country("Netherlands", "NL", "NLD", 528));
            collection.Add(new ISO3166Country("New Caledonia", "NC", "NCL", 540));
            collection.Add(new ISO3166Country("New Zealand", "NZ", "NZL", 554));
            collection.Add(new ISO3166Country("Nicaragua", "NI", "NIC", 558));
            collection.Add(new ISO3166Country("Niger", "NE", "NER", 562));
            collection.Add(new ISO3166Country("Nigeria", "NG", "NGA", 566));
            collection.Add(new ISO3166Country("Niue", "NU", "NIU", 570));
            collection.Add(new ISO3166Country("Norfolk Island", "NF", "NFK", 574));
            collection.Add(new ISO3166Country("Northern Mariana Islands", "MP", "MNP", 580));
            collection.Add(new ISO3166Country("Norway", "NO", "NOR", 578));
            collection.Add(new ISO3166Country("Oman", "OM", "OMN", 512));
            collection.Add(new ISO3166Country("Pakistan", "PK", "PAK", 586));
            collection.Add(new ISO3166Country("Palau", "PW", "PLW", 585));
            collection.Add(new ISO3166Country("Palestine, State of", "PS", "PSE", 275));
            collection.Add(new ISO3166Country("Panama", "PA", "PAN", 591));
            collection.Add(new ISO3166Country("Papua New Guinea", "PG", "PNG", 598));
            collection.Add(new ISO3166Country("Paraguay", "PY", "PRY", 600));
            collection.Add(new ISO3166Country("Peru", "PE", "PER", 604));
            collection.Add(new ISO3166Country("Philippines", "PH", "PHL", 608));
            collection.Add(new ISO3166Country("Pitcairn", "PN", "PCN", 612));
            collection.Add(new ISO3166Country("Poland", "PL", "POL", 616));
            collection.Add(new ISO3166Country("Portugal", "PT", "PRT", 620));
            collection.Add(new ISO3166Country("Puerto Rico", "PR", "PRI", 630));
            collection.Add(new ISO3166Country("Qatar", "QA", "QAT", 634));
            collection.Add(new ISO3166Country("Réunion", "RE", "REU", 638));
            collection.Add(new ISO3166Country("Romania", "RO", "ROU", 642));
            collection.Add(new ISO3166Country("Russia", "RU", "RUS", 643));
            collection.Add(new ISO3166Country("Rwanda", "RW", "RWA", 646));
            collection.Add(new ISO3166Country("Saint Barthélemy", "BL", "BLM", 652));
            collection.Add(new ISO3166Country("Saint Helena, Ascension and Tristan da Cunha", "SH", "SHN", 654));
            collection.Add(new ISO3166Country("Saint Kitts and Nevis", "KN", "KNA", 659));
            collection.Add(new ISO3166Country("Saint Lucia", "LC", "LCA", 662));
            collection.Add(new ISO3166Country("Saint Martin (French part)", "MF", "MAF", 663));
            collection.Add(new ISO3166Country("Saint Pierre and Miquelon", "PM", "SPM", 666));
            collection.Add(new ISO3166Country("Saint Vincent and the Grenadines", "VC", "VCT", 670));
            collection.Add(new ISO3166Country("Samoa", "WS", "WSM", 882));
            collection.Add(new ISO3166Country("San Marino", "SM", "SMR", 674));
            collection.Add(new ISO3166Country("Sao Tome and Principe", "ST", "STP", 678));
            collection.Add(new ISO3166Country("Saudi Arabia", "SA", "SAU", 682));
            collection.Add(new ISO3166Country("Senegal", "SN", "SEN", 686));
            collection.Add(new ISO3166Country("Serbia", "RS", "SRB", 688));
            collection.Add(new ISO3166Country("Seychelles", "SC", "SYC", 690));
            collection.Add(new ISO3166Country("Sierra Leone", "SL", "SLE", 694));
            collection.Add(new ISO3166Country("Singapore", "SG", "SGP", 702));
            collection.Add(new ISO3166Country("Sint Maarten (Dutch part)", "SX", "SXM", 534));
            collection.Add(new ISO3166Country("Slovakia", "SK", "SVK", 703));
            collection.Add(new ISO3166Country("Slovenia", "SI", "SVN", 705));
            collection.Add(new ISO3166Country("Solomon Islands", "SB", "SLB", 90));
            collection.Add(new ISO3166Country("Somalia", "SO", "SOM", 706));
            collection.Add(new ISO3166Country("South Africa", "ZA", "ZAF", 710));
            collection.Add(new ISO3166Country("South Georgia and the South Sandwich Islands", "GS", "SGS", 239));
            collection.Add(new ISO3166Country("South Sudan", "SS", "SSD", 728));
            collection.Add(new ISO3166Country("Spain", "ES", "ESP", 724));
            collection.Add(new ISO3166Country("Sri Lanka", "LK", "LKA", 144));
            collection.Add(new ISO3166Country("Sudan", "SD", "SDN", 729));
            collection.Add(new ISO3166Country("Suriname", "SR", "SUR", 740));
            collection.Add(new ISO3166Country("Svalbard and Jan Mayen", "SJ", "SJM", 744));
            collection.Add(new ISO3166Country("Swaziland", "SZ", "SWZ", 748));
            collection.Add(new ISO3166Country("Sweden", "SE", "SWE", 752));
            collection.Add(new ISO3166Country("Switzerland", "CH", "CHE", 756));
            collection.Add(new ISO3166Country("Syrian Arab Republic", "SY", "SYR", 760));
            collection.Add(new ISO3166Country("Taiwan, Province of China[a]", "TW", "TWN", 158));
            collection.Add(new ISO3166Country("Tajikistan", "TJ", "TJK", 762));
            collection.Add(new ISO3166Country("Tanzania, United Republic of", "TZ", "TZA", 834));
            collection.Add(new ISO3166Country("Thailand", "TH", "THA", 764));
            collection.Add(new ISO3166Country("Timor-Leste", "TL", "TLS", 626));
            collection.Add(new ISO3166Country("Togo", "TG", "TGO", 768));
            collection.Add(new ISO3166Country("Tokelau", "TK", "TKL", 772));
            collection.Add(new ISO3166Country("Tonga", "TO", "TON", 776));
            collection.Add(new ISO3166Country("Trinidad and Tobago", "TT", "TTO", 780));
            collection.Add(new ISO3166Country("Tunisia", "TN", "TUN", 788));
            collection.Add(new ISO3166Country("Turkey", "TR", "TUR", 792));
            collection.Add(new ISO3166Country("Turkmenistan", "TM", "TKM", 795));
            collection.Add(new ISO3166Country("Turks and Caicos Islands", "TC", "TCA", 796));
            collection.Add(new ISO3166Country("Tuvalu", "TV", "TUV", 798));
            collection.Add(new ISO3166Country("Uganda", "UG", "UGA", 800));
            collection.Add(new ISO3166Country("Ukraine", "UA", "UKR", 804));
            collection.Add(new ISO3166Country("United Arab Emirates", "AE", "ARE", 784));
            collection.Add(new ISO3166Country("United Kingdom of Great Britain and Northern Ireland", "GB", "GBR", 826));
            collection.Add(new ISO3166Country("United States of America", "US", "USA", 840));
            collection.Add(new ISO3166Country("United States Minor Outlying Islands", "UM", "UMI", 581));
            collection.Add(new ISO3166Country("Uruguay", "UY", "URY", 858));
            collection.Add(new ISO3166Country("Uzbekistan", "UZ", "UZB", 860));
            collection.Add(new ISO3166Country("Vanuatu", "VU", "VUT", 548));
            collection.Add(new ISO3166Country("Venezuela (Bolivarian Republic of)", "VE", "VEN", 862));
            collection.Add(new ISO3166Country("Viet Nam", "VN", "VNM", 704));
            collection.Add(new ISO3166Country("Virgin Islands (British)", "VG", "VGB", 92));
            collection.Add(new ISO3166Country("Virgin Islands (U.S.)", "VI", "VIR", 850));
            collection.Add(new ISO3166Country("Wallis and Futuna", "WF", "WLF", 876));
            collection.Add(new ISO3166Country("Western Sahara", "EH", "ESH", 732));
            collection.Add(new ISO3166Country("Yemen", "YE", "YEM", 887));
            collection.Add(new ISO3166Country("Zambia", "ZM", "ZMB", 894));
            collection.Add(new ISO3166Country("Zimbabwe", "ZW", "ZWE", 716));

            return collection;
        }

        // Retrieve deck from PlayGwent
        //static public Deck pull_deck(string url)
        //{
        //    Deck d = new Deck();

        //    string html = GetPagePhantomJs(url);

        //    var htmlDoc = new HtmlDocument();
        //    htmlDoc.LoadHtml(html);

        //    // find faction
        //    foreach (var node in htmlDoc.DocumentNode.SelectNodes("//p"))
        //    {
        //        string class_name = node.GetAttributeValue("class", "");
        //        if (class_name.Contains("FactionContent__FactionName"))
        //        {
        //            string faction = node.InnerText.Split(' ')[0];
        //            d.faction = faction;
        //            break;
        //        }
        //    }


        //    // find leader ability name
        //    foreach (var node in htmlDoc.DocumentNode.SelectNodes("//div"))
        //    {
        //        string class_name = node.GetAttributeValue("class", "");
        //        if (class_name.Contains("Card__AbilityName"))
        //        {
        //            string leader = node.InnerText;
        //            d.leader = leader;
        //            break;
        //        }
        //    }

        //    // find all cards in deck
        //    int iteration = 0;
        //    foreach (var node in htmlDoc.DocumentNode.SelectNodes("//li[@class='Card__CardContainer-wm0ofx-3 jUEyAH']"))
        //    {
        //        if (iteration > 0)
        //        {
        //            var name_node = node.ChildNodes[0].ChildNodes[2];
        //            string card_name = name_node.InnerText;
        //            string inner = node.ChildNodes[0].ChildNodes[3].InnerText;
        //            int count = node.ChildNodes[0].ChildNodes[3].InnerText.ToCharArray()[1] - '0';
        //            if (iteration == 1)
        //            {
        //                d.stratagem = card_name;
        //            }
        //            else
        //            {
        //                d.add_card(card_name);
        //            }
        //            /* // disabled to avoid overemphasizing bronzes
        //            if (count == 2)
        //            {
        //                d.add_card(card_name);
        //            }
        //            */
        //        }
        //        iteration += 1;
        //    }

        //    /*
        //    foreach(string name in d.cards)
        //    {
        //        Console.WriteLine(name);
        //    }
        //    */

        //    return d;
        //}

        // Tournament pages use Javascript, so it is harder to obtain the html.  I am using an online service for this, which will cost a little bit if we load a large number of pages.  The free plan is < 500 pages per day.
        // Can replace ak-r9... with your own account if you want.
        public static string GetPagePhantomJs(string url)
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                client.DefaultRequestHeaders.ExpectContinue = false;
                var pageRequestJson = new System.Net.Http.StringContent(@"{'url':'" + url + "','renderType':'html','outputAsJson':false }");
                var response = client.PostAsync("https://PhantomJsCloud.com/api/browser/v2/ak-r9hft-vm1x5-qdfxq-s2wnw-m1702/", pageRequestJson).Result;
                return response.Content.ReadAsStringAsync().Result;
            }
        }
    }
}
