﻿using System;
using System.Collections.Generic;
using System.Drawing;
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

        // TODO: Ovde odredjujem/dodajem atribute za moguce korake, atribute da li su kutije pokupljene i slicno.
        public bool pokupljenaPlavaKutija;
        public bool pokupljenjaNarandzastaKutija;
        private static int [,] koraci = { { 0, 1 }, { 0, -1 }, { -1, 0 }, { 1, 0 } };

        // TODO: Ovde govorimo sta sledece stanje ima i sta nosi sa sobom
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
            rez.pokupljenaPlavaKutija = this.pokupljenaPlavaKutija;
            rez.pokupljenjaNarandzastaKutija = this.pokupljenjaNarandzastaKutija;
            if(lavirint[rez.markI,rez.markJ] == 4)
            {
                rez.pokupljenaPlavaKutija = true;
            }
            if (lavirint[rez.markI, rez.markJ] == 5 && rez.pokupljenaPlavaKutija)
            {
                rez.pokupljenjaNarandzastaKutija = true;
            }

            return rez;
        }

        // TODO: Ovde odredjujemo validne kordinate
        private bool validneKordinate(int kordI, int kordJ)
        {
            if(kordI<0 || kordI >= Main.brojVrsta)
            {
                return false;
            }
            if(kordJ<0 || kordJ >= Main.brojKolona)
            {
                return false;
            }
            /*
             * Posto smo zavrsili sa proverom za izlazak van opsega table,
             * ogranicavamo da nije moguce prolaziti kroz sivu[vrednost polja 1]
             * kutiju.
             * 
             */
            if(lavirint[kordI,kordJ] == 1)
            {
                return false;
            }

            return true;
        }
        
        // TODO: Ovde odredjujemo moguca sledeca kretanja
        // Ako se nista posebno ne trazi, ovo je dovoljno.
        public List<State> mogucaSledecaStanja()
        {
            List<State> validnaSledecaStanja = new List<State>();

            for(int i = 0; i < koraci.GetLength(0); i++)
            {
                int novoI = markI + koraci[i, 0];
                int novoJ = markJ + koraci[i, 1];

                if (validneKordinate(novoI, novoJ))
                {
                    validnaSledecaStanja.Add(sledeceStanje(novoI, novoJ));
                    dodajValidnaSledecaStanjaZaTeleport(validnaSledecaStanja);
                }

            }
            return validnaSledecaStanja;
        }

        // TODO: Ovde odredjujemo koji je hash code
        public override int GetHashCode()
        {
            int hash = 10 * markI + markJ;
            if (pokupljenaPlavaKutija)
            {
                hash = 100 + 10 * markI + markJ;
            }
            if (pokupljenjaNarandzastaKutija)
            {
                hash = 200 + 10 * markI + markJ;
            }
            return hash;
        }

        // TODO: Ovde menjamo kada se krajnje stanje uslovljava i zavisi od necega
        public bool isKrajnjeStanje()
        {
            //return Main.krajnjeStanje.markI == markI && Main.krajnjeStanje.markJ == markJ && kutijaPokupljena;
            return Main.krajnjeStanje.markI == markI && Main.krajnjeStanje.markJ == markJ && pokupljenaPlavaKutija && pokupljenjaNarandzastaKutija;
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

        private void dodajValidnaSledecaStanjaZaTeleport(List<State> validnaSledecaStanja)
        {
            if (lavirint[markI, markJ] == 6)
            {   // u pitanju je teleport pa su ostali teleporti validna sledeca stanja
                validnaSledecaStanja.AddRange(dobaviValidnaSledecaStanjaZaTeleport());
            }
        }

        private List<State> dobaviValidnaSledecaStanjaZaTeleport()
        {
            List<State> validnaSledecaStanjaZaTeleport = new List<State>();
            foreach (Point portal in Main.portali)
            {
                if (markI == portal.X && markJ == portal.Y)
                {
                    continue;
                }

                validnaSledecaStanjaZaTeleport.Add(sledeceStanje(portal.X, portal.Y));

            }

            return validnaSledecaStanjaZaTeleport;
        }
    }
}
