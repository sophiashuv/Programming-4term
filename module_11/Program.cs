using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace module_11
{
    
    // Розробити типи для облiку обслуговування у мережi автозаправок наосновi записiв про 
    // обслуговування, якi мiстять номер заправки, дату, видпального, кiлькiсть пального в 
    // лiтрах.Кожна заправка характеризується номером i назвою селища-мiста, данiусiх 
    // заправок задано окремим файлом. Цiна кожного виду палива (за 1 л)задано окремим 
    // файлом.Данi про обслуговування задано окремим xml-файлом.Завдання 
    // виконати1
    // -3 з використання linq4 з використанням python
    // 1. Отримати xml-файл, в якому для кожної заправки вказати вирученусуму за весь час, 
    // вказуючи номер заправки та її мiсцезнаходження.
    // 2. Отримати xml-файл, в якому для кожного селища-мiста вказати ви-ручену 
    // суму усiма заправками разом за усi днi.
    // 3. Отримати xml-файл, в якому вказати перелiк заправок у кожномуселищi-мiстi, 
    // якi отримали найбiльшу виручку сумарно за за усi днi.
    // 4. Побудувати графiки для пункту 1, вказуючи по осi абсцис номер за-правки.
    internal class Program
    {
        public static void read_from_txt<T>(List<T> container, string fileName) 
            where T :  new()
        {
            using(var reader = new StreamReader(fileName))
            {
                var atters = reader.ReadLine().Split(',');
                while (!reader.EndOfStream)
                {
                    var values = reader.ReadLine().Split(',');
                    var asc = new T();
                    for (int i = 0; i < atters.Length; i++)
                    {
                        PropertyInfo propertyInfo = asc.GetType().GetProperty(atters[i]);
                        var v = Convert.ChangeType(values[i], propertyInfo.PropertyType);
                        propertyInfo.SetValue(asc, v, null);
                    }
                    container.Add(asc);
                }
            }
        }
        
        public static void write_toXML<T>(List<T> studentList, string xf)
        {
            var xmldict = new XmlSerializer(typeof(List<T>));
            using (var file = new FileStream(xf, FileMode.OpenOrCreate))
            {
                xmldict.Serialize(file, studentList);
            }
        }
    

        public static List<T> read_fromXML<T>(string xf)where T :  new()
        {
            var xmldict = new XmlSerializer(typeof(List<T>));
            using (var file = new FileStream(xf, FileMode.OpenOrCreate) )
            {
                var newcust = xmldict.Deserialize(file) as List<T>;
                return newcust;
            }
        }
        
        // 1. Отримати xml-файл, в якому для кожної заправки вказати виручену суму за весь час, 
        // вказуючи номер заправки та її мiсцезнаходження.
        public static void Task1(List<Station> stations, List<Service>services, List<Information>informations)
        {
            var query = from st in stations
                join inf in informations on st.Number equals inf.Number
                select new {Station = st, Information = inf};

            var res = from inf in query
                join serv in services on inf.Information.Type equals serv.Type into joined
                select new
                {
                    Number = inf.Station.Number, 
                    Locality = inf.Station.Locality,
                    TotalPrice = joined.Sum(el => el.Price * inf.Information.Amount)
                };
            
            var final = new XElement("StationsPrice", from r in res
                group r by new {r.Number, r.Locality}
                into gr
                select new XElement("Stations",
                    new XElement("Number" , gr.Key.Number),
                    new XElement("Locality" , gr.Key.Locality),
                    new XElement("TotalPrice", gr.Sum(el => el.TotalPrice)
                    )));
            
            var xmldict = new XmlSerializer(typeof(XElement));
            using (var file = new FileStream("../../task1.XML", FileMode.OpenOrCreate))
            {
                xmldict.Serialize(file, final);
            }
        }
        
        // 2. Отримати xml-файл, в якому для кожного селища-мiста вказати ви-ручену 
        // суму усiма заправками разом за усi днi.
        public static void Task2(List<Station> stations, List<Service> services, List<Information> informations)
        {
            var query = from st in stations
                join inf in informations on st.Number equals inf.Number
                select new {Station = st, Information = inf};
            
            var res = from inf in query
                join serv in services on inf.Information.Type equals serv.Type into joined
                select new {
                   Locality = inf.Station.Locality,
                   TotalPrice = joined.Sum(el => el.Price * inf.Information.Amount)
                };
            
            var final = new XElement("TotalPrice", from r in res
                group r by r.Locality into gr
                select new XElement("Locations",
                    new XElement("Locality" , gr.Key),
                    new XElement("TotalPrice", gr.Sum(el => el.TotalPrice)
                )));

            
            var xmldict = new XmlSerializer(typeof(XElement));
            using (var file = new FileStream("../../task2.XML", FileMode.OpenOrCreate))
            {
                xmldict.Serialize(file, final);
            }
        }
        // 3. Отримати xml-файл, в якому вказати перелiк заправок у кожномуселищi-мiстi, 
        // якi отримали найбiльшу виручку сумарно за за усi днi.
        public static void Task3(List<Station> stations, List<Service> services, List<Information> informations)
        {
            // var query = from st in stations
            //     join inf in informations on st.Number equals inf.Number
            //     select new {Station = st, Information = inf};
            //
            // var res = from inf in query
            //     join serv in services on inf.Information.Type equals serv.Type
            //     select new {
            //         Station = inf.Station, Information = inf.Information, Price = serv
            //     };
            //
            // var ress = from inf in res
            //     group res by inf.Station.Locality
            //     into s
            //     let maxP = res.Max(el => el.Price.Price * el.Information.Amount)
            //     select new {Location = s.Key, A = res.Where(el => el.Price.Price * el.Information.Amount == maxP)};
            //
            //
            // foreach (var kvp in ress)
            // {
            //     Console.WriteLine($"{kvp.Location}");
            //     foreach (var kvpp in kvp.A)
            //     {
            //         Console.WriteLine($"\t{kvpp.Station.Number}");
            //     }
            //}
            var r = from i in informations
                join s in services on i.Type equals s.Type
                select new {Info = i, Serv = s};

            var rr = from i in r
                group i by i.Info.Number
                into grouped
                select new {Number = grouped.Key, 
                    Price = grouped.Sum(el => el.Info.Amount * el.Serv.Price)};

            var result = from i in rr
                join s in stations on i.Number equals s.Number
                select new {Location = s.Locality, Info = i};

            var grouped_res = new XElement("Max_in_Cities", from i in result
                group i by i.Location
                into grouped
                select new XElement("Cities",
                    new XElement("Location", grouped.Key),
                    new XElement("Stations", grouped.Where(el =>
                        el.Info.Price == grouped.Max(k => k.Info.Price)).
                        Select(el => el.Info.Number).ToArray())
                ));
            
            grouped_res.Save("../../task3.XML");
            
            
        }
        
        public static void Main(string[] args)
        {

            var stations = new List<Station>();
            var services = new List<Service>();
            var informations = new List<Information>();
            read_from_txt(stations,"../../stations.txt");
            read_from_txt(services,"../../services.txt");
            read_from_txt(informations,"../../informations.txt");
            
            write_toXML(informations, "../../informations.XML");
            informations = read_fromXML<Information>("../../informations.XML");

            Task1(stations, services, informations);
            Task2(stations, services, informations);
            Task3(stations, services, informations);

        }
    }
}