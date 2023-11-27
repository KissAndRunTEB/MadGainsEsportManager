using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MadGains.Logic
{
    [XmlRoot(ElementName = "ListOfPlayers")]
    public class ListOfPlayers
    {
        public ListOfPlayers()
        {
            Players = new List<Player>();
        }

        [XmlElement(ElementName = "Employee")]
        public List<Player> Players { get; set; }

        public Player this[string nick]
        {
            get { return Players.FirstOrDefault(s => string.Equals(s.Nick, nick, StringComparison.OrdinalIgnoreCase)); }
        }
    }


    public class Player
    {
        string nick;
        int ladderPosition;

        List<int> ladderPositionThrewSeasons;

        int mmr;

        int wr;
        int lei;

        int matches;
        int wins;
        int losses;
        int draws;

        int moMatches;
        int moWins;
        int nrMatches;
        int nrWins;
        int syMatches;
        int syWins;
        int skMatches;
        int skWins;
        int stMatches;
        int stWins;
        int ngMatches;
        int ngWins;

        int moWR;
        int nrWR;
        int syWR;
        int skWR;
        int stWR;
        int ngWR;


        [XmlAttribute("nick")]
        public string Nick { get => nick; set => nick = value; }

        [XmlAttribute("ladderposition")]
        public int LadderPosition { get => ladderPosition; set => ladderPosition = value; }

        [XmlAttribute("mmr")]
        public int Mmr { get => mmr; set => mmr = value; }

        [XmlAttribute("matches")]
        public int Matches { get => matches; set => matches = value; }

        [XmlAttribute("wins")]
        public int Wins { get => wins; set => wins = value; }

        [XmlAttribute("losses")]
        public int Losses { get => losses; set => losses = value; }

        [XmlAttribute("draws")]
        public int Draws { get => draws; set => draws = value; }

        [XmlAttribute("MOMatches")]
        public int MOMatches { get => moMatches; set => moMatches = value; }

        [XmlAttribute("MOWins")]
        public int MOWins { get => moWins; set => moWins = value; }

        [XmlAttribute("NRMatches")]
        public int NRMatches { get => nrMatches; set => nrMatches = value; }

        [XmlAttribute("NRWins")]
        public int NRWins { get => nrWins; set => nrWins = value; }

        [XmlAttribute("SYMatches")]
        public int SYMatches { get => syMatches; set => syMatches = value; }

        [XmlAttribute("SYWins")]
        public int SYWins { get => syWins; set => syWins = value; }

        [XmlAttribute("SKMatches")]
        public int SKMatches { get => skMatches; set => skMatches = value; }

        [XmlAttribute("SKWins")]
        public int SKWins { get => skWins; set => skWins = value; }

        [XmlAttribute("STMatches")]
        public int STMatches { get => stMatches; set => stMatches = value; }

        [XmlAttribute("STWins")]
        public int STWins { get => stWins; set => stWins = value; }

        [XmlAttribute("NGMatches")]
        public int NGMatches { get => ngMatches; set => ngMatches = value; }

        [XmlAttribute("NGWins")]
        public int NGWins { get => ngWins; set => ngWins = value; }
        public List<int> LadderPositionThrewSeasons { get => ladderPositionThrewSeasons; set => ladderPositionThrewSeasons = value; }
        public int Lei { get => lei; set => lei = value; }
        public int Wr { get => wr; set => wr = value; }
        public int NgWR { get => ngWR; set => ngWR = value; }
        public int StWR { get => stWR; set => stWR = value; }
        public int SkWR { get => skWR; set => skWR = value; }
        public int SyWR { get => syWR; set => syWR = value; }
        public int NrWR { get => nrWR; set => nrWR = value; }
        public int MoWR { get => moWR; set => moWR = value; }

        public Player(string nick)
        {
            this.nick = nick;
            this.ladderPosition = 0;

            this.ladderPositionThrewSeasons = new List<int>();

            this.mmr = 0;
            this.Wr = 0;
            this.Lei = 0;

            this.matches = 0;
            this.wins = 0;
            this.losses = 0;
            this.MOMatches = 0;
            this.MOWins = 0;
            this.NRMatches = 0;
            this.NRWins = 0;
            this.SYMatches = 0;
            this.SYWins = 0;
            this.SKMatches = 0;
            this.SKWins = 0;
            this.STMatches = 0;
            this.STWins = 0;
            this.NGMatches = 0;
            this.NGWins = 0;
        }
    }
}
