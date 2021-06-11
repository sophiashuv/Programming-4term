using System.ComponentModel;

namespace Ex_pr_02
{
    // Вiдвiдувачi характеризуються iдентифiкацiйним номером,
    // прiзвищем,iм’ям та вiдсотком знижки на усi сервiси.
    public class Visitors
    {
        private double _discount;
        
        public uint Id { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public double Discount {
            get => _discount;
            set => _discount = Validation.ValidatePositive(value); 
        }
        public string FullName => Name + " " + Surname;

        public override string ToString() {
            string res = "";
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this)) 
                res += ($"{prop.Name}: {prop.GetValue(this)}\n");
            return res;
        }
    }
}