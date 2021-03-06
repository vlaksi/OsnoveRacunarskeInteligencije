﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.Globalization;

namespace Masinsko_Ucenje
{
    public partial class Main : Form
    {

        LinearRegression regression;
        KMeans kmeans;
        string[] lines;


        public Main()
        {
            InitializeComponent();

            lines = File.ReadAllLines(@"./../../train.csv");
            lines = lines.Skip(1).ToArray(); // skip header row (State, Lat, Mort, Ocean, Long)
        }

        private void btnLinearRegression_Click(object sender, EventArgs e)
        {
            regression = new LinearRegression();
            List<double> x = new List<double>();
            List<double> y = new List<double>();

            // TODO 1: Ucitati i isparsirati skup podataka iz lines u x i y
            foreach (string line in lines)
            {
                string[] elements = line.Split(','); // csv file split by , (comma)
                //Console.WriteLine(elements[4]);
                x.Add(double.Parse(elements[3], CultureInfo.InvariantCulture)); // take Lat attribute as x value
                y.Add(double.Parse(elements[4], CultureInfo.InvariantCulture)); // take Mort attribute as y value
            }
            // TODO 4.1: Izvršiti linearnu regresiju na primeru predviđanja stope 
            // smrtnosti od raka kože na osnovu geografske širine američkih država.
            regression.fit(x.ToArray(), y.ToArray());

            // TODO 4.2: Izvršiti predikciju stope mortaliteta za vrednost geografske širine od tačno 37
            double regressionResult = regression.predict(7.1);
            Console.WriteLine("Prediktovana vrednost col_4 za vredonst col_3 od 7.1 je " + regressionResult + "\n\n");
            regressionResult = regression.predict(7.4);
            Console.WriteLine("Prediktovana vrednost col_4 za vredonst col_3 od 7.4 je " + regressionResult + "\n\n");
            regressionResult = regression.predict(8.5);
            Console.WriteLine("Prediktovana vrednost col_4 za vredonst col_3 od 8.5 je " + regressionResult + "\n\n");
            // draw regresiion line on a chart
            drawRegressionResults(x, y);
        }

        private void btnKmeans_Click(object sender, EventArgs e)
        {
            this.kmeans = new KMeans();
            clusteringHistory = new List<List<Cluster>>();

            List<Point> kmeansElements = new List<Point>();
            int k = Convert.ToInt32(tbK.Value);
            double toleracijaNaGresku = Convert.ToDouble(tbErr.Text);

            List<double> col_1 = new List<double>();
            List<double> col_2 = new List<double>();
            List<double> col_3 = new List<double>();
            List<double> col_4 = new List<double>();

            // TODO 8: Klasterizovati američke države na osnovu geografkse dužine i širine
            foreach (string line in lines)
            {
                string[] elements = line.Split(','); // csv file split by , (comma)
                col_1.Add(double.Parse(elements[1], CultureInfo.InvariantCulture));
                col_2.Add(double.Parse(elements[2], CultureInfo.InvariantCulture));
                col_3.Add(double.Parse(elements[3], CultureInfo.InvariantCulture));
                col_4.Add(double.Parse(elements[4], CultureInfo.InvariantCulture));

            }
            List<double> col_1Normalizovani = normalizacija(col_1);
            List<double> col_2Normalizovani = normalizacija(col_2);
            List<double> col_3Normalizovani = normalizacija(col_3);
            List<double> col_4Normalizovani = normalizacija(col_4);

            for (int i = 0; i < lines.Length; i++)
            {
                Point kmeansElement = new Point(col_1Normalizovani[i], col_2Normalizovani[i], col_3Normalizovani[i], col_4Normalizovani[i]);
                kmeansElements.Add(kmeansElement);
            }

            this.kmeans.elementi = kmeansElements;
            this.kmeans.podeliUGrupe(k, toleracijaNaGresku);

            // draw clustering results on a chart
            drawClusteringResults();

            List<double> normalizacija(List<double> listaVrednosti)
            {
                List<double> noramlizovanaLista = new List<double>();

                double dataMax = listaVrednosti.Max();
                double dataMin = listaVrednosti.Min();

                foreach (double vrednost in listaVrednosti)
                {
                    noramlizovanaLista.Add((vrednost - dataMin) / (dataMax - dataMin));
                    // Console.WriteLine("Minimalna vrendost je" + (vrednost - dataMin) / (dataMax - dataMin));
                }
                return noramlizovanaLista;
            }
        }

