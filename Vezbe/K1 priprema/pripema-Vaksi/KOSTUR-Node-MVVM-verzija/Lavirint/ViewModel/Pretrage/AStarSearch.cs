﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Net;
using System.Collections.Specialized;

namespace Lavirint
{
    class AStarSearch
    {
        public State search(State pocetnoStanje)
        {
            // TODO 5.1: Implementirati algoritam vodjene pretrage A*
            List<State> stanjaNaObradi = new List<State>();
            Hashtable predjeniPut = new Hashtable();

            stanjaNaObradi.Add(pocetnoStanje);
            while (stanjaNaObradi.Count > 0)
            {
                State naObradi = getBest(stanjaNaObradi);

                if (!predjeniPut.Contains(naObradi.trenutniCvor.GetHashCode()))
                {
                    predjeniPut.Add(naObradi.trenutniCvor.GetHashCode(), null);
                    Main.allSearchStates.Add(naObradi);

                    if (naObradi.isKrajnjeStanje())
                    {
                        return naObradi;
                    }
                    List<State> mogucaSledecaStanja = naObradi.getMogucaSledecaStanja();
                    stanjaNaObradi.AddRange(mogucaSledecaStanja);
                }

                stanjaNaObradi.Remove(naObradi);
            }

            return null;
        }

        public double heuristicFunction(State s)
        {
            return Math.Sqrt(Math.Pow(s.trenutniCvor.kordinataI - Main.krajnjiNode.kordinataI, 2)
                + Math.Pow(s.trenutniCvor.kordinataJ - Main.krajnjiNode.kordinataJ, 2));
        }

        public State getBest(List<State> svaStanjaZaObradu)
        {
            State rez = null;
            double min = Double.MaxValue;

            foreach (State stanjeNaObradi in svaStanjaZaObradu)
            {
                double h = heuristicFunction(stanjeNaObradi)+  stanjeNaObradi.cena;
                if (h < min)
                {
                    min = h;
                    rez = stanjeNaObradi;
                }
            }
            return rez;
        }
    }
}
