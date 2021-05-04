using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace LINQ_4
{
    internal class Program
    {
        public static List<Student> ReadJson(string fileName)
        {
            string json = File.ReadAllText(fileName);
            var studentList = JsonConvert.DeserializeObject<List<Student>>(json);
            return studentList;
        }

        public static void write_toXML(List<Student> studentList, string xf)
        {
            var xmldict = new XmlSerializer(typeof(List<Student>));
            using (var file = new FileStream(xf, FileMode.OpenOrCreate))
            {
                xmldict.Serialize(file, studentList);
            }
        }
        
        public static List<Student> read_fromXML(string xf)
        {
            var xmldict = new XmlSerializer(typeof(List<Student>));
            using (var file = new FileStream(xf, FileMode.OpenOrCreate) )
            {
                var newcust = xmldict.Deserialize(file) as List<Student>;
                Console.WriteLine(newcust);
                return newcust;
            }
        }

        // public static void Task1(List<Student> studentList)
        // {
        //     var result = studentList.GroupBy(o => o.Group).ToDictionary(o=> o.Key, o=> o.Count());
        //     foreach (var v in result)
        //         Console.WriteLine($"{v.Key} - {v.Value}");
        //     
        //     var result2 = studentList.GroupBy(o => o.Group).ToDictionary(o=> o.Key, 
        //         o=> o.GroupBy(el=>el.Name).ToDictionary(el
        //                 => el.Key,el=>el.Select(k => new{Rating=k.Rating})));
        //
        //     foreach (var v in result2)
        //     {
        //         Console.WriteLine($"{v.Key}");
        //         foreach (var vv in v.Value)
        //         {
        //             Console.WriteLine($"\t{vv.Key}");
        //             foreach (var vvv in vv.Value)
        //             {
        //                 Console.WriteLine($"\t\t{vvv}");
        //             }
        //         }
        //     }
        // }
        //
        // public static void Task2(List<Student> studentList)
        // {
        //     var result = studentList.GroupBy(o=>o.Group).ToDictionary(o => o.Key, o => o.Where(i=>i.Rating == o.Max(a=>a.Rating)).Select(j=>j.Name));
        //     foreach (var v in result)
        //     {
        //         Console.WriteLine($"{v.Key}: ");
        //         foreach (var vv in v.Value)
        //         {
        //             Console.WriteLine($"\t{vv}");
        //         }
        //     }
        //
        // }

        public static void Task1(List<Student> studentList)
        {
            var res = studentList.GroupBy(el => el.Group)
                .ToDictionary(el => el.Key, el => 
                    el.Max(k => k.Grades.Average())).OrderByDescending(el=>
                    el.Value).ThenByDescending(el=>el.Key);
            
            foreach (var v in res)
            {
                Console.WriteLine($"{v.Key}: {v.Value}");
            }

            var res2 = from s in studentList
                group s by s.Group
                into gr
                select new {Group = gr.Key, Points = gr.Max(el => el.Grades.Average())};

            var res22 = from s in res2
                orderby s.Points descending, s.Group descending 
                select String.Format($"{s.Group}: {s.Points}");

            Console.WriteLine(String.Join("\n", res22));
        }

        public static void Task2(List<Student> studentList)
        {
            var res = studentList.GroupBy(el => el.Group)
                .ToDictionary(el => el.Key, el =>
                    el.Count(k => k.Grades.Average() >= studentList.Average(kk => kk.Grades.Average())))
                .Select(el=>String.Format($"{el.Key}: {el.Value}"));
            
            Console.WriteLine(String.Join("\n", res));
        }

        public static void Task3(List<Student> studentList)
        {
            var res = studentList.GroupBy(el => el.Year).
                Select(el => new {Year = el.Key, Amount = el.Count()}).OrderBy(el=>el.Amount);
            foreach (var y in res)
            {
                Console.WriteLine($"{y.Year}: {y.Amount}");
            }
        }

        public static void Task4(List<Student> studentList)
        {
            var res = studentList.GroupBy(el => el.Group).Select(el => new
            {
                Group = el.Key,
                Students = el.ToList().OrderByDescending(k => k.Year).
                    ThenBy(k => k.Surname)
                    .Select(kk => new {Name = kk.Surname})
            });

            foreach (var v in res)
            {
                Console.WriteLine($"{v.Group}: {String.Join(" ", v.Students.Select(el => String.Format(el.Name)))}");
            }
        }


        public static void Main(string[] args)
        {
            var studentList = ReadJson("../../Data2.json");
            Console.WriteLine(String.Join("\n\n", studentList));
            write_toXML(studentList, "/Users/sophiyca/RiderProjects/LINQ_4/LINQ_4/Data.XML");
            studentList = read_fromXML("../../Data.XML");
            Task1(studentList);
            Task2(studentList);
            Task3(studentList);
            Task4(studentList);
            
            int[] nums = { 10, -6, 3, 1, -4, 25 };
            var result = nums.Select(l=> new { Num = l, Coupled = l%2==0}).OrderByDescending(o=> o.Coupled);
            Console.WriteLine(String.Join(" " ,result.Select(el => el.Num)));

        }
    }
}