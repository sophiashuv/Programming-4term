using System;
using System.ComponentModel;

namespace module_01
{
    public class ASC: IComparable
    {
        
        public ASC()
        {
        }
        public ASC(string marka, double price, double count)
        {
            Marka = marka;
            Price = price;
            Count = count;
        }
        
        public ASC(string line)
        {
            var split = line.Split(',');
            Marka = split[0];
            Price = Convert.ToDouble(split[1]);
            Count = Convert.ToInt32(split[2]);
        }

        public string Marka { get; set; }

        public double Price { get; set; }

        public double Count { get; set; }

        public virtual double AllPayment => Price * Count;

        public int CompareTo(object obj) {
            if (obj == null) return 1;
            ASC otherTemperature = obj as ASC;
            if (otherTemperature != null)
                return this.AllPayment.CompareTo(otherTemperature.AllPayment);
            else
                throw new ArgumentException("Object is not a Temperature");
        }

        public override string ToString() {
            string res = "";
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this)) 
                res += ($"\t{prop.Name}: {prop.GetValue(this)}\n");
            return res;
        }
    }

}