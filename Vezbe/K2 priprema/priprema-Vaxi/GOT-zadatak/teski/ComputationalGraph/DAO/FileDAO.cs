﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using ComputationalGraph.Utilities;

namespace ComputationalGraph.DAO
{
    /// <summary>
    /// Pristup fajlovima, i ucitavanja iz njih.
    /// Pattern: Data Access Object
    /// </summary>
    public class FileDAO
    {
        #region Atributi

        
        /// <summary>
        /// Ideja je da predstavlja kolekciju za 'one hot'
        /// 
        /// key: uneti kategoricki atribut [ TEXT ]
        /// value: isTajKey
        /// 
        /// Primer  {
        ///             "srbija": [ 1 ]
        ///             "grcka" : [ 0, 1] 
        ///         }
        /// 
        /// Koliko imam elemenata u dict, na to mesto
        /// stavljam 1 kod novog clana u dict
        ///         {
        ///             "gruzija" : [ 0, 0, 1]
        ///         }
        ///         
        /// Na kraju bi trebalo proci kroz citav dict i svakome
        /// value koji nema elemenata koliko i dict dodati dopunu
        /// do tog broja elemenata(naravno nulama)
        /// 
        /// </summary>
        private Dictionary<string,bool> _kategorickiAtributi;

        /// <summary>
        /// Normalizuje prosledjene neuredjene podatke
        /// u lepo domenovane podatke[0-1]
        /// </summary>
        private NormalizatorPodataka _normalizacija;

        /// <summary>
        /// Predstavlja listu kolona koje smo ucitali
        /// 
        /// Kolone koje predstavljaju labele ulaznih atributa !
        /// </summary>
        private List<List<double>> _ulazneKolone;

        /// <summary>
        /// Predstavlja listu ucitanih kolona izlaznog atributa,
        /// ne Y nego samo listu kolona izlaznih atributa.
        /// </summary>
        private List<List<double>> _izlazneKolone;

        /// <summary>
        /// Iscitavanje podataka iz csv fajla zadatom u citacu.
        /// </summary>
        private DataReader _citacPodataka;

        /// <summary>
        /// Svi INPUTI koji se dovode na ulaz mreze
        /// </summary>
        private List<List<double>> _X;

        /// <summary>
        /// Svi OUTPUT-i koje se nalaze na izlazu mreze
        /// </summary>
        private List<List<double>> _Y;

        /// <summary>
        /// Inputi koje cuvamo samo za testiranje podataka
        /// kako ne bi ipak overfitovali budemo li na svim podacima
        /// fitovali tj trenirali !
        /// </summary>
        private List<List<double>> _Xtest;

        /// <summary>
        /// Outputi koje cuvamo samo za testiranje modela.
        /// Isto kao i XTest
        /// </summary>
        private List<List<double>> _Ytrain;


        #endregion

        #region Propertiji

        public Dictionary<string, bool> KategorickiAtributi
        {
            get { return _kategorickiAtributi; }
            set { _kategorickiAtributi = value; }
        }

        public NormalizatorPodataka Normalizacija
        {
            get { return _normalizacija; }
            set { _normalizacija = value; }
        }

        public List<List<double>> UlazneKolone
        {
            get { return _ulazneKolone; }
            set { _ulazneKolone = value; }
        }

        public List<List<double>> IzlazneKolone
        {
            get { return _izlazneKolone; }
            set { _izlazneKolone = value; }
        }

        public DataReader CitacPodataka
        {
            get { return _citacPodataka; }
            set { _citacPodataka = value; }
        }

        public List<List<double>> X
        {
            get { return _X; }
            set { _X = value; }
        }

        public List<List<double>> Y
        {
            get { return _Y; }
            set { _Y = value; }
        }

        public List<List<double>> YTest
        {
            get { return _Ytrain; }
            set { _Ytrain = value; }
        }

        public List<List<double>> XTest
        {
            get { return _Xtest; }
            set { _Xtest = value; }
        }
        
        #endregion

        /// <summary>
        /// Proslediti indekse kolona za ucitavanje kako bi setovali X inpute,
        /// a proslediti indekse kolona koje predstavljaju izlaze kako bi setovali Y odnosno
        /// OUTPUT-e mreze.
        /// </summary>
        /// <param name="indeksiKolonaZaUcitavanjeInputa"> indeksi kolona koje zelimo ucitati iz data seta ( a predstavljaju ulazne atribute u mrezu ) </param>
        /// <param name="indeksiKolonaZaUcitavanjeOutputa"> indeksi kolona koje zelimo ucitati iz data seta (a predstavljaju izlazne atribute iz mreze) </param>
        /// <param name="procenatTestPodataka"> Procenat koliko zelimo da imamo testnih podataka od svih ucitanih </param>"
        public FileDAO(List<int> indeksiKolonaZaUcitavanjeInputa, List<int> indeksiKolonaZaUcitavanjeOutputa, int procenatTestPodataka)
        {

            UlazneKolone = new List<List<double>>();
            IzlazneKolone = new List<List<double>>();

            Normalizacija = new NormalizatorPodataka();
            CitacPodataka = new DataReader();

            X = new List<List<double>>();
            Y = new List<List<double>>();

            XTest = new List<List<double>>();
            YTest = new List<List<double>>();

            ucitajUlazneKolone(indeksiKolonaZaUcitavanjeInputa);
            odrediInpute(procenatTestPodataka);

            ucitajIzlazneKolone(indeksiKolonaZaUcitavanjeOutputa);
            odrediOutpute(procenatTestPodataka);

        }

        
        #region Ucitavanje svih kolona

