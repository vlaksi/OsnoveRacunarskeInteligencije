﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Lavirint
{
    public class State
    {
        public static int[,] lavirint; 
        State parent;
        public int markI, markJ; 
        public double cost;
        public int skupljenBrojPoena;
        private List<int> listaSakupljenihKutija = new List<int>();
      
        public State sledeceStanje(int markI, int markJ)
        {
            State rez = new State();
            rez.markI = markI;
            rez.markJ = markJ;
            rez.parent = this;
            rez.cost = this.cost + 1;
            rez.skupljenBrojPoena = this.skupljenBrojPoena;
            rez.listaSakupljenihKutija.AddRange(this.listaSakupljenihKutija);
            return rez;
        }


        public List<State> mogucaSledecaStanja()
        {

            List<State> rez = new List<State>();

            //Logika za kutije
            if (lavirint[markI, markJ] == 4 && !this.listaSakupljenihKutija.Contains(100 * markI + markJ))
            {
                this.skupljenBrojPoena += 3;
                this.listaSakupljenihKutija.Add(100 * markI + markJ);
            }

            if (lavirint[markI, markJ] == 5 && !this.listaSakupljenihKutija.Contains(100 * markI + markJ))
            {
                this.skupljenBrojPoena += 1;
                this.listaSakupljenihKutija.Add(100 * markI + markJ);
            }

            if (skupljenBrojPoena < 10) {
                #region Kretnja kao skakac ( sahovska figura konj)

                for (int i = -2; i <= 2; i += 4)
                {                                       //Ove dve for petlje nam daju kombinaciju kretanja levo desno za dva polja i gore dole za jedno polje
                    int newMarkI = markI + i;
                    for (int j = -1; j <= 1; j += 2)
                    {
                        int newMarkJ = markJ + j;
                        if (newMarkI >= 0 && newMarkI < Main.brojVrsta)
                        {
                            if (newMarkJ >= 0 && newMarkJ < Main.brojKolona)
                            {
                                if (lavirint[newMarkI, newMarkJ] != 1)
                                {
                                    State novo = sledeceStanje(newMarkI, newMarkJ);
                                    rez.Add(novo);
                                }
                            }
                        }
                    }
                }

                for (int j = -2; j <= 2; j += 4)
                {                                        //Ove dve for petlje nam daju kombinaciju kretanja gore dole za dva polja i levo desno za jedno polje
                    int newMarkJ = markJ + j;
                    for (int i = -1; i <= 1; i += 2)
                    {
                        int newMarkI = markI + i;
                        if (newMarkI >= 0 && newMarkI < Main.brojVrsta)
                        {
                            if (newMarkJ >= 0 && newMarkJ < Main.brojKolona)
                            {
                                if (lavirint[newMarkI, newMarkJ] != 1)
                                {
                                    State novo = sledeceStanje(newMarkI, newMarkJ);
                                    rez.Add(novo);
                                }
                            }
                        }
                    }
                }
                #endregion
            }
            if (skupljenBrojPoena >= 10)
            {
                #region Kretnja kao top
                //Kretanje u pravcu dole
                for (int i = 1; i <= Main.brojVrsta; i += 1)
                {
                    int newMarkI = markI + i;


                    if (newMarkI >= 0 && newMarkI < Main.brojVrsta)
                    {
                        if (markJ >= 0 && markJ < Main.brojKolona)
                        {
                            if (lavirint[newMarkI, markJ] == 1)
                            {
                                break;
                            }
                            if (lavirint[newMarkI, markJ] != 1)
                            {
                                State novo = sledeceStanje(newMarkI, markJ);
                                rez.Add(novo);
                            }
                        }
                    }
                }

                for (int j = 1; j <= Main.brojVrsta; j += 1)
                {
                    int newMarkJ = markJ - j;

                    if (markI >= 0 && markI < Main.brojVrsta)
                    {
                        if (newMarkJ >= 0 && newMarkJ < Main.brojKolona)
                        {
                            if (lavirint[markI, newMarkJ] == 1)
                            {
                                break;
                            }
                            if (lavirint[markI, newMarkJ] != 1)
                            {
                                State novo = sledeceStanje(markI, newMarkJ);
                                rez.Add(novo);
                            }
                        }
                    }
                }

                for (int i = 1; i <= Main.brojVrsta; i += 1)
                {
                    int newMarkI = markI - i;

                    if (newMarkI >= 0 && newMarkI < Main.brojVrsta)
                    {
                        if (markJ >= 0 && markJ < Main.brojKolona)
                        {
                            if (lavirint[newMarkI, markJ] == 1)
                            {
                                break;
                            }
                            if (lavirint[newMarkI, markJ] != 1)
                            {
                                State novo = sledeceStanje(newMarkI, markJ);
                                rez.Add(novo);
                            }
                        }
                    }
                }

                for (int j = 1; j <= Main.brojVrsta; j += 1)
                {

                    int newMarkJ = markJ + j;

                    if (markI >= 0 && markI < Main.brojVrsta)
                    {
                        if (newMarkJ >= 0 && newMarkJ < Main.brojKolona)
                        {
                            if (lavirint[markI, newMarkJ] == 1)
                            {
                                break;
                            }
                            if (lavirint[markI, newMarkJ] != 1)
                            {
                                State novo = sledeceStanje(markI, newMarkJ);
                                rez.Add(novo);
                            }
                        }
                    }
                }
                #endregion
            }

            return rez;
        }

        public override int GetHashCode()
        {
            int code = 10 * markI + markJ;
         
            foreach (int codeSakupljenogPolja in this.listaSakupljenihKutija)
            {
                code += codeSakupljenogPolja;
            }
            return code;
        }

        public bool isKrajnjeStanje()
        { 
            return Main.krajnjeStanje.markI == markI && Main.krajnjeStanje.markJ == markJ && this.skupljenBrojPoena >=10;
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
