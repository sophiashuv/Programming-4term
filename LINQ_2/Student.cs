using System;
using System.ComponentModel;

namespace LINQ_2
{
    public enum Faculty {fac1, fac2, fac3, fac4}
    
    public class Student
    {
        //cтудентх арактеризується 
        //     iменем i прiзвищем, назвою факультету, назвою групи, атакож iдентифiкацiйним номером. 
        
        public string Name { get; set; }
        public string Surname { get; set; }
        public Faculty StFaculty{ get; set; }
        public string Group { get; set; }
        public  String Id { get; set; }
        
        public Student() {}
        
        public Student(string name, string surname, Faculty faculty, string group, String id)
        {
            Name = name;
            Surname = surname;
            StFaculty = faculty;
            Group = group;
            Id = id;
        }
        
        public override string ToString() {
            string res = "";
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this)) 
                res += ($"\t{prop.Name}: {prop.GetValue(this)}\n");
            return res;
        }
    }
}