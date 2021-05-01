using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace LINQ_2
{
    internal class Program
    {
        
        // Завдання 1.
        // Розробити типи для пiдведення пiдсумкiв екзаменвцiйної сесiї. Студентх арактеризується 
        //     iменем i прiзвищем, назвою факультету, назвою групи, атакож iдентифiкацiйним номером. 
        //     Результати iспитiв характеризується на-звою предмета, номером талона (екзкменацiйна 
        //     вiдомiсть – це талон No1),номером студента, кiлькiстю балiв в семестрi i за предмет 
        //     вцiлому. Iнформа-цiя про студентiв подана окремим файлом. Результати iспитiв також 
        //     поданокiлькома окремими файлами. 
        //     Отримати:
        //         1. iмена i прiзвища студентiв, якi успiшно здали iспити з усiх предметiв,вiдповiдно до рейтингу;
        //         2. для кожного предмету iмена i прiзвища студентiв, якi успiшно здалиiспити по талону No2;
        //         3. для кожного факультету список студентiв на вiдрахування iз зазна-ченням 
        //         назви предмету i сумарної кiлькостi балiв.
        
        public static void read_from_txt<TK>(List<TK> container, string fileName)
            where TK: new ()
        {
            using(var reader = new StreamReader(fileName))
            {
                var atters = reader.ReadLine()?.Split(',');
                while (!reader.EndOfStream)
                {
                    var values = reader.ReadLine()?.Split(',');
                    var asc = new TK();

                    if (atters != null)
                        for (int i = 0; i < atters.Length; i++)
                        {
                            try
                            {
                                PropertyInfo propertyInfo = asc.GetType().GetProperty(atters[i]);
                                propertyInfo?.SetValue(asc, Convert.ChangeType(values?[i], propertyInfo.PropertyType),
                                    null);
                            }
                            catch (Exception)
                            {
                                try
                                {
                                    var att = (Faculty) Enum.Parse(typeof(Faculty), values?[i], true);
                                    PropertyInfo propertyInfo = asc.GetType().GetProperty(atters[i]);
                                    propertyInfo?.SetValue(asc, Convert.ChangeType(att, propertyInfo.PropertyType),
                                        null);
                                }
                                catch (Exception)
                                {
                                    var att = (Talon) Enum.Parse(typeof(Talon), values?[i], true);
                                    PropertyInfo propertyInfo = asc.GetType().GetProperty(atters[i]);
                                    propertyInfo?.SetValue(asc, Convert.ChangeType(att, propertyInfo.PropertyType),
                                        null);
                                }
                                
                            }
                        }
                    container.Add(asc);
                }
            }
        }

        // 1. iмена i прiзвища студентiв, якi успiшно здали iспити з усiх предметiв,вiдповiдно до рейтингу;
        public static void Task1(List<Student> students, List<Result> results)
        {
            var passedStudents =
                from r in results
                group r by r.StudentId
                into g
                from s in students
                where s.Id == g.Key && g.All(el => el.TotalPoints > 50)
                select String.Format($"{s.Name} {s.Surname}");
            
            Console.WriteLine("TASK1");
            Console.WriteLine(String.Join("\n", passedStudents));
        }
        
        // 2. для кожного предмету iмена i прiзвища студентiв, якi успiшно здали iспити по талону No2;
        public static void Task2(List<Student> students, List<Result> results)
        {
            var subjectStudents =
                from r in results
                where r.TalonNum == Talon.Talon2 && r.TotalPoints > 50
                group r by r.Subject
                into g
                from s in students
                from k in g
                where k.StudentId == s.Id
                select new {Subject = g.Key, FullName = String.Format($"{s.Name}, {s.Surname}")}
                into stud
                group stud by stud.Subject;
            
            Console.WriteLine("TASK2");
            foreach (var s in subjectStudents)
            {
                Console.WriteLine($"{s.Key}: ");
                foreach (var fullname in s)
                {
                    Console.WriteLine($"\t{fullname.FullName};");
                }
            }
        }
        
        // 3. для кожного факультету список студентiв на вiдрахування iз зазна-ченням 
        // назви предмету i сумарної кiлькостi балiв.
        public static void Task3(List<Student> students, List<Result> results)
        {
            var res = from st in results
                where st.TotalPoints < 50
                join s in students
                    on st.StudentId equals s.Id
                select new
                {
                    Student = s,
                    Result = st
                }
                into kk
                group kk by kk.Student.StFaculty
                into grouped
                let k = grouped.Select(el => String.Format($"\t{el.Result.Subject}: {el.Student.Name} {el.Student.Surname}"))
                select String.Format(
                    $"{grouped.Key}:\n{String.Join("\n", k)}");
            
            Console.WriteLine("TASK3");
            Console.WriteLine(String.Join("\n", res));
            
        }
        
        
        
        
        
        public static void Main(string[] args)
        {
            // var rand = new Random();
            // for (int i = 0; i < 10; i++)
            // {
            //     Console.WriteLine($"Subject{rand.Next(1, 8)},talon{rand.Next(1, 5)},{rand.Next(0, 51)},{rand.Next(50, 101)}");
            // }
            
            var students = new List<Student>();
            var results = new List<Result>();
            read_from_txt<Student>(students, "/Users/sophiyca/RiderProjects/LINQ_2/LINQ_2/students.csv");
            read_from_txt<Result>(results, "/Users/sophiyca/RiderProjects/LINQ_2/LINQ_2/results.csv");
            
           
            // foreach (var p in students)
            //     Console.WriteLine($"{p}");
            //
            // foreach (var p in results)
            //     Console.WriteLine($"{p}");
            
            Task1(students, results);
            Task2(students, results);
            Task3(students, results);

        }
    }
}