using System.ComponentModel;

namespace Exam1
{
    public class Result
    {
        public uint Id { get; set; }
        public uint Points { get; set; }

        public override string ToString() {
            string res = "";
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this)) 
                res += ($"\t{prop.Name}: {prop.GetValue(this)}\n");
            return res;
        }
    }
}