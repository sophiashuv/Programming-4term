using System.ComponentModel;

namespace Ex_pr_03
{
    // Бiгун характеризується iменем i прiзвищем, а також номером
    public class Runner
    {
        public string Name { get; set; }
        public string FullName => Name + " " + Surname;
        public string Surname { get; set; }
        public uint Id { get; set; }

        public override string ToString() {
            string res = "";
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this)) 
                res += ($"{prop.Name}: {prop.GetValue(this)}\n");
            return res;
        }
    }
}