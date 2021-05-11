using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace module_09
{
    //  Текстовий файл Text.txt містить інформацію про книжкові магазини (КМ) міста,
    // кожен книжковий магазин містить назву і список книг. Розробити базовий клас Книга
    // (назва, ціна, кількість екземплярів), похідні класи Технічна і Художня книга, а також
    // клас Магазин (назва магазину і колекція книг). 

//     - Зчитати з файлу дані про кілька магазинів, вивести на консоль, впорядкувавши для кожного магазину список книг за назвою. 
//     - Серіалізувати у файл Data.XML в xml-форматі  інформацію про заданий магазин.
//     - Обчислити кількість книг художньої літератури по кожному магазину, інформацію зберегти в колекцію Dictionary.
//     - Обчислити сумарну вартість книжок по всіх магазинах.
//     - Задати назву книги і згенерувати відповідну подію, якщо у магазині є така книжка.  Обробник має виводити на консоль назву магазину, назву книжки і її ціну. 
//     - Передбачити обробку виключних ситуацій.

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
            string[] validFileExtensions = { ".txt", ".json"};
            if (!value.EndsWith(end))
            {
                throw new WrongFileFormatException($"Incorrect .{end} format.");
            }
            return value;
        }
    }

    [XmlInclude(typeof(TeсhnicalBook))]
    [XmlInclude(typeof(FictionBook))]
    [Serializable]
    public abstract class Book
    {
        private double price;
        public string Title { get; set; }
        
        public double Price
        {
            get => price;
            set => price = Validation.ValidatePrice(value); 
        }
        public uint Amount { get; set; }

        public override string ToString()
        {
            string res = "\t" + this.GetType().Name + "\n";
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this))
                res += ($"\t{prop.Name}: {prop.GetValue(this)}\n");
            return res;
        }
    }
    
    [Serializable]
    public class TeсhnicalBook : Book
    {
    }
    
    [Serializable]
    public class FictionBook : Book
    {
    }
    
    
    [Serializable]
    public class Shop
    {
        public static event EventHandler<EventArgs> myEvent;
        public string Title { get; set; }
        public List<Book> Books{get; set; }

        public static List<Shop> read_from_txt(string fileName)
        {
            var shops = new List<Shop>();
            using (var reader = new StreamReader(fileName))
            {
                while (!reader.EndOfStream)
                {
                    var shop = new Shop();
                    try {
                        var shopValues = reader.ReadLine().Split(':');
                        shop.Title = shopValues[0];
                        shop.Books = new List<Book>();
                        var books = shopValues[1].Split(',');
                        foreach (var book in books)
                        {
                            var bookAtters = book.Split(' ');
                            try
                            {
                                if (bookAtters[0] == "Teсhnical")
                                {
                                    shop.Books.Add(new TeсhnicalBook
                                    {
                                        Title = bookAtters[1],
                                        Price = double.Parse(bookAtters[2]),
                                        Amount = uint.Parse(bookAtters[3])
                                    });
                                }
                                else if (bookAtters[0] == "Fiction")
                                {
                                    shop.Books.Add(new FictionBook()
                                    {
                                        Title = bookAtters[1],
                                        Price = double.Parse(bookAtters[2]),
                                        Amount = uint.Parse(bookAtters[3])
                                    });
                                }
                            }
                            catch (ArgumentException e)
                            {
                                Console.WriteLine($"{e.Message}");
                            }
                        }
                        shops.Add(shop);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Wrong txt file format.");
                        continue;
                    }
                }
            }
            return shops;
        }
        
        public void FindBook(string bookName)
        {
            var find = Books.Find(i => i.Title == bookName && i.Amount != 0);
            if (find != null)
                myEvent?.Invoke(new {Shop = Title, BookName = bookName, Price = find.Price}, new EventArgs());
        }
        
        public override string ToString()
        {
            var books = String.Join("\n", Books);
            return$"{Title}:\n{books}";
        }
    }
    
    [Serializable]
    public class WrongFileFormatException : Exception
    {
        public WrongFileFormatException() { }

        public WrongFileFormatException(string message)
            : base(message) { }
    }
    
    internal class Program
    {
        //  - вивести на консоль, впорядкувавши для кожного магазину список книг за назвою. 
        public static void Task1(List<Shop> shops)
        {
            var q = shops.Select(el => new Shop{Title = el.Title, Books = el.Books.OrderBy(k => k.Title).ToList()});

            foreach (var shop in q)
            {
                Console.WriteLine(shop);
            }
        }
        
        //     - Серіалізувати у файл Data.XML в xml-форматі  інформацію про заданий магазин.
        public static void write_toXML(List<Shop> shops, string xf)
        {
            var xmldict = new XmlSerializer(typeof(List<Shop>));
            using (var file = new FileStream(xf, FileMode.OpenOrCreate))
            {
                xmldict.Serialize(file, shops);
            }
        }
        
        // - Обчислити кількість книг художньої літератури по кожному магазину, інформацію зберегти в колекцію
        // Dictionary.
        public static void Task3(List<Shop> shops)
        {
            var q = shops.ToDictionary(el => el.Title,
                el => el.Books.Count(b => b.GetType().Name == "FictionBook"));
            
            foreach (var kvp in q)
            {
                Console.WriteLine($"{kvp.Key}: {kvp.Value}");
            }
        }

        // - Обчислити сумарну вартість книжок по всіх магазинах.
        public static void Task4(List<Shop> shops)
        {
            var q = shops.ToDictionary(el => el.Title,
                el => el.Books.Aggregate(0.0, (sum, book) => sum + book.Amount * book.Price));
            
            foreach (var kvp in q)
            {
                Console.WriteLine($"{kvp.Key}: {kvp.Value}");
            }
        }
        
        // - Задати назву книги і згенерувати відповідну подію, якщо у магазині є така книжка.
        // Обробник має виводити на консоль назву магазину, назву книжки і її ціну. 
        static void helper(object ob, EventArgs args)
        {
            Console.WriteLine($"Book info: {(ob)}");
        }

        public static void Main(string[] args)
        {
            var filemame = "/Users/sophiyca/RiderProjects/module_09/module_09/Shops.txt";
            var xf = "/Users/sophiyca/RiderProjects/module_09/module_09/Data.XML";
            try
            {
                filemame = Validation.ValidateFile(filemame, "txt");
                xf = Validation.ValidateFile(xf, "XML");
            }
            catch (WrongFileFormatException e)
            {
                Console.WriteLine(e.Message);
                return;
            }
            
            var shops = Shop.read_from_txt(filemame);
            
            Console.WriteLine("Task1");
            Task1(shops);
            
            write_toXML(shops, xf);
            
            Console.WriteLine("Task3");
            Task3(shops);
            
            Console.WriteLine("Task4");
            Task4(shops);
            
            Console.WriteLine("Task5");
            Shop.myEvent += helper;
            foreach (var shop in shops)
            {
                shop.FindBook("TeсhnicalBook3");
            }
        }
    }
}