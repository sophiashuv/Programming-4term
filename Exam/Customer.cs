using System.ComponentModel;

namespace Exam
{
    // Замовники характеризуються iдентифiкацiйним номером, прiзвищем та
    // iм’ям, мiсцем проживання.
    public class Customer
    {
        public uint Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Location { get; set; }

        public string Initials => LastName + " " + FirstName[0] + ".";
        
        public override string ToString()
        {
            string res = "";
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this))
                res += ($"{prop.Name}: {prop.GetValue(this)}\n");
            return res;
        }
    }
}