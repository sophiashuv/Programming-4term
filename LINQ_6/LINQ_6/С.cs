using System.ComponentModel;

namespace LINQ_6
{
    // С-<Код споживача><Знижка (%)><Назва магазину>
    public class C
    {
        public int Code { get; set; }
        public double Discount { get; set; }
        public string Shop { get; set; }
        
        public override string ToString() {
            string res = "";
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this)) 
                res += ($"\t{prop.Name}: {prop.GetValue(this)}\n");
            return res;
        }
    }
}