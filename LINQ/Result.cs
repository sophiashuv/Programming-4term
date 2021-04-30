using System.ComponentModel;

namespace LINQ
{
    public class Result
    {
        public string Surname { get; set; }
        public string School { get; set; }
        public  int Ukr { get; set; }
        public  int Math { get; set; }
        public  int History { get; set; }
        
        // public Result(string surname, int school, int ukr, int math, int history)
        // {
        //     Surname = surname;
        //     School = school;
        //     Ukr = ukr;
        //     Math = math;
        //     History = history;
        // }

        public int Sum() => Ukr + Math + History;
        
        public override string ToString() {
            string res = "";
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this)) 
                res += ($"\t{prop.Name}: {prop.GetValue(this)}\n");
            return res;
        }
    }
}