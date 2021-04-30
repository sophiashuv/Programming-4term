using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace LINQ
{
      // Задано текстовий файл з інформацією про результати ЗНО з математики, укр.мови та 
      // історії (не менше 10 записів). Кожен запис містить прізвище учня, номер школи, 
      // бали (три цілих числа) у вказаній послідовності. Використовуючи запити LINQ:
      //
      // - Обчислити найменший сумарний бал, вивести список учнів, які набрали найменше балів,
      //   впорядкувавши їх прізвища за алфавітом.
      //
      // - Для кожної школи вивести кількість учнів, які в сумі набрали більше ніж 550 балів
      //   (впорядкувати за спаданням кількості учнів, а для однакової кількості – за зростанням номера школи)
      //
      // - Для кожної школи і кожного предмету знайти середній бал, набраний учнями цієї 
      //   школи по даному предмету, вивести в порядку спадання номера школи.
      //
      // - Для кожної школи вивести прізвище учня, що набрав найбільший бал з математики серед 
      //   учнів цієї школи, якщо таких учнів кілька, вивести лише першого по порядку. Інформацію вивести по школах,
      //   впорядкувавши за зростанням номера школи.
      //
      // - Вивести інформацію про учнів, які набрали менше 100 балів по кожному з трьох
      //   предметів у вигляді – номер школи, прізвище учня, бали з кожного предмету, впорядковано за
      //   зростанням номера школи, а якщо номер школи однаковий, то за прізвищем учня.
    internal class Program
    {
        public static void read_from_txt(List<Result> container, string fileName)
        {
            // var listOfObjects = File.ReadLines( "/Users/sophiyca/RESTful_API_03/module_01/module_01/asc.csv" ).Skip(1).Select( line => new Result(line) ).ToList();
            using(var reader = new StreamReader(fileName))
            {
                var atters = reader.ReadLine()?.Split(',');
                while (!reader.EndOfStream)
                {
                    var values = reader.ReadLine()?.Split(',');
                    var asc = new Result();
                    if (atters != null)
                        for (int i = 0; i < atters.Length; i++)
                        {
                            PropertyInfo propertyInfo = asc.GetType().GetProperty(atters[i]);
                            propertyInfo?.SetValue(asc, Convert.ChangeType(values?[i], propertyInfo.PropertyType), null);
                        }

                    container.Add(asc);
                }
            }
        }
        
        
        public static void Main(string[] args)
        {
            List<Result> results = new List<Result>();
            read_from_txt(results, "/Users/sophiyca/RiderProjects/LINQ/LINQ/results.txt");
            foreach (var res in results)
            {
                Console.WriteLine(res);
            }
            
            
            Console.WriteLine("TASK1");
            var smallestSum = results.Min(el => el.Sum());
            var smallestStud = results.Where(el => el.Sum() == smallestSum);
            
            Console.WriteLine($"smallest_sum_1: {smallestSum}");
            Console.WriteLine(String.Join("\n", smallestStud));
            var smallestStud2 = from result in results
                                                    where result.Sum() == smallestSum
                                                    select result;
            
            Console.WriteLine($"smallest_sum_2: {smallestSum}");
            Console.WriteLine(String.Join("\n", smallestStud2));
            
            
            Console.WriteLine("\nTASK2");
            var task2 = from res in results
                where res.Sum() > 550
                group res by res.School
                into schools
                orderby schools.Count() descending, schools.Key 
                select new {School = schools.Key, Amount = schools.Count()};
            
            foreach (var s in task2)
            {
                Console.WriteLine($"{s.School} - {s.Amount}");
            }
            
            
            Console.WriteLine("\nTASK3");
            var task3 = from res in results
                group res by res.School
                into schools
                orderby schools.Key descending 
                select new {
                    School = schools.Key, 
                    Math = schools.Average(el => el.Math), 
                    Ukr = schools.Average(el => el.Ukr),
                    History = schools.Average(el => el.History)
                };
            
            foreach (var s in task3)
            {
                Console.WriteLine($"{s.School}: \n\tUkr: {s.Ukr};\n\tMath: {s.Math};\n\tHistory: {s.History}.");
            }
            
            
            Console.WriteLine("\nTASK4");
            var task4 = from res in results
                group res by res.School
                into schools
                orderby schools.Key
                let maxP = schools.Max(el=>el.Math)
                select new {
                    School = schools.Key, 
                    Student = schools.First(el => el.Math == maxP).Surname, 
                    Points = maxP,
                };

            foreach (var s in task4)
            {
                Console.WriteLine($"{s.School}: {s.Student} - {s.Points};");
            }
            
            
            Console.WriteLine("\nTASK5");
            bool CheckLoverHundred(Result res) => res.History < 100 || res.Math < 100 || res.Ukr < 100;
            
            var task5 = from res in results
                where CheckLoverHundred(res)
                group res by res.School
                into schools
                let studs = from s in schools
                                                    orderby s.Surname
                                                    select s
                // let studs = schools.OrderBy(el => el.Surname)                                   
                orderby schools.Key
                select new {School = schools.Key, Students = studs};

            foreach (var s in task5)
            {
                Console.WriteLine($"{s.School}: ");
                foreach (var stud in s.Students)
                {
                    Console.WriteLine($"\t{stud.Surname}: " +
                                      $"\n\t\tUkr: {stud.Ukr}, " +
                                      $"\n\t\tMath: {stud.Math}, " +
                                      $"\n\t\tHistory: {stud.History}");
                }
            }
        }
    }
}