        /// <summary>
        /// Ucitavanje svih trazenih ulaznih kolona po prosledjenim indeksima
        /// tih kolona u tabeli.
        /// </summary>
        /// <param name="brojKolonaZaUcitavanjeInputa"></param>
        private void ucitajUlazneKolone(List<int> brojKolonaZaUcitavanjeInputa)
        {
            foreach (int kolona in brojKolonaZaUcitavanjeInputa)
            {
                List<double> podaciKolone = CitacPodataka.ucitajPodatkeIzKolone(kolona);
                podaciKolone = Normalizacija.normalizujPodatke(podaciKolone);

                if (CitacPodataka.UniqKategorickiAtributi.Count > 0)
                {
                    List<List<double>> noveKolone = CitacPodataka.UniqKategorickiAtributi.Values.ToList();

                    /*
                     * Zbog https://towardsdatascience.com/one-hot-encoding-multicollinearity-and-the-dummy-variable-trap-b5840be3c41a
                     * izbacujem jednu kolonu kako ne bi upao u zamku koja se krije u one hot encodingu
                     */
                    noveKolone.RemoveAt(noveKolone.Count - 1);

                    UlazneKolone.AddRange(noveKolone);
                }
                else
                {
                    UlazneKolone.Add(podaciKolone);
                }

                
            }
        }


        /// <summary>
        /// Ucitavanje svih trazenih izlaznih kolona ( atributa)
        /// sa prosledjenim indeksima tih kolona u tabeli
        /// </summary>
        /// <param name="brojKolonaZaUcitanjeOutputa"></param>
        private void ucitajIzlazneKolone(List<int> brojKolonaZaUcitanjeOutputa)
        {
            foreach (int kolona in brojKolonaZaUcitanjeOutputa)
            {
                List<double> podaciKolone = CitacPodataka.ucitajPodatkeIzKolone(kolona);
                podaciKolone = Normalizacija.normalizujPodatke(podaciKolone);

                IzlazneKolone.Add(podaciKolone);
            }
        }
        #endregion

        #region Odredjivanje inputa na osnovu ucitanih ulaznih kolona

        /// <summary>
        /// Inicijalizacija X inputa u neuronskoj mrezi.
        /// 
        /// Voditi racuna da bude tek nakon ucitavanja kolona ulaza.
        /// 
        /// Ideja: 
        /// Prodjem kroz svaki red ucitanih ulaznih kolona
        /// i dodam taj red (sample of row, tj tu jednu kombinaciju ulaza)
        /// u listu svih ulaza X (red je takodje lista pa je X zato lista listi)
        /// </summary>
        /// <param name="procenatTestPodataka"> Procenat koliko zelimo da bude test podataka iz data seta </param>
        private void odrediInpute(int procenatTestPodataka)
        {
            int ukupnoEntitetaSistema = CitacPodataka.UcitaniRedovi.Length;
            int indeksPocetkaTestPodataka = ukupnoEntitetaSistema * procenatTestPodataka / 100;

            // CitacPodataka.UcitaniRedovi.Length; jer za svaki input imam jednu kolonu, te to predstavlja dimenziju X [inputa]
            for (int indeksReda = 0; indeksReda < ukupnoEntitetaSistema; indeksReda++)
            {
                List<double> sample = new List<double>();           // jedan sample/ kombinacija INPUTA u mrezu
                foreach (List<double> kolona in UlazneKolone)
                {
                    sample.Add(kolona[indeksReda]);
                }
                if(indeksReda <= indeksPocetkaTestPodataka)
                {
                    // Nakon sto prodjem kroz jedan red/sample, dodam ga u listu TRAIN INPUTA
                    XTest.Add(sample);
                }
                else
                {
                    // Nakon sto prodjem kroz jedan red/sample, dodam ga u listu INPUTA
                    X.Add(sample);
                }
                

            }
        }

        #endregion

        #region Odredjivanje outputa na osnovu ucitanih izlaznih kolona

        /// <summary>
        /// Inicijalizacija Y outputa u neuronskoj mrezi.
        /// 
        /// Voditi racuna da bude tek nakon sto ucitamo kolone koje predstavljaju izlaze.
        /// 
        /// Ideja je kao i kod odredjivanja inputa samo sad za outpute
        /// </summary>
        /// <param name="procenatTestPodataka"> Procenat koliko zelimo da bude test podataka iz data seta </param>
        private void odrediOutpute(int procenatTestPodataka)
        {
            int ukupnoEntitetaSistema = CitacPodataka.UcitaniRedovi.Length;
            int indeksPocetkaTestPodataka = ukupnoEntitetaSistema * procenatTestPodataka / 100;

            for (int indeksReda = 0; indeksReda < CitacPodataka.UcitaniRedovi.Length; indeksReda++)
            {
                List<double> sample = new List<double>();           // jedan sample/ kombinacija OUTPUT-a na izlazu mreze
                foreach (List<double> kolona in IzlazneKolone)
                {
                    sample.Add(kolona[indeksReda]);
                }

                if(indeksReda <= indeksPocetkaTestPodataka)
                {
                    // Nakon sto prodjem kroz jedan red/sample, dodam ga u listu kombinacija OUTPUTA
                    YTest.Add(sample);
                }
                else
                {
                    // Nakon sto prodjem kroz jedan red/sample, dodam ga u listu kombinacija OUTPUTA
                    Y.Add(sample);
                }

                

            }
        }


        #endregion
    }
}