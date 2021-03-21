using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace module_02_Shuvar
{

    // Завдання 1.
    // Розробити типи для змагання з бiгу по маршруту, який складається зкiлькох етапiв.
    // Бiгун характеризується iменем i прiзвищем, а також номером. Результат на етапi 
    // характеризується номером бiгуна, часом промiжно-го фiнiшу на етапi ( у секундах вiд початку змагання),
    // номером етапу i йогопротяжнiстю (в км). Вiдомо, що не всi добiгли до фiнiшу. Iнформацiя пробiгунiв
    // i результати з етапiв подано окремими файлами. Отримати:
    // 1. iмена i прiзвища трьох бiгунiв, якi очолювали бiг на кожному етапi;
    // 2. iмена i прiзвища бiгунiв, якi пробiгли весь маршрут, вiдповiдно до загального рейтингу;
    // 3. для кожного бiгуна етап, де вiн мав найбiльшу середню швидкiсть iвеличину цiєї швидкостi.
    
    internal class Program
    {
        public static void read_from_txt(List<Runner> container, string fileName)
        {
            // var listOfObjects = File.ReadLines( "/Users/sophiyca/RESTful_API_03/module_01/module_01/asc.csv" ).Skip(1).Select( line => new ASC(line) ).ToList();
            using(var reader = new StreamReader(fileName))
            {
                var atters = reader.ReadLine().Split(',');
                while (!reader.EndOfStream)
                {
                    var values = reader.ReadLine().Split(',');
                    var asc = new Runner();
                    for (int i = 0; i < atters.Length; i++)
                    {
                        PropertyInfo propertyInfo = asc.GetType().GetProperty(atters[i]);
                        propertyInfo.SetValue(asc, Convert.ChangeType(values[i], propertyInfo.PropertyType), null);
                    }
                    container.Add(asc);
                }
            }
        }

        public static List<Results> read_from_json(string fileName)
        {
            using (StreamReader r = new StreamReader(fileName))
            {
                string json = r.ReadToEnd();
                var runners = JsonConvert.DeserializeObject<List<Results>>(json);
                return runners;
            }
        }

        public static void read_from_txt(List<Results> ress, string fileName)
        {
            using (var reader = new StreamReader(fileName))
            {
                while (!reader.EndOfStream)
                {
                    var values = reader.ReadLine().Split(',');
                    var result = new Results();

                    result.RunnerId = (uint) Int32.Parse(values[0]);
                    for (int i = 1; i < values.Length; i += 3)
                    {
                        var et = new EtapResults((Etap) Enum.Parse(typeof(Etap), values[i], true),
                            (uint) Int32.Parse(values[i + 1]), (uint) Int32.Parse(values[i + 2]));
                        result.RunnerResults.Add(et);
                    }

                    ress.Add(result);
                }
            }

            foreach (var r in ress)
                Console.WriteLine(r);
        }


        public static void Task1(List<Results> results, List<Runner> runners)
        {
            void EtapWinners(object etp, int ss)
            {
                List<Results> filteredResults = results.FindAll(e => e[ss].Length == (ulong)(Etap)etp);
                filteredResults.Sort((r2, r1) => r2[ss].Time.CompareTo(r1[ss].Time));
                Console.WriteLine("----------------");
                Console.WriteLine($"{etp} etap: ");
                for (int i = 0; i < 3; i++)
                {
                    var winner = runners.Find(x => x.Id == filteredResults[i].RunnerId);
                    Console.WriteLine(winner.FullName);
                }
            }
            
            int k = 0;
            foreach (var etpp in Enum.GetValues(typeof(Etap)))
            {
                EtapWinners(etpp, k);
                k++;
            }
        }

        public static void Task2(List<Results> results, List<Runner> runners)
        {
            List<Results> filteredResults = results.FindAll(e => e.RunnerResults.Count == Enum.GetNames(typeof(Etap)).Length);
            foreach (var runner in filteredResults)
            {
                var winner = runners.Find(x => x.Id == runner.RunnerId);
                Console.WriteLine(winner.FullName);
            }
        }
        
        public static void Task3(List<Results> results, List<Runner> runners)
        {
            foreach (var res in results)
            {
                var runner = runners.Find(x => x.Id == res.RunnerId);
                var max_etap = res.RunnerResults.Find(x => 
                    x.Speed == res.RunnerResults.Max(y => y.Speed));
                Console.WriteLine($"{runner.FullName}: {max_etap.Etp} - {max_etap.Speed}");
            }
        }


        public static void Main(string[] args)
        {
            var runners = new List<Runner>();
            // var results = new List<Results>();
            read_from_txt(runners, "/Users/sophiyca/RiderProjects/module_02_Shuvar/module_02_Shuvar/runners.csv");
            var results = read_from_json("/Users/sophiyca/RiderProjects/module_02_Shuvar/module_02_Shuvar/results.json");
            // read_from_txt(results, "/Users/sophiyca/RiderProjects/module_02_Shuvar/module_02_Shuvar/results.txt");

            foreach (var r in runners)
                Console.WriteLine(r);
            
            Console.WriteLine("-----------------------------------");
            Task1(results, runners);
            Console.WriteLine("-----------------------------------");
            Task2(results, runners);
            Console.WriteLine("-----------------------------------");
            Task3(results, runners);
        }
    }
}