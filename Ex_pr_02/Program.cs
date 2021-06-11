using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using System.Xml.Linq;

namespace Ex_pr_02
{
        // Розробити типи для облiку сервiсiв тренажерного центру.Види сервiсiв характеризуються 
        // за назвою, iдентифiкацiйним номеромта вартiстю однiєї години тренування.Вiдвiдувачi 
        // характеризуються iдентифiкацiйним номером, прiзвищем,iм’ям та вiдсотком знижки на усi 
        // сервiси. Данi про вiдвiдувачiв та сервiси задано окремими файлами.Данi про тренування 
        // мають формат < дата-iдентифiкацiйний номервiдвiдувача-номер сервiсу-тривалiсть в годинах > 
        // , їх задано xml-файлом.
        // Завдання виконати1-3 з використанням linq 4 з використанням python
        //     1. Отримати xml-файл, де для кожного вiдвiдувача (за прiзвищем таiм’ям) вказана сумарна 
        //     вартiсть усiх його тренувань.
        //     2. Отримати xml-файл, де для кожного виду тренувань вказано щомiся-чну кiлькiсть наданих 
        //     годин та зароблену суму.
        //     3. Отримати xml-файл, де для кожного виду тренувань вказано заро-блену за кожен мiсяць 
        //     суму за спадаючим рейтингом
        //     4. Побудувати графiки для п.3, впорядковуючи данi за порядком мiсяцiв.
    
    internal class Program
    {
        public static void read_from_csv<T>(List<T> listContainer, string fileName) where T :  new()
        {
            using(var reader = new StreamReader(fileName))
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
                                var v = Convert.ChangeType(values?[i],
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
        
        public static List<T> read_fromXML<T>(string xf) where T :  new()
        {
            var xmldict = new XmlSerializer(typeof(List<T>));
            using (var file = new FileStream(xf, FileMode.OpenOrCreate) )
            {
                var listContainer = xmldict.Deserialize(file) as List<T>;
                return listContainer;
            }
        }
        
        //     1. Отримати xml-файл, де для кожного вiдвiдувача (за прiзвищем та iм’ям) вказана сумарна 
        //     вартiсть усiх його тренувань.
        public static void Task1(List<Service> services, List<Visitors> visitors, List<Trainings> trainings)
        {
            var res1 =  from t in trainings 
                join v in visitors on t.VisitorId equals v.Id
                join s in services on t.ServiceId equals s.Id
                select new {Id = t.VisitorId, Visitor = v.FullName, Price = t.Duration*s.Price*v.Discount/100};

            var res2 = new XElement("VisitorsPrices", from r in res1
                group r by new {r.Id, r.Visitor}
                into grouped
                select new XElement("Info", 
                    new XElement("Visitor",grouped.Key.Visitor), 
                    new XElement("Price", Math.Round((double)grouped.Sum(el => el.Price), 2))
                    ));
            
            res2.Save("../../task_1.XML");
        }

        //     2. Отримати xml-файл, де для кожного виду тренувань вказано щомiсячну кiлькiсть наданих 
        //     годин та зароблену суму.
        public static void Task2(List<Service> services, List<Visitors> visitors, List<Trainings> trainings)
        {
            var res1 =  from t in trainings 
                join v in visitors on t.VisitorId equals v.Id
                join s in services on t.ServiceId equals s.Id
                select new 
                {
                    Id = t.ServiceId, 
                    Service = s.Name, 
                    Date = t.Date,  
                    Duration = t.Duration, 
                    Price = t.Duration*s.Price*v.Discount/100
                    
                };
            
            var res2 = new XElement("Task2", 
                from r in res1
                group r by new {r.Id, r.Service}
                into grouped
                select new XElement("Service", 
                    new XAttribute("Name", grouped.Key.Service), 
                    from g in grouped
                        group g by g.Date into grouped2 
                        select new XElement
                        ("Date",
                            new XAttribute("Day", grouped2.Key.Date.ToString("yyyy-dd-MM")),
                            new XElement("Duration", grouped2.Sum(el => el.Duration)),
                            new XElement("Price", Math.Round((double)grouped2.Sum(el => el.Price), 2))
                        )
                ));
            
            res2.Save("../../task_2.XML");
        }
        
        //     3. Отримати xml-файл, де для кожного виду тренувань вказано зароблену за кожен мiсяць 
        //     суму за спадаючим рейтингом
        public static void Task3(List<Service> services, List<Visitors> visitors, List<Trainings> trainings)
        {
            var res1 =  from t in trainings 
                join v in visitors on t.VisitorId equals v.Id
                join s in services on t.ServiceId equals s.Id
                select new 
                {
                    Id = t.ServiceId, 
                    Service = s.Name, 
                    Date = t.Date,
                    Price = t.Duration*s.Price*v.Discount/100
                    
                };
            
            var res2 = new XElement("Task2", 
                from r in res1
                group r by new {r.Id, r.Service}
                into grouped
                select new XElement("Service", 
                    new XAttribute("Name", grouped.Key.Service), 
                    (from g in grouped
                    group g by g.Date into grouped2
                    let price = grouped2.Sum(el => el.Price)
                    orderby price descending 
                    select new XElement
                    ("Date",
                        new XAttribute("Day", grouped2.Key.Date.ToString("yyyy-dd-MM")),
                        new XElement("Price", Math.Round(price, 2))
                    ))
                ));
            
            res2.Save("../../task_3.XML");
        }



        public static void Main(string[] args)
        {
            var services = new List<Service>();
            var visitors = new List<Visitors>();
            var trainings = new List<Trainings>();
            read_from_csv(services,"../../services.csv");
            read_from_csv(visitors,"../../visitors.csv");
            read_from_csv(trainings,"../../trainings.csv");

            write_toXML(trainings, "../../training.XML");
            try
            {
                trainings = read_fromXML<Trainings>("../../training.XML");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine($"{e.Message}");
                return;
            }

            Task1(services, visitors, trainings);
            Task2(services, visitors, trainings);
            Task3(services, visitors, trainings);

        }
    }
}