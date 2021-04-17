using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EventsSR
{
    public class Collection
    {
        public List<Student> Lst { get; set; }
        public event EventHandler<EventArgs> myEvent; 

        public Collection()
        {
            Lst = new List<Student>();
        }

        public void ReadTxt(string fileName)
        {
            using (StreamReader sr = new StreamReader(fileName))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    var stud = line.Split(' ');
                    string[] array = new string[stud.Length - 2];
                    Array.Copy(stud, 2, array, 0, stud.Length - 2);
                    var student = new Student(stud[0], stud[1], Array.ConvertAll(array, s => int.Parse(s)));
                    Lst.Add(student);
                }
            }
        }
        
        public override string ToString()
        {
            var res = "";
            foreach (var r in Lst)
                res += "\t" + r.ToString() + "\n";
            return res;
        }
        
        public void Failed()
        {
            foreach(var el in Lst)
                if (el.Grades.Any(i => i <= 2))
                    myEvent?.Invoke(el, new EventArgs());
        }
    }
}