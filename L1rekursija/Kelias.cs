namespace L1rekursija
{
    /// <summary>
    /// Konteinerinė klasė, skirta kelionių tarp atskirų punktų kainoms
    /// </summary>
    public class Kelias
    {
        private int[] Kainos;
        public int Kiekis { get; private set; }

        public Kelias(int dydis)
        {
            Kainos = new int[dydis];
            Kiekis = 0;
        }
        public void PridetiKaina(int kaina)
        {
            Kainos[Kiekis++] = kaina;
        }
        public int GautiKaina(int i)
        {
            return Kainos[i];
        }
    }
}