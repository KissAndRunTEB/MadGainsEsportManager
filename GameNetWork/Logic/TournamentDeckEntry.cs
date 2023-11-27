using System;
using System.Collections.Generic;
using System.Text;

namespace MadGains.Logic
{


    class TournamentDeckEntry
    {
    string player;

        string country;

        string deckA;
        string factionA;

        string deckB;
        string factionB;

        string deckC;
        string factionC;

        string deckD;
        string factionD;

        public string Player { get => player; set => player = value; }
        public string DeckA { get => deckA; set => deckA = value; }
        public string FactionA { get => factionA; set => factionA = value; }
        public string DeckB { get => deckB; set => deckB = value; }
        public string FactionB { get => factionB; set => factionB = value; }
        public string DeckC { get => deckC; set => deckC = value; }
        public string FactionC { get => factionC; set => factionC = value; }
        public string DeckD { get => deckD; set => deckD = value; }
        public string FactionD { get => factionD; set => factionD = value; }
        public string Country { get => country; set => country = value; }
    }
}
