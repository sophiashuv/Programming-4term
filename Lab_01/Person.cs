using System;
using System.ComponentModel;
using System.Reflection;

namespace Lab_01
{
    public enum Sex
    {
        male, female
    }
    
    public class Person
    {
        protected Guid id;
        protected string surname;
        protected string name;
        protected Sex personSex;
        
        public Person()
        {
            id = new Guid();
            surname = null;
            name = null;
            personSex = Sex.male;
        }
        
        public Person(string surname, string name, Sex personSex)
        {
            id = Guid.NewGuid();
            this.surname = surname;
            this.name = name;
            this.personSex = personSex;
        }

        public string FullName()
        {
            return ($"{surname} {name}");
        }
        
        public override string ToString() 
        {
            return ($"{id}, {surname}, {name}, {personSex}\n");
        }
        
    }
}