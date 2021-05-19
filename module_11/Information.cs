using System;
using System.ComponentModel;

namespace module_11
{
    [Serializable]
    public class Information
    {
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public double Amount { get; set; }

        public override string ToString()
        {
            string res = "";
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this))
                res += ($"\t{prop.Name}: {prop.GetValue(this)}\n");
            return res;
        }
    }
}