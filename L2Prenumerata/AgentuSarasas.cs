namespace L2Prenumerata
{
    /// <summary>
    /// Agentu sarasas
    /// </summary>
    public sealed class AgentuSarasas
    {
        /// <summary>
        /// Vieno agento mazgo klase
        /// </summary>
        private sealed class AgentuMazgas
        {
            public Agentas Duomenys { get; set; }
            public AgentuMazgas Kitas { get; set; }
            public AgentuMazgas() { }
            public AgentuMazgas(Agentas reiksme, AgentuMazgas adr)
            {
                Duomenys = reiksme;
                Kitas = adr;
            }
        } 
        private AgentuMazgas pr;//saraso pradzia
        private AgentuMazgas pb;//saraso pabaiga
        private AgentuMazgas ss;//saraso sasaja
        
        public AgentuSarasas()
        {
            pb = null;
            pr = null;
            ss = null;
        }

        public Agentas Agentas
        {
            get => default(Agentas);
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
            return  ss != null;
        }
        public Agentas Gauti()
        {
            return ss.Duomenys;
        }
        public void Prideti(Agentas agentas)
        {
            var dd = new AgentuMazgas(agentas, null);
            if(pr!=null)
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
        public void RikiuotiB()
        {
            if (pr == null) return;
            bool keista = true;
            while (keista)
            {
                keista = false;
                var pra = pr;
                while (pra.Kitas != null)
                {
                    if (pra.Duomenys > pra.Kitas.Duomenys)
                    {
                        Agentas Ag = pra.Duomenys;
                        pra.Duomenys = pra.Kitas.Duomenys;
                        pra.Kitas.Duomenys = Ag;
                        keista = true;
                    }
                    pra = pra.Kitas;
                }
            }
        }
        public void Salinimas(Agentas agentas)
        {
            if (pr == null) return;
            if (pr.Duomenys == agentas)
            {
                pr = pr.Kitas;
                Pradzia();
                return;
            }            
            for (Pradzia(); ss.Kitas != null; Toliau())
            {
                if (ss.Kitas.Duomenys == agentas)
                {
                    ss.Kitas = ss.Kitas.Kitas;
                    return;
                }
            }
            if (pb.Duomenys == agentas)
            {
                ss = null;
            }
        }
    }
}