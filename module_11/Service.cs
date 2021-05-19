using System.ComponentModel;

namespace module_11
{
    public class Service
    {
        public string Type { get; set; }
       
        public double Price { get; set; }

        public override string ToString() {
            string res = "";
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this)) 
                res += ($"{prop.Name}: {prop.GetValue(this)}\n");
            return res;
        }
    }
}