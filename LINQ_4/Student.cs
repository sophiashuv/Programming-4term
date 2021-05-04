using System;
using System.ComponentModel;

namespace LINQ_4
{
    [Serializable]
    public class Student
    {
        // public string Name { get; set; }
        // public int Group { get; set; }
        // public double Rating { get; set; }
        //
        // public override string ToString() {
        //     string res = "";
        //     foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this)) 
        //         res += ($"\t{prop.Name}: {prop.GetValue(this)}\n");
        //     return res;
        // }
        
        public string Surname { get; set; }
        public string Group { get; set; }
        public int Year { get; set; }
        public  int[] Grades { get; set; }
        
        public Student(string surname, string group, int year, params int[] grades)
        {
            Surname = surname;
            Group = group;
            Year = year;
            Grades = grades;
        }
        
        public Student()
        {
           
        }
        
        public override string ToString()
        {
            return $"{Surname} {Group} {Year} [{string.Join(", ", Grades)}]";
        }
    }
}