using System;
using System.ComponentModel;

namespace Library_Sequences
{
    public class Reader: IComparable<Reader>
    {
        
        public Reader(string name, string surname, uint id)
        {
            Name = name;
            Surname = surname;
            Id = id;
        }
        public Reader()
        {
        }
        public string Name { get; set; }
        public string Surname { get; set; }
        public uint Id { get; set; }
        public string FullName => Name + " " + Surname;

        public int CompareTo(Reader other)
        {
            return string.Compare(Surname, other.Surname, StringComparison.Ordinal);
        }

        public override string ToString() {
            string res = "";
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this)) 
                res += ($"\t{prop.Name}: {prop.GetValue(this)}\n");
            return res;
        }
    }
}