using System;
using System.Collections.Generic;
using System.Text;

namespace MadGains.Logic
{


    public class Matchup
    {
        int id;
        int idDeckA;
        int idDeckB;
        string description;
        string date;
        int author;

        public int Id { get => id; set => id = value; }
        public int IdDeckA { get => idDeckA; set => idDeckA = value; }
        public int IdDeckB { get => idDeckB; set => idDeckB = value; }
        public string Description { get => description; set => description = value; }
        public string Date { get => date; set => date = value; }
        public int Author { get => author; set => author = value; }
    }
}
