using System.Collections.Generic;
using System.ComponentModel;

namespace EventsSR
{
    public class Student {
        
        public string Surname { get; set; }
        public string Group { get; set; }
        public  int[] Grades { get; set; }
        
        public Student(string surname, string group, params int[] grades)
        {
            Surname = surname;
            Group = group;
            Grades = grades;
        }
        
        public override string ToString()
        {
            return $"{Surname} {Group} [{string.Join(", ", Grades)}]";
        }
    }
}