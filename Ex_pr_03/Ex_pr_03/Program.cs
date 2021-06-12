using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Ex_pr_03
{
    // Завдання 1.Розробити типи для змагання з бiгу по маршруту, який складається зтрьох 
    // етапiв. Бiгун характеризується iменем i прiзвищем, а також номером.Результат на 
    // етапi характеризується номером бiгуна, часом у секундах, заякий спортсмен подолав 
    // етап, номером етапу i його протяжнiстю (в км).Вiдомо, що не всi добiгли до фiнiшу. 
    // Iнформацiя про бiгунiв i результати зетапiв подано окремими xml-файлами. Отримати 
    // з використанням linq:
    //     1. на консолi iмена i прiзвища трьох бiгунiв, якi очолювали бiг на ко-жному етапi;
    //     2. xml-файл у форматi: прiзвище бiгуна, їхнiй загальний час; врахуватилише тих
    //     бiгунiв, якi пробiгли усi етапи маршруту, i впорядкувати вiдпо-вiдно до рейтингу 
    //     за загальним часом;
    //     3. csv-файл у форматi: прiзвище бiгуна, номер етапу, де вiн мав найбiль-шу середню 
    //     швидкiсть, i величина цiєї швидкостi; вивести данi для кожногобiгуна.
    internal class Program
    {
        public static void read_from_csv<T>(List<T> listContainer, string fileName) where T : new()
        {
            using (var reader = new StreamReader(fileName))
            {
                var atters = reader.ReadLine()?.Split(',');
                while (!reader.EndOfStream)
                {
                    var values = reader.ReadLine()?.Split(',');
                    var obj = new T();
                    if (atters != null)
                        for (int i = 0; i < atters.Length; i++)
                        {
                            try
                            {
                                PropertyInfo propertyInfo = obj.GetType().GetProperty(atters[i]);
                                var v = propertyInfo?.Name == "Etp"
                                    ? (Etap) Enum.Parse(typeof(Etap), values?[i] ?? string.Empty, true)
                                    : Convert.ChangeType(values?[i],
                                        propertyInfo?.PropertyType ?? throw new InvalidOperationException());
                                propertyInfo?.SetValue(obj, v, null);
                            }
                            catch (ArgumentException e)
                            {
                                Console.WriteLine($"{e.Message}");
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"Wrong file format");
                            }
                        }

                    listContainer.Add(obj);
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

        public static List<T> read_fromXML<T>(string xf) where T : new()
        {
            var xmldict = new XmlSerializer(typeof(List<T>));
            using (var file = new FileStream(xf, FileMode.OpenOrCreate))
            {
                var listContainer = xmldict.Deserialize(file) as List<T>;
                return listContainer;
            }
        }

        // 1. на консолi iмена i прiзвища трьох бiгунiв, якi очолювали бiг на ко-жному етапi;
        public static void Task1(List<Runner> runners, List<EtapResults> results)
        {
            var res1 = from r in results
                where r.Length == (int) r.Etp
                join rr in runners on r.RunnerId equals rr.Id
                select new {Id = rr.Id, FullName = rr.FullName, Etp = r.Etp, Time = r.Time};

            var res2 = new XElement("TopThreeRunners", from r in res1
                group r by r.Etp
                into grouped
                select new XElement("Etap",
                    new XAttribute("Etp", grouped.Key),
                    from rr in grouped.OrderBy(el => el.Time).Take(3)
                    select new XElement("Runner", rr.FullName)
                   
                ));
            res2.Save("../../task_1.XML");
        }

        //     2. xml-файл у форматi: прiзвище бiгуна, їхнiй загальний час; врахуватилише тих
        //     бiгунiв, якi пробiгли усi етапи маршруту, i впорядкувати вiдпо-вiдно до рейтингу 
        //     за загальним часом;
        public static void Task2(List<Runner> runners, List<EtapResults> results)
        {
            var res1 = from r in results
                where r.Length == (int) r.Etp
                join rr in runners on r.RunnerId equals rr.Id
                select new {Id = rr.Id, FullName = rr.FullName, Etp = r.Etp, Time = r.Time};

            var res2 = new XElement("Task2", from r in res1
                group r by new {r.Id, r.FullName}
                into grouped
                let enum_etps = Enum.GetValues(typeof(Etap)).Cast<Etap>().Select(v => v.ToString()).ToList()
                where grouped.Count() == 3 && grouped.Select(el => el.Etp.ToString()).All(enum_etps.Contains)
                let tot_time = grouped.Sum(el => el.Time)
                orderby tot_time
                select new XElement("Runner", 
                    new XAttribute("FullName", grouped.Key.FullName),
                    from g in grouped
                    orderby g.Etp
                    select new XElement("EtapResult",
                        new XAttribute("Etap", g.Etp), 
                        new XAttribute("Time", g.Time))));
            
            res2.Save("../../task_2.XML");
        }
        
        //     3. csv-файл у форматi: прiзвище бiгуна, номер етапу, де вiн мав найбiльшу середню 
        //     швидкiсть, i величина цiєї швидкостi; вивести данi для кожногобiгуна.
        public static void Task3(List<Runner> runners, List<EtapResults> results)
        {
            var res1 = from r in results
                join rr in runners on r.RunnerId equals rr.Id
                select new {Id = rr.Id, FullName = rr.FullName, Etp = r.Etp, Speed = r.Speed};

            var res2 = new XElement("Task3", from r in res1
                group r by new {r.Id, r.FullName}
                into grouped
                let max_speed = grouped.Max(el => el.Speed)
                let max_etp = grouped.Where(el => el.Speed == max_speed)
                select new XElement("Runner",
                    new XAttribute("FullName", grouped.Key.FullName),
                    new XElement("MaxSpeed", max_speed),
                    new XElement("Etaps", 
                        from k in max_etp
                        select new XElement("Etp", k.Etp))));
            res2.Save("../../task_3.XML");

        }


        public static void Main(string[] args)
        {
            var runners = new List<Runner>();
            var results = new List<EtapResults>();

            read_from_csv(runners, "../../runners.txt");
            read_from_csv(results, "../../results.txt");
            
            write_toXML(runners, "../../runners.XML");
            write_toXML(results, "../../results.XML");

            runners = read_fromXML<Runner>("../../runners.XML");
            results = read_fromXML<EtapResults>("../../results.XML");
            
            Task1(runners, results);
            Task2(runners, results);
            Task3(runners, results);

        }
    }
}