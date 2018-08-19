using System;
using System.IO;
using System.Xml.Serialization;

namespace YourBudget
{
    [Serializable]
    public class Outcome
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

        private bool isDirectDebit;

        public bool IsDirectDebit
        {
            get { return isDirectDebit; }
            set { isDirectDebit = value; }
        }

        private int day;

        public int Day
        {
            get { return day; }
            set
            {
                if (value > 0 && value < 32)
                {
                    day = value;
                }
            }
        }

        //Constructor default. For serealisation
        public Outcome() { }

        public Outcome(string name, double price, bool isDirectDebit)
        {
            this.name = name;
            this.price = price;
            this.isDirectDebit = isDirectDebit;
        }

        public Outcome(string name, double price, bool isDirectDebit, int day)
        {
            this.name = name;
            this.price = price;
            this.isDirectDebit = isDirectDebit;
            this.day = day;
        }


    }
}
