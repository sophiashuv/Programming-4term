using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace module_01
{
    
    // Завдання 1.1. Розробити тип для платежiв за пальне на АЗС з атрибутами: марка пального,
    // цiна i кiлькiсть влiтрах. Утворити перелiк за допомогою масиву i заповнити його кiлькома 
    // об’єктами платежiв з рiзнимиданими. Вивести в стандартний потiк вмiстиме перелiку та марку 
    // палива з найбiльшим платежем.
    // 2. Розробити тип для розрахунку з власниками карток лояльностi АЗС, 
    // коли атрибути п.1 допов-нюються номером картки i при обчисленнi надається f% знижки. Ввести данi 
    // таких заправок з файлау перелiк (масив). Визначити номер картки, на який припав найбiльший платiж
    // i для кожної марки пального знайти сумарну вартiсть платежiв.
    // 3. Розробити тип для розрахунку за
    // заправку в режимi “до повного баку”, коли при обчисленнi надається h% знижки, причому без iнших 
    // варiантiв знижок. Ввести у перелiк (спiльний масив) данi заправок з трьох окремих файлiв. Для кожної 
    // марки пального знайти сумарну вартiсть платежiв.
    
    internal class Program
    {
        public static void read_from_txt<T, K>(List<K> container, string fileName) 
            where T : K, new()
            where K: ASC, new ()
        {
            // var listOfObjects = File.ReadLines( "/Users/sophiyca/RESTful_API_03/module_01/module_01/asc.csv" ).Skip(1).Select( line => new ASC(line) ).ToList();
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
                        propertyInfo.SetValue(asc, Convert.ChangeType(values[i], propertyInfo.PropertyType), null);
                    }
                    container.Add(asc);
                }
            }
        }
        
        public static void Task1(StreamWriter writer)
        {
            var ASCs = new List<ASC>();
            read_from_txt<ASC, ASC>(ASCs, "/Users/sophiyca/RESTful_API_03/module_01/module_01/asc.csv");
            
            writer.WriteLine("ALL ASCs: ");
            foreach (var w in ASCs)
                writer.WriteLine($"{w}");
            
            writer.WriteLine("MAX ASCs MARKS: ");
            // var maxASC = ASCs.Max();
            // var maxASCs = ASCs.FindAll(
            //     x => Math.Abs(x.AllPayment - maxASC.AllPayment) < 0.000000000001);
            //
            // foreach (var w in maxASCs)
            //     writer.WriteLine($"{w.Marka} - {w.AllPayment}");
            
            var totSum = new Dictionary<string, double>();
            foreach (var p in ASCs)
            {
                double value = 0;
                totSum.TryGetValue(p.Marka, out value);
                totSum[p.Marka] = p.AllPayment;
            }
            
            var keyOfMaxValue = totSum.Aggregate((x, y) => x.Value > y.Value ? x : y);
            foreach (var kvp in totSum)
                if (kvp.Value == keyOfMaxValue.Value)
                    writer.WriteLine($"\t{kvp.Key} - {kvp.Value}");
        }

        public static void Task2(StreamWriter writer)
        {
            Payment.Discount = 15;
            var payments = new List<Payment>();
            read_from_txt<Payment, Payment>(payments, "/Users/sophiyca/RESTful_API_03/module_01/module_01/peyment.csv");
            
            writer.WriteLine("ALL Payments: ");
            foreach (var p in payments)
                writer.WriteLine($"{p}");
            
            var totSum = new Dictionary<long, double>();
            foreach (var p in payments)
            {
                double value = 0;
                totSum.TryGetValue(p.CardNum, out value);
                totSum[p.CardNum] = p.AllPayment;
            }
            
            writer.WriteLine("\nMAX CARDS");
            var keyOfMaxValue = totSum.Aggregate((x, y) => x.Value > y.Value ? x : y);
            foreach (var kvp in totSum)
                if (kvp.Value == keyOfMaxValue.Value)
                    writer.WriteLine($"\t{kvp.Key} - {kvp.Value}");
            
            writer.WriteLine("\nAll Payments");
            foreach (var w in payments)
                writer.WriteLine($"\t{w.Marka} - {w.AllPayment}");
        }

        public static void Task3(StreamWriter writer)
        {
            ToFullBank.Discount = 40;
            var all = new List<ASC>();
            read_from_txt<ASC, ASC>(all, "/Users/sophiyca/RESTful_API_03/module_01/module_01/asc.csv");
            read_from_txt<Payment, ASC>(all, "/Users/sophiyca/RESTful_API_03/module_01/module_01/peyment.csv");
            read_from_txt<ToFullBank, ASC>(all, "/Users/sophiyca/RESTful_API_03/module_01/module_01/toFullBank.csv");
            
            writer.WriteLine("ALL Data: ");
            foreach (var p in all)
                writer.WriteLine($"{p}");
            
            writer.WriteLine("\nTotal Payment: ");
            var totSum = new Dictionary<string, double>();
            foreach (var p in all)
            {
                double value = 0;
                totSum.TryGetValue(p.Marka, out value);
                totSum[p.Marka] = p.AllPayment;
            }

            foreach (KeyValuePair<string, double> kvp in totSum)
                writer.WriteLine($"\tMark = { kvp.Key}, Payment = {kvp.Value}");
        }
        
        public static void Main(string[] args)
        {
            using (StreamWriter writer = new StreamWriter("/Users/sophiyca/RESTful_API_03/module_01/module_01/res.txt"))
            {
                writer.WriteLine("\n----------TASK1----------");
                Task1(writer);
                writer.WriteLine("\n----------TASK2----------");
                Task2(writer);
                writer.WriteLine("\n----------TASK3----------");
                Task3(writer);
            }
        }
    }
}