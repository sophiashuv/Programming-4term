using System.ComponentModel;

namespace Exam
{
    // Товар характеризуються цiлочисловим iдентифiкатором, назвою, кате-
    // горiєю, одиницею фасування та цiною одиницi васування.
    public class Item
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public double Unit { get; set; }
        public double Price { get; set; }
        public override string ToString()
        {
            string res = "";
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this))
                res += ($"{prop.Name}: {prop.GetValue(this)}\n");
            return res;
        }
    }
}