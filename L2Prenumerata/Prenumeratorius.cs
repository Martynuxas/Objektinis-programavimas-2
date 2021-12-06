namespace L2Prenumerata
{
    /// <summary>
    /// Prenumeratoriaus duomenu klase
    /// </summary>
    public class Prenumeratorius
    {
        public string Pavarde { get; set; }
        public string Adresas { get; set; }
        public int LaikotarpioPradzia { get; set; }
        public int LaikotarpioIlgis { get; set; }
        public string Kodas { get; set; }
        public int LeidiniuKiekis { get; set; }
        public string AgentoKodas { get; set; }

        public Prenumeratorius(string pavarde, string adresas, int pradzia, int ilgis, string kodas, int kiekis, string aKodas)
        {
            Pavarde = pavarde;
            Adresas = adresas;
            LaikotarpioPradzia = pradzia;
            LaikotarpioIlgis = ilgis;
            Kodas = kodas;
            LeidiniuKiekis = kiekis;
            AgentoKodas = aKodas;
        }
        public override string ToString()
        {
            return string.Format("|{0,-15}|{1,-16}|{2,19}|{3,17}|{4,14}|{5,15}|{6,12}|", Pavarde, Adresas, LaikotarpioPradzia, LaikotarpioIlgis, Kodas, LeidiniuKiekis, AgentoKodas);
        }
    }
}