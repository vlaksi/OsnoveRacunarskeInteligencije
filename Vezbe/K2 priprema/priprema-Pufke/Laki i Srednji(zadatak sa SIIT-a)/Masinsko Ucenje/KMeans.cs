﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Masinsko_Ucenje
{
    public class KMeans
    {
        public List<Point> elementi = new List<Point>();
        public List<Cluster> grupe = new List<Cluster>();
        public int brojGrupa = 0;
        Random rnd = new Random();

        public void podeliUGrupe(int brojGrupa, double errT)
        {
            this.brojGrupa = brojGrupa;
            if (brojGrupa == 0) return;
            //------------  inicijalizacija -------------
            for (int i = 0; i < brojGrupa; i++)
            {
                // TODO 5: na slucajan nacin inicijalizovati centre grupa
                int ii = rnd.Next(elementi.Count);
                Cluster grupa = new Cluster();
                grupa.centar = elementi[ii];
                grupe.Add(grupa);
            }
            //------------- iterativno racunanje centara ---
            for (int it = 0; it < 1000; it++)
            {
                foreach (Cluster grupa in grupe)
                    grupa.elementi = new List<Point>();

                foreach (Point cc in elementi)
                {
                    int najblizaGrupa = 0;
                    for (int i = 0; i < brojGrupa; i++)
                    {
                        if (grupe[najblizaGrupa].rastojanje(cc) >
                            grupe[i].rastojanje(cc))
                        {
                            najblizaGrupa = i;
                        }
                    }
                    grupe[najblizaGrupa].elementi.Add(cc);
                }
                double err = 0;
                for (int i = 0; i < brojGrupa; i++)
                {
                    err += grupe[i].pomeriCentar();
                }
                if (err < errT)
                    break;

                Main.clusteringHistory.Add(Main.DeepClone(this.grupe));
            }
        }
    }

    [Serializable]
    public class Cluster
    {
        public Point centar = new Point(0,0,0,0);
        public List<Point> elementi = new List<Point>();

        public double rastojanje(Point c)
        {   // TODO 6: implementirati funkciju rastojanja
            return Math.Abs(c.x - centar.x) + Math.Abs(c.y - centar.y) + Math.Abs(c.z - centar.z) + Math.Abs(c.w - centar.w); ;
        }

        public double pomeriCentar()
        {   // TODO 7: implemenitrati funkciju koja pomera centre klastera
            double sX = 0;
            double sY = 0;
            double sZ = 0;
            double sW = 0;
            double retVal = 0;

            foreach (Point c in elementi)
            {
                sX += c.x;
                sY += c.y;
                sZ += c.z;
                sW += c.w;
            }
            int n = elementi.Count();
            if (n != 0)
            {
                Point nCentar = new Point((int)(sX / n), (int)(sY / n), (int)(sZ / n), (int)(sW / n));
                retVal = rastojanje(nCentar);
                centar = nCentar;
            }

            return retVal;
        }
    }
}
