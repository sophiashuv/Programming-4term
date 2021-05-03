using System.ComponentModel;

namespace LINQ_3
{
    class Patient
    {
        public Patient(string name, string surname, uint id)
        {
            Name = name;
            Surname = surname;
            Id = id;
        }
        public Patient()
        {
        }
        public string Name { get; set; }
        public string Surname { get; set; }
        public uint Id { get; set; }
        public string FullName => Name + " " + Surname;

        public override string ToString() {
            string res = "";
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this)) 
                res += ($"\t{prop.Name}: {prop.GetValue(this)}\n");
            return res;
        }
    }
}