using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Ex_pr_01
{
    // Розробити типи для облiку обслуговування у мережi автозаправок на основi записiв 
    // про обслуговування, якi мiстять номер заправки, дату, видпального, кiлькiсть 
    // пального в лiтрах.Кожна заправка характеризується номером i назвою селища-мiста, 
    // данi усiх заправок задано окремим файлом. Цiна кожного виду палива (за 1 л)задано 
    // окремим файлом. Данi про обслуговування задано окремим xml-файлом.
    // Завдання виконати1-3 з використання linq4 з використанням python
    //     1. Отримати xml-файл, в якому для кожної заправки вказати вирученусуму за весь час, 
    //        вказуючи номер заправки та її мiсцезнаходження.
    //     2. Отримати xml-файл, в якому для кожного селища-мiста вказати ви-ручену суму 
    //        усiма заправками разом за усi днi.
    //     3. Отримати xml-файл, в якому вказати перелiк заправок у кожномуселищi-мiстi, якi 
    //        отримали найбiльшу виручку сумарно за за усi днi.
    //     4. Побудувати графiки для пункту 1, вказуючи по осi абсцис номер за-правки.
    
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
                                var v = propertyInfo?.Name == "Type"
                                    ? (Types) Enum.Parse(typeof(Types), values?[i] ?? string.Empty, true)
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
        
        public static List<T> read_fromXML<T>(string xf) where T :  new()
        {
            var xmldict = new XmlSerializer(typeof(List<T>));
            using (var file = new FileStream(xf, FileMode.OpenOrCreate) )
            {
                var listContainer = xmldict.Deserialize(file) as List<T>;
                return listContainer;
            }
        }
        
        // 1. Отримати xml-файл, в якому для кожної заправки вказати виручену суму за весь час, 
        // вказуючи номер заправки та її мiсцезнаходження.
        public static void Task1(List<GasStation> stations, List<Service> services, List<Data> data)
        {
            var res1 = from d in data 
                join s in services on d.Type equals s.Type
                select new {Number = d.Number, Price = s.Price*d.Amount};
            
            var res3 = new XElement("GasStationsPrices", from r in res1
                group r by r.Number into grouped
                join s in stations on grouped.Key equals s.Number
                select new XElement("Info", 
                    new XElement("Number", grouped.Key), 
                    new XElement("Location", s.Locality), 
                    new XElement("Price", grouped.Sum(el => el.Price)))
                );
            
            res3.Save("../../task_1.XML");
        }
        
        // 2. Отримати xml-файл, в якому для кожного селища-мiста вказати виручену 
        // суму усiма заправками разом за усi днi.
        public static void Task2(List<GasStation> stations, List<Service> services, List<Data> data)
        {
            var res1 =  from d in data 
                join s in services on d.Type equals s.Type
                join ss in stations on d.Number equals ss.Number
                select new {Locality = ss.Locality, Price = s.Price*d.Amount};
            
            var res2 = new XElement("GasLocationsPrices", from r in res1
                group r by r.Locality into grouped
                select new XElement("Info",
                    new XElement("Location", grouped.Key), 
                    new XElement("Price", grouped.Sum(el => el.Price)))
            );
            
            res2.Save("../../task_2.XML");
        }
        
        
        // 3. Отримати xml-файл, в якому вказати перелiк заправок у кожномуселищi-мiстi, 
        // якi отримали найбiльшу виручку сумарно за за усi днi.
        public static void Task3(List<GasStation> stations, List<Service> services, List<Data> data)
        {
            var res1 =  from d in data 
                join s in services on d.Type equals s.Type
                join ss in stations on d.Number equals ss.Number
                select new {Locality = ss.Locality, Number = ss.Number, Price = s.Price*d.Amount};
            
            var res2 =  from r in res1
                group r by new {r.Number, r.Locality} into grouped
                select new {Location = grouped.Key.Locality, 
                    Number = grouped.Key.Number,
                    Price = grouped.Sum(el => el.Price)};

            var res3 = new XElement("GasLocationsWinners", from r in res2
                group r by r.Location
                into grouped
                select new XElement("Info",
                    new XElement("Location",  grouped.Key),
                    new XElement("Winners" , from h in grouped.Where(el =>
                        el.Price == grouped.Max(k => k.Price)).
                            Select(el => el.Number)
                        select new XElement("Winner", h))
                ));
            res3.Save("../../task_3.XML");
        }
        
        public static void Main(string[] args)
        {
            var stations = new List<GasStation>();
            var services = new List<Service>();
            var data = new List<Data>();
            read_from_csv(stations,"../../stations.csv");
            read_from_csv(services,"../../services.csv");
            read_from_csv(data,"../../data.csv");

            write_toXML(data, "../../data.XML");
            try
            {
                data = read_fromXML<Data>("../../data.XML");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine($"{e.Message}");
                return;
            }

            Task1(stations, services, data);
            Task2(stations, services, data);
            Task3(stations, services, data);
        }
    }
}