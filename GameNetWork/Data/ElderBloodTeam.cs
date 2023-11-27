using MadGains.Logic;
using System;
using System.Collections.Generic;
using System.Text;

namespace MadGains.Data
{
    static class ElderBloodTeam
    {
        public static string masters = "masters-5";
        public static string season = DateTime.Now.ToString("MMMM")+"-season";

        public static int where_cut_off = 1000;


        static public List<TeamMates> listofEsport()
        {
            List<TeamMates> list = new List<TeamMates>();


            TeamMates z7 = new TeamMates("Killerganon", "killerganon");
            list.Add(z7);


            TeamMates l = new TeamMates("Gregory_Black", "[TEB] Gregory_Black#4499");
            list.Add(l);



            TeamMates z11 = new TeamMates("Idris_98", "idris_98#5579");
            list.Add(z11);




           TeamMates ee = new TeamMates("Olsmer", "[TEB] Olsmer");
            list.Add(ee);


            TeamMates z16 = new TeamMates("t1mMy1337", "[TEB] t1mMy#5117");
            list.Add(z16);




            TeamMates u = new TeamMates("Gandalf0271", "gandalf0271");
            list.Add(u);



            TeamMates z17 = new TeamMates("gerinter", "[TEB] gerinter#2006");
            list.Add(z17);


            TeamMates z3 = new TeamMates("FergieB", "[TEB] FergieB#5812");
            list.Add(z3);


            TeamMates s = new TeamMates("CaptainFlixon", "CaptainFlixon#5656");
            list.Add(s);



            //

            TeamMates w = new TeamMates("MattiHautamaki", "Bart232#1534"); //Truzky
            list.Add(w);

            TeamMates m = new TeamMates("Highrollerko", "HighRollerko#5633");
            list.Add(m);



            TeamMates h = new TeamMates("Kaneki_Yamori", "kaneki_yamori");
            list.Add(h);

            TeamMates h2 = new TeamMates("KozixTheUnique", "Kozix#5511");
            list.Add(h2);

            TeamMates nn3 = new TeamMates("RonaldinhoGwent", "[TEB] kupcevic#4217");
            list.Add(nn3);



            ///
            ///



            TeamMates g = new TeamMates("9Kjer", "9kjer");
            list.Add(g);


            TeamMates z8 = new TeamMates("rogbros", "[TEB] rogbros#1955");
            list.Add(z8);



            TeamMates z10 = new TeamMates("Puzzle.Express", "puzzle_express");
            list.Add(z10);


            TeamMates i = new TeamMates("Zoltan3344", "[TEB] Zoltan3344");
            list.Add(i);



            TeamMates aa2 = new TeamMates("TEB_passi", "tebpassi");
            list.Add(aa2);


            TeamMates oo = new TeamMates("CaueVrihedd", "cauevrihedd");
            list.Add(oo);

            TeamMates oo33 = new TeamMates("rogbros", "[TEB] rogbros#1955");
            list.Add(oo33);

            TeamMates oo344 = new TeamMates("Lemon444", "lem0n#4650");
            list.Add(oo344);

            TeamMates oo3445 = new TeamMates("JaimeOneHand", "jaimeonehand");
            list.Add(oo3445);

            TeamMates oo34456 = new TeamMates("Kareem_Eid", "kareem_eid");
            list.Add(oo34456);
            

            TeamMates enerGiiX = new TeamMates("enerGiiX", "enerGiiX#6933");
            list.Add(enerGiiX);

            TeamMates JSN991 = new TeamMates("JSN991", "jsn991");
            list.Add(JSN991);

            TeamMates Edizsevim = new TeamMates("Edizsevim", "edizsvm");
            list.Add(Edizsevim);


            return list;
        }

        static public List<TeamMates> listofNotEsport()
        {
            List<TeamMates> list = new List<TeamMates>();






            TeamMates zz8 = new TeamMates("Alkso", "[TEB] Electric000");
            list.Add(zz8);


            TeamMates zz10 = new TeamMates("ArtNhr", "[TEB] ArtNhr");
            list.Add(zz10);

            TeamMates zz11 = new TeamMates("--oXo--", "[TEB] oXo");
            list.Add(zz11);

            TeamMates zz12 = new TeamMates("Rykov_", "[TEB] Rykov_");
            list.Add(zz12);


            TeamMates zz13 = new TeamMates("INAF_Official", "[TEB] INAF_Official#3996");
            list.Add(zz13);


            //nie chce być na liście, a potem mówi że żart
            TeamMates aa3 = new TeamMates("PiotrCNS", "PiotrCNS#2862");
            list.Add(aa3);



            TeamMates a2 = new TeamMates("ili_DIANN", "[TEB] ili_DIANN");
            list.Add(a2);

            TeamMates b2 = new TeamMates("Drobniak12", "AlanitoBandito#9832");
            list.Add(b2);

            TeamMates j3 = new TeamMates("HerrDawix", "[TEB] HerrDawix");
            list.Add(j3);

            TeamMates j4 = new TeamMates("moshcraft", "[TEB] moshcraft");
            list.Add(j4);

            TeamMates j5 = new TeamMates("Sawyer1888", "Sawyer1888");
            list.Add(j5);



            return list;
        }
    }
}
