using System;
using System.Web.UI.WebControls;
using System.IO;

namespace L2Prenumerata
{
    public partial class Prenumerata : System.Web.UI.Page
    {
        const string Rezultatai = "Rezultatai.txt";//rezultatu failas
        const string PradiniaiPrenumeratoriu = "U9aa.txt";//prenumeratoriu duomenu failas
        const string PradiniaiAgentu = "U9bb.txt";//agentu duomenu failas
        StreamWriter Rasyti;
        bool paspaustas = false;//ar paspaustas antras mygtukas
        static AgentuSarasas s1 = new AgentuSarasas();//agentu sarsasas su kruviais
        double VidKruvis;//pasirinkto menesio vidurkis

        public PrenumeratoriuSarasas PrenumeratoriuLinkedList
        {
            get => default(PrenumeratoriuSarasas);
            set
            {
            }
        }

        public AgentuSarasas AgentuLinkedList
        {
            get => default(AgentuSarasas);
            set
            {
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Label2.Visible = false;
            Label3.Visible = false;
            TextBox1.Visible = false;
            Button2.Enabled = false;
            Label10.Visible = false;
            Label9.Visible = false;

            Table1.Visible = false;
            Table2.Visible = false;
            Table3.Visible = false;
            Table4.Visible = false;
            Table5.Visible = false;
            Table6.Visible = false;
        }
        /// <summary>
        /// Iskvieciami metodai duomenu nuskaitymui, rezultatu isvedimui
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button1_Click(object sender, EventArgs e)
        {
            s1 = new AgentuSarasas();
            if (DropDownList1.SelectedValue == "-")
                RequiredFieldValidator1.IsValid = false;

            if (RequiredFieldValidator1.IsValid)
            {
                if (!paspaustas)
                {
                    if (File.Exists(Server.MapPath("App_Data/" + Rezultatai)))
                        File.Delete(Server.MapPath("App_Data/" + Rezultatai));
                    StreamWriter rasyti = new StreamWriter(Server.MapPath("App_Data/" + Rezultatai), true, System.Text.Encoding.UTF8);
                    Rasyti = rasyti;
                }
                Label8.Text = "PRADINIAI DUOMENYS";
                int men = Menuo(DropDownList1.SelectedValue);
                AgentuSarasas agentai = new AgentuSarasas();
                PrenumeratoriuSarasas prenumeratoriai = new PrenumeratoriuSarasas();

                Skaitymas(agentai, prenumeratoriai);
                if (!paspaustas)
                {
                    SpausdintiPradinius(agentai, prenumeratoriai);
                    Rasyti.Close();
                }
                SkaiciuotiKruvi(prenumeratoriai, agentai);
                SpausdintiPrenumeratorius(prenumeratoriai);
                SpausdintiAgentus(agentai);
                Button2.Enabled = true;

                Label11.Text = "REZULTATAI";
                VidKruvis = VidutinisKruvis(men);
                SpausdintiAgentusSuKruviais(s1, men, Table4, "Kiekvieno agento krūvis ir nešiojamos prenumeratos nurodytą mėnesį", Label7);

                AgentuSarasas s3 = AgentuVidutinis(men, VidKruvis);
                Label6.Text = string.Format("<b>Vidutinis mėnesio krūvis:</b> {0}", VidKruvis);
                s3.Pradzia();
                if (s3.Egzistuoja())
                {
                    SpausdintiAgentusSuKruviais(s3, men, Table3, "Sąrašas agentų, kurie dirba daugiau nei vidutinis krūvis nurodytam mėnesiui:", Label5);
                    s3 = new AgentuSarasas();
                    Table3.Visible = true;
                }
                else
                    Label5.Text = "<b>Nėra agentų, kurie dirbtų daugiau nei vidutinis krūvis nurodytam mėnesiui</b>.";
                Label3.Visible = true;
                TextBox1.Visible = true;
                Table1.Visible = true;
                Table2.Visible = true;
                Table4.Visible = true;
                Label5.Visible = true;
                Label6.Visible = true;
                Label8.Visible = true;
                Label11.Visible = true;
            }
            else
            {
                Label2.Visible = false;
                Label8.Visible = false;
                Label11.Visible = false;
                Label4.Visible = false;
                Label6.Visible = false;
                Label5.Visible = false;
                Label7.Visible = false;
            }
        }
        /// <summary>
        /// Metodas, skirtas nuskaityti pradinius prenumeratoriu ir agentu duomenis
        /// </summary>
        /// <param name="agentai">agentu sarasas</param>
        /// <param name="prenumeratoriai">prenumeratoriu sarasas</param>
        void Skaitymas(AgentuSarasas agentai, PrenumeratoriuSarasas prenumeratoriai)
        {
            using (StreamReader skaityti = new StreamReader(Server.MapPath("App_Data/" + PradiniaiPrenumeratoriu), System.Text.Encoding.UTF8))
            {
                string eilute;
                while ((eilute = skaityti.ReadLine()) != null)
                {
                    string[] reiksmes = eilute.Split(';');
                    var prenum = new Prenumeratorius(reiksmes[0], reiksmes[1], int.Parse(reiksmes[2]), int.Parse(reiksmes[3]), reiksmes[4], int.Parse(reiksmes[5]), reiksmes[6]);
                    prenumeratoriai.Prideti(prenum);
                }
            }

            using (StreamReader skaityti = new StreamReader(Server.MapPath("App_Data/" + PradiniaiAgentu), System.Text.Encoding.UTF8))
            {
                string eilute;
                while ((eilute = skaityti.ReadLine()) != null)
                {
                    string[] reiksmes = eilute.Split(';');
                    var agent = new Agentas(reiksmes[0], reiksmes[1], reiksmes[2], reiksmes[3], reiksmes[4]);
                    agentai.Prideti(agent);
                }
            }
        }
        /// <summary>
        /// Metodas, grazinantis menesio numeri pagal pavadinima
        /// </summary>
        /// <param name="men">menesio pavadinimas</param>
        /// <returns>menesio skaiciu</returns>
        static int Menuo(string men)
        {
            switch (men)
            {
                case "sausis":
                    return 1;
                case "vasaris":
                    return 2;
                case "kovas":
                    return 3;
                case "balandis":
                    return 4;
                case "gegužė":
                    return 5;
                case "birželis":
                    return 6;
                case "liepa":
                    return 7;
                case "rugpjūtis":
                    return 8;
                case "rugsėjis":
                    return 9;
                case "spalis":
                    return 10;
                case "lapkritis":
                    return 11;
                case "gruodis":
                    return 12;
            }
            return 0;
        }
        /// <summary>
        /// Metodas, skirtas suskaiciuoti kiekvieno agento kruvi kiekviena menesi
        /// </summary>
        /// <param name="prenumeratoriai">prenumeratoriu sarasas</param>
        /// <param name="agentai">agentu sarasas</param>
        void SkaiciuotiKruvi(PrenumeratoriuSarasas prenumeratoriai, AgentuSarasas agentai)
        {
            s1.Pradzia();
            for (agentai.Pradzia(); agentai.Egzistuoja(); agentai.Toliau())
            {
                Agentas ag = agentai.Gauti();
                string nr = agentai.Gauti().Kodas;
                for (prenumeratoriai.Pradzia(); prenumeratoriai.Egzistuoja(); prenumeratoriai.Toliau())
                {
                    if (prenumeratoriai.Gauti().AgentoKodas == nr)
                    {
                        ag.pridetiKruviIrPrenum(prenumeratoriai.Gauti().LaikotarpioPradzia, prenumeratoriai.Gauti().LaikotarpioIlgis, prenumeratoriai.Gauti().LeidiniuKiekis, prenumeratoriai.Gauti().Kodas);
                    }
                }
                s1.Prideti(ag);
            }
        }
        /// <summary>
        /// Metodas, skirtas sukaiciuoti vidutini agentu kruvi nurodyta menesi
        /// </summary>
        /// <param name="men">nurodytas menuo</param>
        /// <returns>viudtini kruvi</returns>
        double VidutinisKruvis(int men)
        {
            double sum = 0;
            int n = 0;
            for (s1.Pradzia(); s1.Egzistuoja(); s1.Toliau())
            {
                sum += s1.Gauti().GautiKruvi(men);
                ++n;
            }
            return sum / n;
        }
        /// <summary>
        /// Metodas, skirtas sukurti sarasa agentu, kuriu kruvis nurodyta menesi didesnis nei vidutinis menesio kruvis
        /// </summary>
        /// <param name="men">menesis</param>
        /// <param name="vid">vidurkis</param>
        /// <returns>Agentu sarasa</returns>
        static AgentuSarasas AgentuVidutinis(int men, double vid)
        {
            AgentuSarasas s3 = new AgentuSarasas();
            for (s1.Pradzia(); s1.Egzistuoja(); s1.Toliau())
            {
                if (s1.Gauti().GautiKruvi(men) > vid)
                    s3.Prideti(s1.Gauti());
            }
            return s3;
        }
        /// <summary>
        /// Metodas, skirtas atspausdinti agentu rezultatinius sarasaus i tekstini faila
        /// </summary>
        /// <param name="rasyti">streamwriter</param>
        /// <param name="antraste">antraste virs lenteles</param>
        /// <param name="agentai">agentu sarasas</param>
        /// <param name="men">pasirinktas menuo</param>
        void SpaudintiSarasaAgentuRez(StreamWriter rasyti, string antraste, AgentuSarasas agentai, int men)
        {
            rasyti.WriteLine("\r\n" + antraste);
            string antrasteAgent = string.Format("|{0}|{1,-15}|{2,-10}|{3,-10}|{4,-15}|", "AGENTO KODAS", "PAVARDĖ", "VARDAS", "KRŪVIS", "PRENUMERATOS");
            string eilute = new string('-', antrasteAgent.Length);
            rasyti.WriteLine(eilute);
            rasyti.WriteLine(antrasteAgent);
            rasyti.WriteLine(eilute);
            for (agentai.Pradzia(); agentai.Egzistuoja(); agentai.Toliau())
            {
                rasyti.WriteLine(agentai.Gauti().Rezultatam(men));
                rasyti.WriteLine(eilute);
            }
        }
        /// <summary>
        /// Metodas, skirtas atspaudinti pradinius duomenis i tekstini faila
        /// </summary>
        /// <param name="agentai">agentu sarasas</param>
        /// <param name="prenumeratoriai">prenumeratoriu sarasas</param>
        void SpausdintiPradinius(AgentuSarasas agentai, PrenumeratoriuSarasas prenumeratoriai)
        {
            string antrastePrenum = string.Format("|{0,-15}|{1,-16}|{2}|{3}|{4}|{5}|{6}|", "PAVARDĖ", "ADRESAS", "LAIKOTARPIO PRADŽIA", "LAIKOTARPIO ILGIS", "LEIDINIO KODAS", "LEIDINIŲ KIEKIS", "AGENTO KODAS");
            string antrasteAgent = string.Format("|{0}|{1,-15}|{2,-10}|{3,-16}|{4}|", "AGENTO KODAS", "PAVARDĖ", "VARDAS", "ADRESAS", "TELEFONAS");

            Rasyti.WriteLine("DUOMENYS\r\n");
            Rasyti.WriteLine("Prenumeratorių duomenys: ");
            Rasyti.WriteLine(new string('-', antrastePrenum.Length));
            Rasyti.WriteLine(antrastePrenum);
            Rasyti.WriteLine(new string('-', antrastePrenum.Length));
            for (prenumeratoriai.Pradzia(); prenumeratoriai.Egzistuoja(); prenumeratoriai.Toliau())
            {
                Rasyti.WriteLine(prenumeratoriai.Gauti().ToString());
                Rasyti.WriteLine(new string('-', antrastePrenum.Length));
            }
            Rasyti.WriteLine();
            Rasyti.WriteLine("Agentų duomenys: ");
            Rasyti.WriteLine(new string('-', antrasteAgent.Length));
            Rasyti.WriteLine(antrasteAgent);
            Rasyti.WriteLine(new string('-', antrasteAgent.Length));
            for (agentai.Pradzia(); agentai.Egzistuoja(); agentai.Toliau())
            {
                Rasyti.WriteLine(agentai.Gauti().ToString());
                Rasyti.WriteLine(new string('-', antrasteAgent.Length));
            }
            Rasyti.WriteLine();
        }
        /// <summary>
        /// Metodas, skirtas patikrinti, ar kruvis ivestas teisingai 
        /// </summary>
        /// <returns>true, jeigu kruvis netinkamas</returns>
        bool PatikrintiKruvi()
        {
            if (TextBox1.Text.Length == 0)
                return true;

            for (int i = 0; i < TextBox1.Text.Length; i++)
            {
                if (char.IsWhiteSpace(TextBox1.Text[i]))
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Metodas, skirtas atpsaudinti prenumeratoriu duomenis puslapyje
        /// </summary>
        /// <param name="prenumeratoriai">prenumeratoriu sarasas</param>
        void SpausdintiPrenumeratorius(PrenumeratoriuSarasas prenumeratoriai)
        {
            Label2.Text = "<b>Prenumeratorių duomenys:</b>";

            Label2.Visible = true;
            TableRow eilute = new TableRow();
            TableCell pav = new TableCell();
            pav.Text = "PAVARDĖ";
            eilute.Cells.Add(pav);
            TableCell adr = new TableCell();
            adr.Text = "ADRESAS";
            eilute.Cells.Add(adr);
            TableCell lp = new TableCell();
            lp.Text = "LAIKOTARPIO PRADŽIA";
            eilute.Cells.Add(lp);
            TableCell li = new TableCell();
            li.Text = "LAIKOTARPIO ILGIS";
            eilute.Cells.Add(li);
            TableCell kod = new TableCell();
            kod.Text = "LEIDINIO KODAS";
            eilute.Cells.Add(kod);
            TableCell lk = new TableCell();
            lk.Text = "LEIDINIŲ KIEKIS";
            eilute.Cells.Add(lk);
            TableCell ak = new TableCell();
            ak.Text = "AGENTO KODAS";
            eilute.Cells.Add(ak);

            Table1.Rows.Add(eilute);
            Table1.GridLines = GridLines.Both;

            Table1.Rows[0].BackColor = System.Drawing.Color.LightBlue;

            for (prenumeratoriai.Pradzia(); prenumeratoriai.Egzistuoja(); prenumeratoriai.Toliau())
            {
                eilute = new TableRow();
                TableCell lang = new TableCell();
                lang.Text = prenumeratoriai.Gauti().Pavarde;
                eilute.Cells.Add(lang);
                lang = new TableCell();
                lang.Text = prenumeratoriai.Gauti().Adresas;
                eilute.Cells.Add(lang);
                lang = new TableCell();
                lang.Text = prenumeratoriai.Gauti().LaikotarpioPradzia.ToString();
                lang.HorizontalAlign = HorizontalAlign.Right;
                eilute.Cells.Add(lang);
                lang = new TableCell();
                lang.Text = prenumeratoriai.Gauti().LaikotarpioIlgis.ToString();
                lang.HorizontalAlign = HorizontalAlign.Right;
                eilute.Cells.Add(lang);
                lang = new TableCell();
                lang.Text = prenumeratoriai.Gauti().Kodas;
                lang.HorizontalAlign = HorizontalAlign.Center;
                eilute.Cells.Add(lang);
                lang = new TableCell();
                lang.Text = prenumeratoriai.Gauti().LeidiniuKiekis.ToString();
                lang.HorizontalAlign = HorizontalAlign.Right;
                eilute.Cells.Add(lang);
                lang = new TableCell();
                lang.Text = prenumeratoriai.Gauti().AgentoKodas;
                lang.HorizontalAlign = HorizontalAlign.Center;
                eilute.Cells.Add(lang);

                Table1.Rows.Add(eilute);
            }
            Table1.HorizontalAlign = HorizontalAlign.Center;
        }
        /// <summary>
        /// Metodas, skirtas isvesti agentu pradinius duomenis i puslapi
        /// </summary>
        /// <param name="agentai">Agentu sarasas</param>
        void SpausdintiAgentus(AgentuSarasas agentai)
        {
            Label4.Text = "<b>Agentų duomenys:</b>";

            Label4.Visible = true;
            TableRow eilute = new TableRow();
            TableCell ak = new TableCell();
            ak.Text = "AGENTO KODAS";
            eilute.Cells.Add(ak);
            TableCell pav = new TableCell();
            pav.Text = "PAVARDĖ";
            eilute.Cells.Add(pav);
            TableCell vard = new TableCell();
            vard.Text = "VARDAS";
            eilute.Cells.Add(vard);
            TableCell adr = new TableCell();
            adr.Text = "ADRESAS";
            eilute.Cells.Add(adr);
            TableCell tel = new TableCell();
            tel.Text = "TELEFONAS";
            eilute.Cells.Add(tel);

            Table2.Rows.Add(eilute);
            Table2.GridLines = GridLines.Both;
            Table2.Rows[0].BackColor = System.Drawing.Color.LightBlue;

            for (agentai.Pradzia(); agentai.Egzistuoja(); agentai.Toliau())
            {
                eilute = new TableRow();
                TableCell lang = new TableCell();
                lang.Text = agentai.Gauti().Kodas;
                lang.HorizontalAlign = HorizontalAlign.Center;
                eilute.Cells.Add(lang);
                lang = new TableCell();
                lang.Text = agentai.Gauti().Pavarde;
                eilute.Cells.Add(lang);
                lang = new TableCell();
                lang.Text = agentai.Gauti().Vardas;
                eilute.Cells.Add(lang);
                lang = new TableCell();
                lang.Text = agentai.Gauti().Adresas;
                eilute.Cells.Add(lang);
                lang = new TableCell();
                lang.Text = agentai.Gauti().TelNr;
                eilute.Cells.Add(lang);

                Table2.Rows.Add(eilute);
            }
            Table2.HorizontalAlign = HorizontalAlign.Center;
        }
        /// <summary>
        /// Metodas, skirtas atspausdinti agentu duomenis su kruviais, sarasais
        /// </summary>
        /// <param name="agentai">agentu sarasas</param>
        /// <param name="men">pasirinktas menuo</param>
        /// <param name="table">lentele</param>
        /// <param name="antraste">antrastes tekstas</param>
        /// <param name="label">antrastes vieta</param>
        void SpausdintiAgentusSuKruviais(AgentuSarasas agentai, int men, Table table, string antraste, Label label)
        {
            table.Rows.Clear();
            label.Text = string.Format("<b>{0}</b>", antraste);
            label.Visible = true;

            TableRow eilute = new TableRow();
            TableCell ak = new TableCell();
            ak.Text = "AGENTO KODAS";
            eilute.Cells.Add(ak);
            TableCell pav = new TableCell();
            pav.Text = "PAVARDĖ";
            eilute.Cells.Add(pav);
            TableCell vard = new TableCell();
            vard.Text = "VARDAS";
            eilute.Cells.Add(vard);
            TableCell kr = new TableCell();
            kr.Text = "KRŪVIS";
            eilute.Cells.Add(kr);
            TableCell pre = new TableCell();
            pre.Text = "PRENUMERATOS";
            eilute.Cells.Add(pre);

            table.Rows.Add(eilute);
            table.GridLines = GridLines.Both;
            table.Rows[0].BackColor = System.Drawing.Color.OrangeRed;

            for (agentai.Pradzia(); agentai.Egzistuoja(); agentai.Toliau())
            {
                agentai.RikiuotiB();
                eilute = new TableRow();
                TableCell lang = new TableCell();
                lang.Text = agentai.Gauti().Kodas;
                lang.HorizontalAlign = HorizontalAlign.Center;
                eilute.Cells.Add(lang);
                lang = new TableCell();
                lang.Text = agentai.Gauti().Pavarde;
                eilute.Cells.Add(lang);
                lang = new TableCell();
                lang.Text = agentai.Gauti().Vardas;
                eilute.Cells.Add(lang);
                lang = new TableCell();
                lang.Text = agentai.Gauti().GautiKruvi(men).ToString();
                lang.HorizontalAlign = HorizontalAlign.Right;
                eilute.Cells.Add(lang);
                lang = new TableCell();
                lang.Text = agentai.Gauti().GautiPrenumerata(men);
                eilute.Cells.Add(lang);

                table.Rows.Add(eilute);
            }
            table.HorizontalAlign = HorizontalAlign.Center;
        }
        /// <summary>
        /// Atlieka veiksmus su sarasais pagal ivesta kruvi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button2_Click(object sender, EventArgs e)
        {
            paspaustas = true;
            Button1_Click(sender, e);
            double kruvis = 0;
            string prenum = "";
            int men = Menuo(DropDownList1.SelectedValue);

            if (PatikrintiKruvi())
                RangeValidator1.IsValid = false;

            StreamWriter rasyti = new StreamWriter(Server.MapPath("App_Data/" + Rezultatai), append: true);
            if (RangeValidator1.IsValid)
            {
                rasyti.WriteLine("REZULTATAI");
                rasyti.WriteLine("Vidutinis mėnesio krūvis: {0}", VidKruvis);
                SpaudintiSarasaAgentuRez(rasyti, "Kiekvieno agento krūvis ir nešiojamos prenumeratos nurodytą mėnesį", s1, men);

                for (s1.Pradzia(); s1.Egzistuoja();)
                {
                    if (int.Parse(TextBox1.Text) >= s1.Gauti().GautiKruvi(men))
                    {
                        kruvis += s1.Gauti().GautiKruvi(men);
                        prenum += s1.Gauti().GautiPrenumerata(men);
                        s1.Salinimas(s1.Gauti());
                        continue;
                    }
                    s1.Toliau();
                }
                s1.Pradzia();
                if (s1.Egzistuoja())
                {
                    SpausdintiAgentusSuKruviais(s1, men, Table5, "Agentų sąrašas po pašalinimo", Label9);
                    SpaudintiSarasaAgentuRez(rasyti, "Agentų sąrašas po pašalinimo", s1, men);
                    Table5.Visible = true;
                }
                else
                {
                    Label9.Text = "Sąrašas tuščias";
                    Label9.ForeColor = System.Drawing.Color.Red;
                    Label9.Visible = true;
                }
                AgentuSarasas sar = KruvioPasikeitimai(s1, kruvis, VidKruvis, men, prenum);
                if (sar.Egzistuoja())
                {
                    SpausdintiAgentusSuKruviais(sar, men, Table6, "Sąrašas agentų, kuriems pasikeitė krūvis", Label10);
                    SpaudintiSarasaAgentuRez(rasyti, "Sąrašas agentų, kuriems pasikeitė krūvis", sar, men);
                    Table6.Visible = true;
                }
            }
            //Label5.Visible = true;
            //Label6.Visible = true;
            Button2.Enabled = true;
            rasyti.Close();
        }
        /// <summary>
        /// Metodas, skirtas surasti agentu skaiciui, kuriems bus galima paskirstyti pasalintu agentu kruvi
        /// </summary>
        /// <param name="agentai">agentu sarasas</param>
        /// <param name="vid">vidutinis kruvis menesio</param>
        /// <param name="men">menesis</param>
        /// <returns>tinkamu agentu kieki</returns>
        int KiekTinkamuAgentu(AgentuSarasas agentai, double vid, int men)
        {
            int kiekis = 0;
            for (agentai.Pradzia(); agentai.Egzistuoja(); agentai.Toliau())
            {
                if (agentai.Gauti().GautiKruvi(men) < vid)
                    ++kiekis;
            }
            return kiekis;
        }
        /// <summary>
        /// Metodas, skirtas sudaryti agentu sarasa, kuriem pakito nurodyto menesio kruvis
        /// </summary>
        /// <param name="agentai">agentu sarasas</param>
        /// <param name="kruvis">pasalintu agentu kruvis</param>
        /// <param name="vid">vidutinis nurodyto menesio kruvis</param>
        /// <param name="men">pasirinktas menuo</param>
        /// <param name="prenum">pasalintu agentu prenumeratos</param>
        /// <returns>agentu sarasa</returns>
        AgentuSarasas KruvioPasikeitimai(AgentuSarasas agentai, double kruvis, double vid, int men, string prenum)
        {
            int tinkamuKiekis = KiekTinkamuAgentu(agentai, vid, men);
            double kruvioKiekisVienamAgentui = kruvis / (double)tinkamuKiekis;

            AgentuSarasas sar = new AgentuSarasas();

            if (tinkamuKiekis > 0)
                for (agentai.Pradzia(); agentai.Egzistuoja(); agentai.Toliau())
                {
                    if (agentai.Gauti().GautiKruvi(men) < vid)
                    {
                        sar.Prideti(agentai.Gauti());
                        sar.Pradzia();
                        sar.Gauti().PridetiKruviIrPrenumPrieMen(men, kruvioKiekisVienamAgentui, prenum);
                    }
                }
            return sar;
        }
        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}