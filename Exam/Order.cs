using System.ComponentModel;

namespace Exam
{
    // Замовлення задано у форматi < iдентифiкатор замовника, iдентифiка-
    // тор товару, кiлькiсть в одиницях фасування >
    public class Order
    {
        public uint UserId { get; set; }
        public uint TovarId{ get; set; }
        public uint Amount { get; set; }
        
        public override string ToString() {
            string res = "";
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this)) 
                res += ($"{prop.Name}: {prop.GetValue(this)}\n");
            return res;
        }
    }
}