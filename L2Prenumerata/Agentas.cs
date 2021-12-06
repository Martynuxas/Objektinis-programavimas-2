using System;

namespace L2Prenumerata
{
    /// <summary>
    /// Agento duomenu klase
    /// </summary>
    public class Agentas : IComparable<Agentas>
    {
        public string Kodas { get; set; }
        public string Pavarde { get; set; }
        public string Vardas { get; set; }
        public string Adresas { get; set; }
        public string TelNr { get; set; }
        private double[] Kruvis = new double[12];
        private string[] Prenumeratos = new string[100];

        public Agentas(string kodas, string pavarde, string vardas, string adresas, string telNr)
        {
            Kodas = kodas;
            Pavarde = pavarde;
            Vardas = vardas;
            Adresas = adresas;
            TelNr = telNr;
        }
        public int CompareTo(Agentas kitas)
        {
            int rezultatas = kitas.Kodas.CompareTo(Kodas);

            if (kitas.Kodas == Kodas)
                rezultatas = Kodas.CompareTo(kitas.Kodas);

            return rezultatas;
        }
        public override string ToString()
        {
            return string.Format("|{0,-12}|{1,-15}|{2,-10}|{3,-16}|{4,9}|", Kodas, Pavarde, Vardas, Adresas, TelNr);
        }
        public string Rezultatam(int men)
        {
            return string.Format("|{0,-12}|{1,-15}|{2,-10}|{3,10}|{4,-15}|", Kodas, Pavarde, Vardas, GautiKruvi(men), GautiPrenumerata(men));
        }
        public void pridetiKruviIrPrenum(int prad, int ilgis, int kiekis, string prenum)
        {
            prad--;
            while (prad != 12 & ilgis != 0)
            {
                Kruvis[prad] += kiekis;
                if (Prenumeratos[prad] == null)
                    Prenumeratos[prad] = prenum + " ";
                else if (!Prenumeratos[prad].Contains(prenum))
                    Prenumeratos[prad] += prenum + " ";
                ++prad;
                --ilgis;
            }
        }
        public void PridetiKruviIrPrenumPrieMen(int men, double kruvis, string prenum)
        {
            Kruvis[men - 1] += kruvis;
            if (Prenumeratos[men - 1] != prenum)
                Prenumeratos[men - 1] += prenum;
        }
        public double GautiKruvi(int men)
        {
            return Kruvis[men - 1];
        }
        public string GautiPrenumerata(int men)
        {
            return Prenumeratos[men - 1];
        }
        static public bool operator >(Agentas a1, Agentas a2)
        {
            int ip = string.Compare(a1.Vardas, a2.Vardas, StringComparison.CurrentCulture);
            int ip1 = string.Compare(a1.Pavarde, a2.Pavarde, StringComparison.CurrentCulture);

            if (ip1 < 1)
                return ip > 0;
            else
                return ip1 > 0;
        }
        static public bool operator <(Agentas a1, Agentas a2)
        {
            return !(a1 > a2);
        }
    }
}