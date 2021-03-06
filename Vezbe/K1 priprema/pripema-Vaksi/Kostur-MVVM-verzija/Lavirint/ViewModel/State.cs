﻿using Lavirint.Model.Kretanje.SahovskeFigure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lavirint
{
    public class State
    {
        public static int[,] lavirint;
        State parent;
        public int markI, markJ; //vrsta i kolona
        public double cost;
        public int level;

        private static Top sahovskaFiguraTop = new Top();
        private static Kralj sahovskaFiguraKralj = new Kralj();
        private static Kraljica sahovskaFiguraKraljica = new Kraljica();
        private static Skakac sahovskaFiguraSkakac = new Skakac();
        private static Lovac sahovskaFiguraLovac = new Lovac();

        private static int[,] kretanjeTopa = sahovskaFiguraTop.getKretanjeFigure();
        private static int[,] kretanjeKralja = sahovskaFiguraKralj.getKretanjeFigure();
        private static int[,] kretanjeKraljice = sahovskaFiguraKraljica.getKretanjeFigure();
        private static int[,] kretanjeSkakaca = sahovskaFiguraSkakac.getKretanjeFigure();
        private static int[,] kretanjeLovca = sahovskaFiguraLovac.getKretanjeFigure();

        private static int[,] koraciRobota = null;
        private static bool jednoPoteznaFigura = false;

        
        


        // TODO: Ovde govorimo sta sledece stanje ima i sta nosi sa sobom
        // voditi da racuna da ono preuzme sve od prethodnog sto treba !
        public State sledeceStanje(int markI, int markJ)
        {
            State rez = new State();
            rez.markI = markI;
            rez.markJ = markJ;
            rez.parent = this;
            rez.cost = this.cost + 1;
            rez.level = this.level + 1;
            // TODO: Ovde recimo mozemo dodati da li je kutija pokupljena
            // Pa ako jeste onda atribut za indikaciju pokupljenosti kutije za ovo stanje stavimo na true

            return rez;
        }

        
        // TODO: Ovde odredjujemo moguca sledeca kretanja
        // Ako se nista posebno ne trazi, ovo je dovoljno.
        public List<State> mogucaSledecaStanja()
        {
            List<State> validnaSledecaStanja = new List<State>();

            podesiKretanjaFigure();

            for(int i = 0; i < koraciRobota.GetLength(0); i++)
            {
                // Broj koraka koji ima odredjena figura u ovom potezu
                int brojKoraka = 1;

                while (true)
                {
                    int novoI = markI + brojKoraka * koraciRobota[i, 0];
                    int novoJ = markJ + brojKoraka * koraciRobota[i, 1];

                    ++brojKoraka;

                    // Odma prekidam istrazivanje ako su kordinate nevalidne
                    if (!validneKordinate(novoI, novoJ))
                        break;

                    // U suprotnosti cu ih dodati kao sledeca validna stanja 
                    State validnoStanje = new State();
                    validnoStanje = sledeceStanje(novoI, novoJ);

                    // Kako bih obisao zapravo samo ona na koja mogu stati
                    // voditi racuna o ovome !
                    if (!(validnoStanje is null))
                        validnaSledecaStanja.Add(validnoStanje);
                    else
                        break;

                    // Restrikcija kretanja na jedan potez samo [za jedno potezne figure]
                    if (jednoPoteznaFigura)
                        break;

                }

            }
            return validnaSledecaStanja;
        }

        // TODO: Ovde odredjujemo koji je hash code
        public override int GetHashCode()
        {
            int hash = 10 * markI + markJ;
            return hash;
        }

        // TODO: Ovde menjamo kada se krajnje stanje uslovljava i zavisi od necega
        public bool isKrajnjeStanje()
        {
            return Main.krajnjeStanje.markI == markI && Main.krajnjeStanje.markJ == markJ;
        }

        /// <summary>
        /// Odredjivanje kretanja figure sa indikatorom jednog poteza
        /// koji predstavlja koliko moze odredjena figura da se krece
        /// po pravilu igre
        /// </summary>
        private void podesiKretanjaFigure()
        {
            // Ovde kad dobijes neko uslovljeno pravilo, podesavas razne uslove
            // i onda kazes koje korake ima robot i da li je jedno potezna figura
            // recimo kretanjeTopa i jedno potezna figura na true nam daje
            // kretanje cik cak, tkd ovde moze biti raznih kombinacija

            koraciRobota = kretanjeKralja;
            jednoPoteznaFigura = true;
        }

        // TODO: Ovde odredjujemo validne kordinate
        private bool validneKordinate(int kordI, int kordJ)
        {
            if (kordI < 0 || kordI >= Main.brojVrsta)
            {
                return false;
            }
            if (kordJ < 0 || kordJ >= Main.brojKolona)
            {
                return false;
            }

            // Branim prolazak kroz sivu [ 1 je reprezent sivog polja na tabli]
            if (lavirint[kordI, kordJ] == 1)
            {
                return false;
            }

            return true;
        }

        public List<State> path()
        {
            List<State> putanja = new List<State>();
            State tt = this;
            while (tt != null)
            {
                putanja.Insert(0, tt);
                tt = tt.parent;
            }
            return putanja;
        }

        
    }
}
