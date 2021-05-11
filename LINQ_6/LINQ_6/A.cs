using System.ComponentModel;

namespace LINQ_6
{
    // А-<Код споживача><Прізвище><Вулиця проживання> 
    public class A
    {
        public int Code { get; set; }
        public string Surname { get; set; }
        public string Street { get; set; }
        
        public override string ToString() {
            string res = "";
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this)) 
                res += ($"\t{prop.Name}: {prop.GetValue(this)}\n");
            return res;
        }
    }
}