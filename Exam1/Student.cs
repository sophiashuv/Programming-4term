using System.ComponentModel;

namespace Exam1
{
    public class Student
    {
        public uint Id { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Group { get; set; }

        public string FullName => Surname + " " + Name;

        public override string ToString() {
            string res = "";
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this)) 
                res += ($"{prop.Name}: {prop.GetValue(this)}\n");
            return res;
        }
    }
}