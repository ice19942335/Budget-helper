using System;
using System.IO;
using System.Xml.Serialization;

namespace YourBudget
{
    [Serializable]
    public class Income
    {
        private double price;

        public double Price
        {
            get { return price; }
            set
            {
                if (value >= 0)
                {
                    price = value;
                }
            }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        //Constructor default. For serealisation
        public Income() { }

        public Income(string name, double price)
        {
            this.name = name;
            this.price = price;
        }

    }
}
