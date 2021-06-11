using System.ComponentModel;

namespace Ex_pr_02
{
    // Види сервiсiв характеризуються за назвою, iдентифiкацiйним номеромта вартiстю однiєї години тренування
    public class Service
    {
        private double _price;
        
        public string Name { get; set; }
        public uint Id { get; set; }
        public double Price
        {
            get => _price;
            set => _price = Validation.ValidatePrice(value); 
        }

        public override string ToString() {
            string res = "";
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this)) 
                res += ($"{prop.Name}: {prop.GetValue(this)}\n");
            return res;
        }
    }
}