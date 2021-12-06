namespace L2Prenumerata
{
    /// <summary>
    /// Prenumeratoriu sarasas
    /// </summary>
    public sealed class PrenumeratoriuSarasas
    {
        /// <summary>
        /// Vieno prenumeratoriaus mazgo klase
        /// </summary>
        private sealed class PrenumeratoriuMazgas
        {
            public Prenumeratorius Duomenys { get; set; }
            public PrenumeratoriuMazgas Kitas { get; set; }


            public Prenumeratorius Prenumeratorius
            {
                get => default(Prenumeratorius);
                set
                {
                }
            }

            public PrenumeratoriuMazgas() { }
            public PrenumeratoriuMazgas(Prenumeratorius duomenys, PrenumeratoriuMazgas adr)
            {
                Duomenys = duomenys;
                Kitas = adr;
            }
        }
        private PrenumeratoriuMazgas pr; //saraso pradzia
        private PrenumeratoriuMazgas pb; //sarasao pabaiga
        private PrenumeratoriuMazgas ss; //saraso sasaja
        public PrenumeratoriuSarasas()
        {
            pb = null;
            pr = null;
            ss = null;
        }

        public Prenumeratorius Prenumeratorius
        {
            get => default(Prenumeratorius);
            set
            {
            }
        }

        public void Pradzia()
        {
            ss = pr;
        }
        public void Toliau()
        {
            ss = ss.Kitas;
        }
        public bool Egzistuoja()
        {
            return ss != null;
        }
        public Prenumeratorius Gauti()
        {
            return ss.Duomenys;
        }
        public void Prideti(Prenumeratorius prenumeratorius)
        {
            var dd = new PrenumeratoriuMazgas(prenumeratorius, null);
            if (pr != null)
            {
                pb.Kitas = dd;
                pb = dd;
            }
            else
            {
                pr = dd;
                pb = dd;
            }
            ss = pr;
        }
    }
}