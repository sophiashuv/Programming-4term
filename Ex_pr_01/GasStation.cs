using System.ComponentModel;

namespace Ex_pr_01
{
    // Кожна заправка характеризується номером i назвою селища-мiста
    public class GasStation
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