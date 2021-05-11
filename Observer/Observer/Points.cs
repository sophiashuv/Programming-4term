using System;
using System.ComponentModel;

namespace Observer
{
    public class Points
    {
        
        public Points(int a, int b)
        {
            var rand = new Random();
            X1 = rand.Next(a, b); 
            Y1 = rand.Next(a, b); 
            X2 = rand.Next(a, b); 
            Y2 = rand.Next(a, b); 
        }
        public int X1 { get; set; }
        public int Y1 { get; set; }
        public int X2 { get; set; }
        public int Y2 { get; set; }
        
        public override string ToString() {
            string res = "";
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this)) 
                res += ($"\t{prop.Name}: {prop.GetValue(this)}\n");
            return res;
        }
    }
}