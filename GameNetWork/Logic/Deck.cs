using System;
using System.Collections.Generic;
using System.Text;

namespace MadGains.Logic
{
    public class Deck
    {
        int id;
        string name;

        string faction;
        string ability;

        string tier;
        string stars;

        string snapshot;
        string url;


        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Faction { get => faction; set => faction = value; }
        public string Ability { get => ability; set => ability = value; }
        public string Tier { get => tier; set => tier = value; }
        public string Stars { get => stars; set => stars = value; }
        public string Snapshot { get => snapshot; set => snapshot = value; }
        public string Url { get => url; set => url = value; }
    }
}
