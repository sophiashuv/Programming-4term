using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Metadata;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;

namespace Exam1
{
    internal class Program
    {
        public static void read_from_txt_students(List<Student> listContainer, string fileName)
        {
            using(var reader = new StreamReader(fileName))
            {
                while (!reader.EndOfStream)
                {
                    var values = reader.ReadLine()?.Split(',');
                    
                    var v = new Student()
                    {
                        Id = uint.Parse(values[0]),
                        Name = values[1],
                        Surname = values[2],
                        Group  = values[3]
                    };
                    listContainer.Add(v);
                }
            }
        }
        
        public static void write_toXML<T>(List<T> listContainer, string xf)
        {
            var xmldict = new XmlSerializer(typeof(List<T>));
            using (var file = new FileStream(xf, FileMode.OpenOrCreate))
            {
                xmldict.Serialize(file, listContainer);
            }
        }
        
        public static List<T> read_fromXML<T>(string xf) where T :  new()
        {
            var xmldict = new XmlSerializer(typeof(List<T>));
            using (var file = new FileStream(xf, FileMode.OpenOrCreate) )
            {
                var listContainer = xmldict.Deserialize(file) as List<T>;
                return listContainer;
            }
        }

        public static void PrepareXmlFile()
        {
            var result1 = new Results()
            {
                Subject = "Programming",
                Res = new List<Result>
                {
                    new Result() {Id = 1, Points = 100},
                    new Result() {Id = 2, Points = 99},
                    new Result() {Id = 3, Points = 70},
                    new Result() {Id = 5, Points = 45},
                    new Result() {Id = 6, Points = 30}
                }
            };
            var result2 = new Results()
            {
                Subject = "Statistics",
                Res = new List<Result>
                {
                    new Result() {Id = 1, Points = 100},
                    new Result() {Id = 2, Points = 89},
                    new Result() {Id = 4, Points = 69},
                    new Result() {Id = 5, Points = 88},
                    new Result() {Id = 6, Points = 100},
                    new Result() {Id = 3, Points = 97},
                    new Result() {Id = 7, Points = 100}
                }
            };
            var result3 = new Results()
            {
                Subject = "Algorithms",
                Res = new List<Result>
                {
                    new Result() {Id = 1, Points = 100},
                    new Result() {Id = 7, Points = 99},
                    new Result() {Id = 3, Points = 70},
                    new Result() {Id = 6, Points = 45},
                    new Result() {Id = 5, Points = 30}
                }
            };
            var results = new List<Results> {result1, result2, result3};
            write_toXML(results, "../../results.XML");
        }

        
        // 1. Отримати xml-файл, де результати систематизованi за схемою <назва
        //     групи, список студентiв, перелiк результатiв з вказанням назви дисциплiни i балiв>. 
        //     За назвою групи i прiзвищем впорядкувати у лексико-графiчному порядку без повторень.
        public static void Task1(List<Student> students, List<Results>results)
        {
            var res1 = from r in results
                from rr in r.Res
                select new {r.Subject, rr.Id, rr.Points};

            var res2 = from r in res1
                join s in students on r.Id equals s.Id
                select new {
                    Group = s.Group, 
                    Id = s.Id, 
                    Student = s.FullName, 
                    Subject = r.Subject, 
                    Points = r.Points
                };

            var res3 = new XElement("Task1", from r in res2
                group r by r.Group
                into grouped
                select new XElement("Res",
                    new XAttribute("Group", grouped.Key), 
                    from g in grouped
                    orderby grouped.Key
                    group g by g.Student into grouped2
                    orderby grouped2.Key
                    select new XElement("Results", 
                        new XAttribute("Student", grouped2.Key), 
                        from gg in grouped2 
                        select new XElement("Result", 
                            new XAttribute("Subject", gg.Subject), 
                            new XAttribute("Points", gg.Points)))));
            
            res3.Save("../../task_1.XML");
        }

        // 2. Отримати з xml-файлу, записаного за умовою завдання 1, новий файл за схемою <назва дисциплiни, 
        // список студентiв (прiзвище та iнiцiали, кiль- кiсть балiв), згрупований за назвами групи>.
        public static void Task2(List<Student> students, List<Results> results)
        {
            XDocument xdoc = XDocument.Load("../../task_1.XML");
            var lv1s = from lv1 in xdoc.Descendants("Res")
                from lv2 in lv1.Descendants("Results")
                from lv3 in lv2.Descendants("Result")
                select new
                {
                    Group = lv1.Attribute("Group").Value,
                    Student = lv2.Attribute("Student").Value,
                    Subject = lv3.Attribute("Subject").Value,
                    Points = lv3.Attribute("Points").Value,
                };

            var res1 = new XElement("Task2", from r in lv1s
                group r by r.Subject into grouped
                select new XElement("Res", 
                    new XAttribute("Subject", grouped.Key),
                    from g in grouped
                    group g by g.Group into grouped2
                    select new XElement("Results",
                        new XAttribute("Group", grouped2.Key),
                        from gg in grouped2
                        select new XElement("Student", 
                            new XAttribute("Name", gg.Student), 
                            new XAttribute("Points", gg.Points)))));
            
            res1.Save("../../task_2.XML");
        }

        // 3. Отримати з xml-файлу, записаного за умовою завдання 1, новий файл за схемою <назва дисциплiни,
        // список груп, впорядкований за вiдсотком якiсної успiшностi, який обчислюють як вiдсоток студентiв,
        //     якi отримали бiльше 70 балiв, вiд кiлькостi студентiв у групi.
        public static void Task3(List<Student> students, List<Results> results)
        {
            XDocument xdoc = XDocument.Load("../../task_1.XML");
            var lv1s = from lv1 in xdoc.Descendants("Res")
                from lv2 in lv1.Descendants("Results")
                from lv3 in lv2.Descendants("Result")
                select new
                {
                    Group = lv1.Attribute("Group").Value,
                    Student = lv2.Attribute("Student").Value,
                    Subject = lv3.Attribute("Subject").Value,
                    Points = uint.Parse(lv3.Attribute("Points").Value),
                };

            var res1 = new XElement("Task3", from r in lv1s
                group r by r.Subject
                into grouped
                select new XElement("Subject", 
                    new XAttribute("Subject", grouped.Key), 
                    from g in grouped 
                    group g by g.Group into grouped2
                    orderby grouped2.Count(el => el.Points > 70)/grouped2.Count()
                    select new XElement("Group", grouped2.Key)));

            res1.Save("../../task_3.XML");
        }
        
        public static void Main(string[] args)
        {
            //PrepareXmlFile();
            
            var students = new List<Student>();
            read_from_txt_students(students, "../../students.txt");
            var results = read_fromXML<Results>("../../results.XML");

            Task1(students, results);
            Task2(students, results);
            Task3(students, results);


        }
    }
}