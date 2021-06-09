using System.ComponentModel;

namespace Ex_pr_01
{
    public enum Types{Type1, Type2, Type3, Type4}
    public class Service
    {
        private double price;
        public Types Type { get; set; }
       
        public double Price
        {
            get => price;
            set => price = Validation.ValidatePrice(value); 
        }

        public override string ToString() {
            string res = "";
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this)) 
                res += ($"{prop.Name}: {prop.GetValue(this)}\n");
            return res;
        }
    }
}