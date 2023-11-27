using System;
using System.Collections.Generic;
using System.Text;

namespace MadGains.Logic
{
    public class Match
    {
        int id;
        int idPlayer;
        string gogOponent;
        int playerDeck;
        int oponentDeck;
        string date;
        string type;
        string win;

        public int Id { get => id; set => id = value; }
        public int IdPlayer { get => idPlayer; set => idPlayer = value; }
        public string GogOponent { get => gogOponent; set => gogOponent = value; }
        public int PlayerDeck { get => playerDeck; set => playerDeck = value; }
        public int OponentDeck { get => oponentDeck; set => oponentDeck = value; }
        public string Date { get => date; set => date = value; }
        public string Type { get => type; set => type = value; }
        public string Win { get => win; set => win = value; }
    }
}
