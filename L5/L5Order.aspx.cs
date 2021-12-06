using System;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace L5Order
{
    public partial class Uzsakymai : System.Web.UI.Page
    {
        const string Result = "Result.txt";//rezultatu failas
        StreamWriter Write;
        static List<Item> s1 = new List<Item>();
        static List<Item> same = new List<Item>();
        static List<Order> fail = new List<Order>();
        protected void Page_Load(object sender, EventArgs e)
        {
            Label2.Visible = false;
            Table1.Visible = false;
            Table2.Visible = false;
            Table3.Visible = false;
            Table4.Visible = false;
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            decimal inputAmount = decimal.Parse(TextBox1.Text);
            string path = Server.MapPath("App_Data/Items/");
            s1 = new List<Item>();//Succeeded order items list
            fail = new List<Order>();//Failed order items list
            same = new List<Item>();//Same items sum

            if (!FileUpload1.HasFile && !FileUpload1.FileName.EndsWith(".txt"))
            {
                RequiredFieldValidator2.IsValid = false;
            }

            if (RequiredFieldValidator2.IsValid && RequiredFieldValidator3.IsValid)
            {

                if (File.Exists(Server.MapPath("App_Data/" + Result)))
                    File.Delete(Server.MapPath("App_Data/" + Result));
                StreamWriter write = new StreamWriter(Server.MapPath("App_Data/" + Result), true, System.Text.Encoding.UTF8);
                Write = write;

                Label8.Text = "<center>PRIMARY DATA</center>";

                List<Order> orders = new List<Order>();
                List<Item> items = new List<Item>();
                try
                { CheckData(); }
                catch (FormatException ex)
                {
                    Response.Write($"<script>alert(\"{ex.Message}\");location=location;</script>");
                    Response.End();
                }
                try
                { ReadOrders(orders); }
                catch (FileNotFoundException)
                {
                    Response.Write("<script>alert(\"nerastas užsakymo failas\");location=location;</script>");
                    Response.End();
                }
                try
                { ReadItems(items, path); }
                catch (Exception ex)
                {
                    Response.Write($"<script>alert(\"{ex.Message}\");location=location;</script>");
                    Response.End();
                }
                PrintDataOfOrders(orders);//to result file
                PrintDataOfItems(items);//to result file

                PrintOrder(orders);
                PrintItems(items);
                SumList(same, items);
                PrintSameItemsSum(same);

                AddBestPriceItemToList(items, orders, s1, fail);

                Label11.Text = "<center>RESULT</center>";
                while (DoOrderExceedsAmount(s1, inputAmount))
                {
                    RemoveExpensive(s1, fail);
                }
                PrintSuccededListResult(s1);
                //-----------------------------------------------------LAMBDA
                fail = fail.OrderBy(nn => nn.Name).ThenBy(nn => nn.PriceRange).ToList();
                PrintFailList(fail);
                PrintDataOfSucceesed(s1);//to result file
                PrintDataOfFails(fail);//to result file
                Write.Close();
                Table1.Visible = true;
                Table2.Visible = true;
                Table4.Visible = true;
                Table3.Visible = true;
                Label5.Visible = true;
                Label8.Visible = true;
                Label11.Visible = true;
                Label12.Visible = true;
            }
            else
            {
                Label2.Visible = false;
                Label8.Visible = false;
                Label11.Visible = false;
                Label4.Visible = false;
                Label5.Visible = false;
                Label7.Visible = false;
                Label12.Visible = false;
            }
        }
        private void CheckData()
        {
            if (int.Parse(TextBox1.Text) < 0)
                throw new FormatException("* Netinkamas sumos skaičius");
            if (TextBox1.Text.Length == 0)
                throw new FormatException("* Neįvedėte sumos");
        }
        /// <summary>
        /// Method to get best price
        /// </summary>
        /// <param name="items"></param>Items list
        /// <param name="ItemName"></param>Item name
        /// <param name="ItemRangePrice"></param>Item range price
        /// <param name="amount"></param>Item amount
        /// <returns></returns>Returns items best price
        static decimal BestPriceWithAmount(List<Item> items, string ItemName, decimal ItemRangePrice, double amount)
        {
            //-----------------------------------------------------LINQ
            var titems = from item in items
                         where item.Name == ItemName && item.Price <= ItemRangePrice && item.Amount >= amount
                         select item;
            return titems.Max(nn => nn.Price);
        }
        /// <summary>
        /// Method which sums same items names prices
        /// </summary>
        /// <param name="items"></param>items list
        /// <param name="name"></param>item name
        /// <returns></returns>
        static decimal SumSamePrice(List<Item> items, string name)
        {
            decimal sum = 0;
            //-----------------------------------------------------LINQ
            var titems = from item in items
                        where item.Name == name
                         select item;


                //-----------------------------------------------------LAMBDA
                sum = titems.Sum(x => x.Price * x.Amount);
            return sum;     
        }
        /// <summary>
        /// Method which adds item to same items list
        /// </summary>
        /// <param name="same"></param>same items list
        /// <param name="items"></param>items list
        static void SumList(List<Item> same, List<Item> items)
        {
            //-----------------------------------------------------LINQ
            var titems = from item in items
                         where DoesntExist(same, item.Name)
                         select item;
            foreach(var item in titems)
            {
                decimal price = SumSamePrice(items, item.Name);
                Item agent = new Item(0, item.Name, 0, price);
                same.Add(agent);
            }
        }
        /// <summary>
        /// Method which check if item exist in list or not
        /// </summary>
        /// <param name="same"></param>same items list
        /// <param name="name"></param>item name
        /// <returns></returns>
        static bool DoesntExist(List<Item> same, string name)
        {
            //-----------------------------------------------------LINQ
            var titems = from item in same
                         where item.Name == name
                         select item;
            if (titems.Count() == 0) return true;
            return false;
        }
        /// <summary>
        /// Method to get item which have best price warehouse
        /// </summary>
        /// <param name="items"></param>Items list
        /// <param name="ItemName"></param>Item  name
        /// <param name="ItemRangePrice"></param>Item range price
        /// <param name="amount"></param>Item amount
        /// <returns></returns>Returns items which have best price warehouse
        static int BestPriceWithAmountWareHouse(List<Item> items, string ItemName, decimal ItemRangePrice, double amount)
        {
            decimal price = 0;
            int warehouse = 0;
            //-----------------------------------------------------LINQ
            var titems = from item in items
                         where item.Name == ItemName && item.Price <= ItemRangePrice && item.Amount >= amount
                         select item;

            foreach (var item in titems)
            {
                if (item.Price > price)
                {
                    price = item.Price;
                    warehouse = item.Warehouse;
                }
            }
            return warehouse;
        }
        /// <summary>
        /// Method to check if we have appropriate price
        /// </summary>
        /// <param name="items"></param>Items list
        /// <param name="name"></param>Item name
        /// <param name="price"></param>Item price
        /// <returns></returns>Returns true if we have appropriate price, or no - false
        static bool RigthPrice(List<Item> items, string name, decimal price)
        {
            //-----------------------------------------------------LINQ
            var titems = from item in items
                         where item.Name == name && item.Price <= price
                         select item;
            if (titems.Count() == 0) return false;
            return true;
        }
        /// <summary>
        /// Method to check if we have item in stock
        /// </summary>
        /// <param name="items"></param>Items list
        /// <param name="name"></param>Item name
        /// <returns></returns>Returns true if we have in stock, return false - no
        static bool AreInStock(List<Item> items, string name)
        {
            //-----------------------------------------------------LINQ
            var titems = from item in items
                         where item.Name == name
                         select item;
            if (titems.Count() == 0) return false;
            return true;
        }
        /// <summary>
        /// Method which check if order exceeds amount
        /// </summary>
        /// <param name="s1"></param>succeeded list
        /// <param name="iamount"></param> item amount
        /// <returns></returns>Returns true if order exceeds amount, else false.
        static bool DoOrderExceedsAmount(List<Item> s1, decimal iamount)
        {
            //-----------------------------------------------------LAMBDA
            if (s1.Sum(x => x.Price * x.Amount) > iamount) return true;
            return false;
        }
        /// <summary>
        /// Method checks if item exist in warehouse
        /// </summary>
        /// <param name="fail"></param>Fail list
        /// <param name="name"></param>Item name
        /// <param name="price"></param>Item price
        /// <returns></returns>Returns true if item exist in warehouse, else false
        static bool NotExists(List<Order> fail, string name, decimal price)
        {
            //-----------------------------------------------------LINQ
            var titems = from item in fail
                         where item.Name == name
                         select item;
            if (titems.Count() == 0) return true;
            return false;
        }
        /// <summary>
        /// Methoc which gets price of items in succeeded list
        /// </summary>
        /// <param name="s1"></param>Succeeded list
        /// <returns></returns>Returns succeeded list price of items
        static decimal PriceOfItems(List<Item> s1)
        {
            //-----------------------------------------------------LAMBDA
            return s1.Sum(x => x.Price * x.Amount);
        }
        /// <summary>
        /// Method check if order succeeded or not
        /// </summary>
        /// <param name="fail"></param>Fail list
        /// <returns></returns>Returns true if order succeded, else false
        static bool OrderSucceeded(List<Order> fail)
        {
            //-----------------------------------------------------LAMBDA
            if (fail.Sum(x => x.Amount) > 0) return false;
            return true;
        }
        /// <summary>
        /// Method adds best price item to list
        /// </summary>
        /// <param name="items"></param>Items list
        /// <param name="order"></param>Order list
        /// <param name="s1"></param>Succeeded list
        /// <param name="fail"></param>Fail list
        void AddBestPriceItemToList(List<Item> items, List<Order> order, List<Item> s1, List<Order> fail)
        {
            for (int i = 0; i < order.Count; i++)
            {
                if (!AreInStock(items, order[i].Name))
                {
                    Order agent = new Order(order[i].Name, order[i].Amount, order[i].PriceRange, "Nebuvo sandėliuose");
                    fail.Add(agent);
                    continue;
                }
                if (!RigthPrice(items, order[i].Name, order[i].PriceRange) && AreInStock(items, order[i].Name))
                {
                    Order agent = new Order(order[i].Name, order[i].Amount, order[i].PriceRange, "Per brangu");
                    fail.Add(agent);
                    continue;
                }
                decimal price = BestPriceWithAmount(items, order[i].Name, order[i].PriceRange, order[i].Amount);
                int warehouse = BestPriceWithAmountWareHouse(items, order[i].Name, order[i].PriceRange, order[i].Amount);
                Item test = new Item(warehouse, order[i].Name, order[i].Amount, price);
                s1.Add(test);
            }
        }
        /// <summary>
        /// Method removes expensive item amount: -1 or remove from list if amount = 0
        /// </summary>
        /// <param name="s1"></param>Succeeded list
        /// <param name="fail"></param>Fail list
        void RemoveExpensive(List<Item> s1, List<Order> fail)
        {
            decimal price = 0;
            string name = "";
            for (int i = 0; i < s1.Count; i++)
            {
                if (s1[i].Amount == 0) s1.Remove(s1[i]);
                //-----------------------------------------------------LAMBDA
                if (s1[i].Price == s1.Max(x => x.Price) && s1[i].Amount > 0)
                {
                    price = s1[i].Price;
                    name = s1[i].Name;
                }
            }
            AddRemovedToFailList(s1, fail, name, price);
        }
        /// <summary>
        /// Method which from succeeded list removed item adds to fail list
        /// </summary>
        /// <param name="s1"></param>Succeeded list
        /// <param name="fail"></param>Fail list
        /// <param name="name"></param>Item name
        /// <param name="price"></param>Price
        void AddRemovedToFailList(List<Item> s1, List<Order> fail, string name, decimal price)
        {
            for (int i = 0; i < s1.Count; i++)
            {
                if (s1[i].Name == name && price == s1[i].Price && s1[i].Amount > 0)
                {
                    s1[i].Amount--;
                    if (s1[i].Amount == 0) s1.Remove(s1[i]);
                    if (NotExists(fail, name, price))
                    {
                        Order agent = new Order(s1[i].Name, 1, s1[i].Price, "Netilpo į užsakymą");
                        fail.Add(agent);
                    }
                    else
                    {
                        for (int j = 0; j < fail.Count; j++)
                        {
                            if (fail[j].Name == name && fail[j].PriceRange == price && fail[j].Amount > 0) fail[j].Amount++;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Send result file
        /// </summary>
        private void SendFile(string text, string name)
        {
            byte[] data = Encoding.UTF8.GetBytes(text);
            Response.Clear();
            Response.AddHeader("Content-Disposition", $"attachment; filename={name}");
            Response.ContentType = "text/plain";
            Response.OutputStream.Write(data, 0, data.Length);
            Response.End();
        }
        //----------------------------------READING----------------------------------------------------------------
        /// <summary>
        /// Method to read warehouses
        /// </summary>
        /// <param name="items"></param>Items list
        /// <param name="path"></param>Path
        void ReadItems(List<Item> items, string path)
        {
            string[] foundOrders = Directory.GetFiles(path, "*_Items.txt");
            foreach (string foundorder in foundOrders)
            {
                string[] line = File.ReadAllLines(foundorder, System.Text.Encoding.UTF8);
                string warehouseID = line[0];
                for (int i = 1; i < line.Length; i++)
                {
                    string[] values = line[i].Split(';');
                    //if (Convert.ToDecimal(values[2]) < 0 || int.Parse(warehouseID) < 0 || int.Parse(values[1]) < 0)
                        //throw new FormatException("* In items found negative number!");
                    var item = new Item(int.Parse(warehouseID), values[0], int.Parse(values[1], System.Globalization.CultureInfo.InvariantCulture), Convert.ToDecimal(values[2], System.Globalization.CultureInfo.InvariantCulture));
                    items.Add(item);
                }
            }
        }
        /// <summary>
        /// Method to read Order
        /// </summary>
        /// <param name="orders"></param>Order list
        void ReadOrders(List<Order> orders)
        {
            using (StreamReader read = new StreamReader(Server.MapPath("App_Data/Orders/" + FileUpload1.FileName), System.Text.Encoding.UTF8))
            {
                string line;
                while ((line = read.ReadLine()) != null)
                {
                    string[] values = line.Split(';');
                    //if (int.Parse(values[1]) < 0 || Convert.ToDecimal(values[2]) < 0)
                        //throw new FormatException("* In order found negative number!");
                    var order = new Order(values[0], int.Parse(values[1], System.Globalization.CultureInfo.InvariantCulture), Convert.ToDecimal(values[2], System.Globalization.CultureInfo.InvariantCulture), "");
                    orders.Add(order);
                }
            }
        }
        //----------------------------------READING END----------------------------------------------------------------
        //-----------------------------------PRINTING------------------------------------------------------------------
        /// <summary>
        /// Method to print order data to .txt file
        /// </summary>
        /// <param name="orders"></param>Order list
        void PrintDataOfOrders(List<Order> orders)
        {
            string heading = string.Format("|{0,-20}|{1,20}|{2,20}|", "NAME", "AMOUNT", "RANGE PRICE");
            Write.WriteLine("DATA\r\n");
            Write.WriteLine("Order: ");
            Write.WriteLine(new string('-', heading.Length));
            Write.WriteLine(heading);
            Write.WriteLine(new string('-', heading.Length));
            for (int i = 0; i < orders.Count; i++)
            {
                Write.WriteLine(orders[i].ToString());
                Write.WriteLine(new string('-', heading.Length));
            }
            Write.WriteLine();
        }
        /// <summary>
        /// Method to print items list to .txt file
        /// </summary>
        /// <param name="items"></param>Items list
        void PrintDataOfItems(List<Item> items)
        {
            string heading = string.Format("|{0,-20}|{1,-20}|{2,20}|{0,20}|", "WAREHOUSE", "NAME", "AMOUNT", "PRICE");

            Write.WriteLine("DATA\r\n");
            Write.WriteLine("Items: ");
            Write.WriteLine(new string('-', heading.Length));
            Write.WriteLine(heading);
            Write.WriteLine(new string('-', heading.Length));
            for (int i = 0; i < items.Count; i++)
            {
                Write.WriteLine(items[i].ToString());
                Write.WriteLine(new string('-', heading.Length));
            }
        }
        /// <summary>
        /// Method to print succeeded list to .txt file
        /// </summary>
        /// <param name="s1"></param>Succeeded list
        void PrintDataOfSucceesed(List<Item> s1)
        {
            string heading = string.Format("|{0,-20}|{1,-20}|{2,20}|{0,20}|", "WAREHOUSE", "NAME", "AMOUNT", "PRICE");

            Write.WriteLine("DATA\r\n");
            Write.WriteLine("Succeesed list: ");
            Write.WriteLine(new string('-', heading.Length));
            Write.WriteLine(heading);
            Write.WriteLine(new string('-', heading.Length));
            for (int i = 0; i < s1.Count; i++)
            {
                Write.WriteLine(s1[i].ToString());
                Write.WriteLine(new string('-', heading.Length));
            }
        }
        /// <summary>
        /// Method to print fail list to .txt file
        /// </summary>
        /// <param name="fail"></param>
        void PrintDataOfFails(List<Order> fail)
        {
            string heading = string.Format("|{0,-20}|{1,20}|{2,20}|{3,-20}|", "NAME", "AMOUNT", "PRICE RANGE", "REASON");

            Write.WriteLine("DATA\r\n");
            Write.WriteLine("Fail list: ");
            Write.WriteLine(new string('-', heading.Length));
            Write.WriteLine(heading);
            Write.WriteLine(new string('-', heading.Length));
            for (int i = 0; i < fail.Count; i++)
            {
                Write.WriteLine(fail[i].ToString());
                Write.WriteLine(new string('-', heading.Length));
            }
        }
        /// <summary>
        /// Method to print table of order list
        /// </summary>
        /// <param name="orders"></param>Order list
        void PrintOrder(List<Order> orders)
        {
            Label4.Text = "<center><b>Order data:</center></b>";

            Label4.Visible = true;
            TableRow eilute = new TableRow();
            TableCell ak = new TableCell();
            ak.Text = "<b><center>NAME</center>";
            eilute.Cells.Add(ak);
            TableCell pav = new TableCell();
            pav.Text = "<b><center>AMOUNT</center>";
            eilute.Cells.Add(pav);
            TableCell vard = new TableCell();
            vard.Text = "<b><center>RANGE PRICE</center>";
            eilute.Cells.Add(vard);

            Table2.Rows.Add(eilute);
            Table2.GridLines = GridLines.Both;
            Table2.Rows[0].BackColor = System.Drawing.Color.LightBlue;

            for (int i = 0; i < orders.Count; i++)
            {
                eilute = new TableRow();
                TableCell lang = new TableCell();
                lang.Text = orders[i].Name;
                eilute.Cells.Add(lang);
                lang.HorizontalAlign = HorizontalAlign.Left;
                lang = new TableCell();
                lang.Text = orders[i].Amount.ToString();
                eilute.Cells.Add(lang);
                lang.HorizontalAlign = HorizontalAlign.Right;
                lang = new TableCell();
                lang.Text = orders[i].PriceRange.ToString("F");
                eilute.Cells.Add(lang);
                lang.HorizontalAlign = HorizontalAlign.Right;

                Table2.Rows.Add(eilute);
            }
            Table2.HorizontalAlign = HorizontalAlign.Center;
        }
        /// <summary>
        /// Method to print table of items list
        /// </summary>
        /// <param name="items"></param>Items list
        void PrintItems(List<Item> items)
        {
            Label2.Text = "<center><b>Items data:</center></b>";

            Label2.Visible = true;
            TableRow eilute = new TableRow();
            TableCell pav = new TableCell();
            pav.Text = "<b><left>WAREHOUSE</left>";
            eilute.Cells.Add(pav);
            TableCell adr = new TableCell();
            adr.Text = "<b><left>NAME</left>";
            eilute.Cells.Add(adr);
            TableCell lp = new TableCell();
            lp.Text = "<b><right>AMOUNT</right>";
            eilute.Cells.Add(lp);
            TableCell li = new TableCell();
            li.Text = "<b><right>PRICE</right>";
            eilute.Cells.Add(li);

            Table1.Rows.Add(eilute);
            Table1.GridLines = GridLines.Both;

            Table1.Rows[0].BackColor = System.Drawing.Color.LightBlue;

            for (int i = 0; i < items.Count; i++)
            {
                eilute = new TableRow();
                TableCell lang = new TableCell();
                lang.Text = items[i].Warehouse.ToString();
                eilute.Cells.Add(lang);
                lang.HorizontalAlign = HorizontalAlign.Left;
                lang = new TableCell();
                lang.Text = items[i].Name;
                eilute.Cells.Add(lang);
                lang.HorizontalAlign = HorizontalAlign.Left;
                lang = new TableCell();
                lang.Text = items[i].Amount.ToString();
                eilute.Cells.Add(lang);
                lang.HorizontalAlign = HorizontalAlign.Right;
                lang = new TableCell();
                lang.Text = items[i].Price.ToString("F");
                eilute.Cells.Add(lang);
                lang.HorizontalAlign = HorizontalAlign.Right;

                Table1.Rows.Add(eilute);
            }
            Table1.HorizontalAlign = HorizontalAlign.Center;
        }
        /// <summary>
        /// Method to print table of same items list
        /// </summary>
        /// <param name="same"></param>Same items list
        void PrintSameItemsSum(List<Item> same)
        {
            Label15.Text = "<center><b>Same items data:</center></b>";

            Label15.Visible = true;
            TableRow eilute = new TableRow();
            TableCell adr = new TableCell();
            adr.Text = "<b><left>NAME</left>";
            eilute.Cells.Add(adr);
            TableCell li = new TableCell();
            li.Text = "<b><right>PRICE</right>";
            eilute.Cells.Add(li);

            Table5.Rows.Add(eilute);
            Table5.GridLines = GridLines.Both;

            Table5.Rows[0].BackColor = System.Drawing.Color.LightBlue;

            for (int i = 0; i < same.Count; i++)
            {
                eilute = new TableRow();
                TableCell lang = new TableCell();
                lang = new TableCell();
                lang.Text = same[i].Name;
                eilute.Cells.Add(lang);
                lang.HorizontalAlign = HorizontalAlign.Left;
                lang = new TableCell();
                lang.Text = same[i].Price.ToString("F");
                eilute.Cells.Add(lang);
                lang.HorizontalAlign = HorizontalAlign.Right;

                Table5.Rows.Add(eilute);
            }
            Table5.HorizontalAlign = HorizontalAlign.Center;
        }
        /// <summary>
        /// Method to print table of succeeded list
        /// </summary>
        /// <param name="s1"></param>Succeded list
        void PrintSuccededListResult(List<Item> s1)
        {
            if (PriceOfItems(s1) != 0)
            {
                Label7.Text = "<center><b>Succeeded order data:</b></center>";

                Label7.Visible = true;
                TableRow eilute = new TableRow();
                TableCell pav = new TableCell();
                pav.Text = "<b><center>WAREHOUSE</center></b>";
                eilute.Cells.Add(pav);
                TableCell adr = new TableCell();
                adr.Text = "<b><center>NAME</center></b>";
                eilute.Cells.Add(adr);
                TableCell lp = new TableCell();
                lp.Text = "<b><center>AMOUNT</center></b>";
                eilute.Cells.Add(lp);
                TableCell li = new TableCell();
                li.Text = "<b><center>PRICE</center></b>";
                eilute.Cells.Add(li);

                Table4.Rows.Add(eilute);
                Table4.GridLines = GridLines.Both;

                Table4.Rows[0].BackColor = System.Drawing.Color.LightGreen;

                for (int i = 0; i < s1.Count; i++)
                {
                    eilute = new TableRow();
                    TableCell lang = new TableCell();
                    lang.Text = s1[i].Warehouse.ToString();
                    eilute.Cells.Add(lang);
                    lang.HorizontalAlign = HorizontalAlign.Left;
                    lang = new TableCell();
                    lang.Text = s1[i].Name;
                    eilute.Cells.Add(lang);
                    lang.HorizontalAlign = HorizontalAlign.Left;
                    lang = new TableCell();
                    lang.Text = s1[i].Amount.ToString();
                    eilute.Cells.Add(lang);
                    lang.HorizontalAlign = HorizontalAlign.Right;
                    lang = new TableCell();
                    lang.Text = s1[i].Price.ToString("F");
                    eilute.Cells.Add(lang);
                    lang.HorizontalAlign = HorizontalAlign.Right;

                    Table4.Rows.Add(eilute);
                }
                Table4.HorizontalAlign = HorizontalAlign.Center;
                Label6.Text = String.Format("<b><center>Total items cost: {0} eur.</center></b>", PriceOfItems(s1));

                Label6.Visible = true;
            }
            else
            {
                Label6.Text = String.Format("<b><center>Neiviena prekė nepriimta.</center></b>");

                Label6.Visible = true;
            }
        }
        /// <summary>
        /// Method to print table of fail list
        /// </summary>
        /// <param name="fail"></param>Fail list
        void PrintFailList(List<Order> fail)
        {
            if (OrderSucceeded(fail) == true)
            {
                Label13.Text = String.Format("<b><center>Užsakymas sėkmingas, visos prekės priimtos!</center></b>");
                Label5.Visible = false;
                Label13.Visible = true;
            }
            else
            {
                Label5.Text = "<center><b>fail data:</center></b>";

                Label5.Visible = true;
                TableRow eilute = new TableRow();
                TableCell adr = new TableCell();
                adr.Text = "<b><left>NAME</left></b>";
                eilute.Cells.Add(adr);
                TableCell lp = new TableCell();
                lp.Text = "<b><left>AMOUNT</left></b>";
                eilute.Cells.Add(lp);
                TableCell li = new TableCell();
                li.Text = "<b><center>PRICE RANGE</center></b>";
                eilute.Cells.Add(li);
                TableCell ri = new TableCell();
                ri.Text = "<b><center>REASON</center></b>";
                eilute.Cells.Add(ri);

                Table3.Rows.Add(eilute);
                Table3.GridLines = GridLines.Both;

                Table3.Rows[0].BackColor = System.Drawing.Color.DarkRed;

                for (int i = 0; i < fail.Count; i++)
                {
                    eilute = new TableRow();
                    TableCell lang = new TableCell();
                    lang.Text = fail[i].Name;
                    eilute.Cells.Add(lang);
                    lang.HorizontalAlign = HorizontalAlign.Left;
                    lang = new TableCell();
                    lang.Text = fail[i].Amount.ToString();
                    eilute.Cells.Add(lang);
                    lang.HorizontalAlign = HorizontalAlign.Left;
                    lang = new TableCell();
                    lang.Text = fail[i].PriceRange.ToString("F");
                    eilute.Cells.Add(lang);
                    lang.HorizontalAlign = HorizontalAlign.Right;
                    lang = new TableCell();
                    lang.Text = fail[i].Reason;
                    eilute.Cells.Add(lang);
                    lang.HorizontalAlign = HorizontalAlign.Right;
                    Table3.Rows.Add(eilute);
                }
                Table3.HorizontalAlign = HorizontalAlign.Center;
            }
        }
        //-----------------------------------PRINTING END---------------------------------------------------------------
        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            var text = new StringBuilder();
            string heading = string.Format("|{0,-20}|{1,20}|{2,20}|{3,-20}|", "NAME", "AMOUNT", "PRICE RANGE", "REASON");

            text.AppendLine("DATA\r\n");
            text.AppendLine("Fail list: ");
            text.AppendLine(new string('-', heading.Length));
            text.AppendLine(heading);
            text.AppendLine(new string('-', heading.Length));
            for (int i = 0; i < fail.Count; i++)
            {
                text.AppendLine(fail[i].ToString());
                text.AppendLine(new string('-', heading.Length));
            }
            SendFile(text.ToString(), "Results.txt");
        }
    }
}