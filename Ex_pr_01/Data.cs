using System;
using System.ComponentModel;

namespace Ex_pr_01
{
    public class Data
    {
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public Types Type { get; set; }
        public uint Amount { get; set; }

        public override string ToString()
        {
            string res = "";
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this))
                res += ($"\t{prop.Name}: {prop.GetValue(this)}\n");
            return res;
        }
    }
}