        private void btnDBScan_Click(object sender, EventArgs e)
        {

        }

        #region GUI_Functions
        private void drawRegressionResults(List<double> X, List<double> Y)
        {
            RegressionChart.Visible = true;
            ClusteringChart.Visible = false;
            RegressionChart.Series.Clear();

            Series diagramLimitsSeries = new Series("DiagramLimits");
            diagramLimitsSeries.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            diagramLimitsSeries.Points.AddXY(X.Min(), Y.Min());
            diagramLimitsSeries.Points.AddXY(X.Max(), Y.Max());
            diagramLimitsSeries.Points[0].IsEmpty = true;
            diagramLimitsSeries.Points[1].IsEmpty = true;
            diagramLimitsSeries.IsVisibleInLegend = false;
            RegressionChart.Series.Add(diagramLimitsSeries);
            RegressionChart.Update();


            // Create a point series.
            Series pointSeries = new Series("Tacke");
            pointSeries.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            RegressionChart.Series.Add(pointSeries);

            for (int i = 0; i < X.Count; i++)
            {
                pointSeries.Points.AddXY(X[i], Y[i]);
                //Thread.Sleep(5);
                RegressionChart.Update();
            }

            // Create a line series.
            string lineLabel = "";
            if (regression.n > 0)
                lineLabel = "Regresiona prava: y=" + Math.Round(regression.k,2) + "*x + " + Math.Round(regression.n,2);
            else
                lineLabel = "Regresiona prava: y=" + Math.Round(regression.k,2) + "*x - " + Math.Round(Math.Abs(regression.n),2);
            Series lineSeries = new Series(lineLabel);
            lineSeries.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            lineSeries.Points.AddXY(X.Max(), regression.n + X.Max() * regression.k);
            lineSeries.Points.AddXY(X.Min(), regression.n + X.Min() * regression.k);
            RegressionChart.Series.Add(lineSeries);
            RegressionChart.Update();
        }

        public static List<List<Cluster>> clusteringHistory = new List<List<Cluster>>();

        private void drawClusteringResults()
        {
            ClusteringChart.Visible = true;
            RegressionChart.Visible = false;

            foreach (List<Cluster> clusterList in clusteringHistory)
            {
                ClusteringChart.Series.Clear();

                // visualize each cluster
                for (int i = 0; i < kmeans.brojGrupa; i++ )
                {
                    // Create a point series.
                    Series pointSeries = new Series("Klaster " + i+1);
                    pointSeries.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;

                    // add cluster center and mark it differently
                    pointSeries.Points.AddXY(clusterList[i].centar.x, clusterList[i].centar.y);
                    pointSeries.Points[0].MarkerSize = pointSeries.Points[0].MarkerSize * 2;
                    pointSeries.Points[0].MarkerBorderColor = Color.Black;
                    pointSeries.Points[0].MarkerBorderWidth = 3;

                    for (int j = 0; j < clusterList[i].elementi.Count; j++)
                    {
                        pointSeries.Points.AddXY(clusterList[i].elementi[j].x, clusterList[i].elementi[j].y);
                    }
                    

                    ClusteringChart.Series.Add(pointSeries);
                }

                ClusteringChart.Update();
                Thread.Sleep(500);
                
            }
        }

        public static T DeepClone<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }
        #endregion GUI_Functions

    }
}
