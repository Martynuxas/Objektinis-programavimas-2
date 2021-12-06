using System;

namespace L4Uzsakymai
{
    /// <summary>
    /// Items list
    /// </summary>
    public class Item : IComparable<Item>, IEquatable<Item>
    {
        public int Warehouse { get; private set; }
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        //Open
        public int Amount { get; set; }
        public Item(int warehouse, string name, int amount, decimal price)
        {
            Warehouse = warehouse;
            Name = name;
            Amount = amount;
            Price = price;
        }
        public int CompareTo(Item kitas)
        {
            int rezultatas = kitas.Name.CompareTo(Name);

            if (kitas.Name == Name && kitas.Price == Price)
                rezultatas = Name.CompareTo(kitas.Name);

            return rezultatas;
        }
        public bool Equals(Item kitas)
        {
            if (kitas.GetType() != GetType() || kitas is null)
                return false;

            return Name == kitas.Name;
        }
        public override string ToString()
        {
            return string.Format("|{0,-20}|{1,-20}|{2,20}|{3,20}|", Warehouse, Name, Amount, Price.ToString("F"));
        }
    }
}