using System.ComponentModel;

namespace module_11
{
    public class Station
    { 
        public int Number { get; set; }
        public string Locality { get; set; }

        public override string ToString()
        {
            string res = "";
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this))
                res += ($"\t{prop.Name}: {prop.GetValue(this)}\n");
            return res;
        }
    }
}