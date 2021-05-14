using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace module_12
{
    // Визначити клас Адреса (місто, вулиця, будинок) і клас Замовлення (прізвище замовника, адреса, 
    // назва товару, вартість замовлення).  Зчитати дані з текстових файлів.
    //
    // Серіалізувати у файл колекцію всіх замовлень заданого товару, впорядкувавши за прізвищем замовника.
    //
    // Використовуючи LINQ, отримати кількість замовлень кожного товару в кожному місті і вивести у вигляді 
    // місто, товар, кількість замовлень.
    //
    // Знайти замовника, в якого найбільша сумарна вартість замовлень, а якщо таких кілька, вивести першого 
    // за алфавітом.
    //
    // Якщо змінюється адреса замовлення, генерується подія, в обробнику якої на консоль виводиться інформація 
    // про товар та нову адресу. Змоделювати зміну адреси кількох замовлень.
    //
    // Передбачити обробку виключних ситуацій.
    
    public static class Validation
    {

        public static double ValidatePrice(double value)
        {
            string strValue = value.ToString(CultureInfo.InvariantCulture).
                IndexOf(".", StringComparison.Ordinal) == -1 ? value.ToString(CultureInfo.InvariantCulture) 
                                                               + "." : value.ToString(CultureInfo.InvariantCulture);
            if (strValue.Substring(strValue.IndexOf(".", StringComparison.Ordinal)).Length > 3)
            {
                throw new ArgumentException("Price must have two digits after coma.");
            }
            return value;
        }
        
        public static string ValidateFile(string value, string end)
        {
            if (!value.EndsWith(end))
                throw new WrongFileFormatException($"Incorrect .{end} format.");
            return value;
        }
    }
    
    [Serializable]
    public class WrongFileFormatException : Exception
    {
        public WrongFileFormatException() { }

        public WrongFileFormatException(string message)
            : base(message) { }
    }
    
    [Serializable]
    public class Adress
    {
        public string City { get; set; }
        public string Street { get; set; }
        public int House { get; set; }
        
        public Adress(){}
        public Adress(string c, string s, int h)
        {
            City = c;
            Street = s;
            House = h;
        }
        public override string ToString()
        {
            string res = "";
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this))
                res += ($"{prop.GetValue(this)} ");
            return res;
        }

    }
    
    [Serializable]
    [XmlInclude(typeof(Adress))]
    public class Order
    {
        private double price;
        public static event EventHandler<EventArgs> MyEvent;
        public string Who_orders { get; set; }
        public Adress OAdress { get; set; }
        public string Tovar { get; set; }
        public double Price
        {
            get => price;
            set => price = Validation.ValidatePrice(value); 
        }
        public Order(){}
        public Order(string w, Adress a, string t, double p)
        {
            Who_orders = w;
            OAdress = a;
            Tovar = t;
            Price = p;
        }
        
        public override string ToString()
        {
            string res = "";
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this))
                res += ($"\t{prop.Name}: {prop.GetValue(this)}\n");
            return res;
        }
        //інформація 
        // про товар та нову адресу
        public void ChangeAdress(Adress newAdress)
        {
            OAdress = newAdress;
            MyEvent?.Invoke(new {Who_orders, Tovar, Price, NewAdress = newAdress}, new EventArgs());
        }
    }
    
    internal class Program
    {
        public static void read_from_txt(List<Order> container, string fileName)
        {
            using (var reader = new StreamReader(fileName))
            {
                var atters = reader.ReadLine().Split(',');
                while (!reader.EndOfStream)
                {
                    try
                    {
                        var values = reader.ReadLine().Split(',');
                        var asc = new Order(values[0], new Adress(values[1], values[2],
                            Convert.ToInt32(values[3])), values[4], Convert.ToDouble(values[5]));
                        container.Add(asc);
                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine($"{e.Message}");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Wromg file format");
                    }
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
        
        // Серіалізувати у файл колекцію всіх замовлень заданого товару, впорядкувавши за прізвищем замовника.
        public static void Task1( List<Order> orders, string xf)
        {
            var tovar = "Toy";

            var res = orders.Where(el => el.Tovar == tovar).OrderBy(el => el.Who_orders).ToList();
            write_toXML(res, xf);
        }

        // Використовуючи LINQ, отримати кількість замовлень кожного товару в кожному місті і вивести у вигляді 
        // місто, товар, кількість замовлень.
        public static void Task2(List<Order> orders)
        {
            var res = orders.GroupBy(el => el.OAdress.City).ToDictionary(el => el.Key,
                el => el.GroupBy(k => k.Tovar).ToDictionary(k => k.Key, 
                    k => k.Count()));

            foreach (var v in res)
            {
                Console.WriteLine($"{v.Key}: ");
                foreach (var vv in v.Value)
                    Console.WriteLine($"\t{vv.Key} - {vv.Value}");
            }
        }
        
        // Знайти замовника, в якого найбільша сумарна вартість замовлень, а якщо таких кілька, вивести першого 
        // за алфавітом.
        public static void Task3(List<Order> orders)
        {
            var res = orders.GroupBy(el => el.Who_orders).
                Select(el => new {WhoOrders = el.Key, TotalPrice = el.Sum(k => k.Price)}).
                OrderBy(el => el.WhoOrders);
            
            var maxP = res.Max(el => el.TotalPrice);
            var finalRes = res.First(el => el.TotalPrice == maxP);
            Console.WriteLine($"{finalRes.WhoOrders} - {finalRes.TotalPrice}");
        }
        
        // Якщо змінюється адреса замовлення, генерується подія, в обробнику якої на консоль виводиться інформація 
        // про товар та нову адресу. Змоделювати зміну адреси кількох замовлень.
        public static void Task4(object ob, EventArgs args)
        {
            Console.WriteLine($"Tovar info: {(ob)}");
        }
        
        public static void Main(string[] args)
        {
            // var tf = "../../orders.txfft";
            var tf = "../../orders.txt";
            string xf = "../../task1.XML";
            
            try
            {
                tf = Validation.ValidateFile(tf, "txt");
                xf = Validation.ValidateFile(xf, "XML");
            }
            catch (WrongFileFormatException e)
            {
                Console.WriteLine(e.Message);
                return;
            }
            
            var orders = new List<Order>();
            read_from_txt(orders, tf);
            Console.WriteLine("\nAll Orders: ");
            foreach (var v in orders)
                Console.WriteLine(v);

            Console.WriteLine("\nTask1");
            Task1(orders, xf);
            Console.WriteLine("\nTask2");
            Task2(orders);
            Console.WriteLine("\nTask3");
            Task3(orders);
            
            Console.WriteLine("\nTask4");
            Order.MyEvent += Task4;
            orders[1].ChangeAdress(new Adress("City", "Totally new Address", 45));
            orders[2].ChangeAdress(new Adress("City2", "Totally new Address2", 45));
            
        }
    }
}