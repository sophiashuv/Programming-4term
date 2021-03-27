using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Library_Sequences
{
    internal class Program
    {
        // Завдання 1.Розробити типи для обслуговування читачiв у бiблiотечному абонементi. 
        //     Читач характеризується iменем, прiзвищем та реєстрацiйним номером.Книга 
        //     характеризується облiковим номером, автором книги, назвою книгиi жанром. 
        //     Замовлення характеризується датою, облiковим номером книги,реєстрацiйним номером 
        //     читача та ознакою виконання(так-нi). Iнформацiяпро читачiв та книги подана окремими 
        //     файлами. Замовлення також поданокiлькома окремими файлами.Отримати:
        // 1. для кожного читача ( за iменем i прiзвищем) повний перелiк авторiв(без повторень) 
        //     замовлених книг у форматi< автор - кiлькiсть замовлених книг >;
        // 2. рейтинг читачiв за загальною кiлькiстю замовлених книг у форматi< прiзвище та iм’я читача - кiлькiсть 
        //     замовлених книг >;
        // 3. для кожної книги кiлькiсть замовлень у форматi< автор - назва - кiлькiсть замовлень>;
        // 4. для кожного дня кiлькiсть виконаних замовлень та вiдмов.
        
        public static void read_from_txt<T>(List<T> container, string fileName) 
            where T : new() 
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
                        propertyInfo.SetValue(asc, Convert.ChangeType(values[i], propertyInfo.PropertyType), null);
                    }
                    container.Add(asc);
                }
            }
        }

        // 1. для кожного читача ( за iменем i прiзвищем) повний перелiк авторiв(без повторень) 
        // замовлених книг у форматi< автор - кiлькiсть замовлених книг >;
        public static void Task1(Dictionary<uint, Reader> reader_dict, Dictionary<uint, Book> book_dict,
            List<Order> orders)
        {
            var totSum = new Dictionary<string, Dictionary<string, int>>();
            foreach (var order in orders)
            {
                if (totSum.ContainsKey(reader_dict[order.ReaderId].FullName))
                {
                    if (totSum[reader_dict[order.ReaderId].FullName].ContainsKey(book_dict[order.BookId].Author))
                        totSum[reader_dict[order.ReaderId].FullName][book_dict[order.BookId].Author] += 1;
                    else
                        totSum[reader_dict[order.ReaderId].FullName].Add(book_dict[order.BookId].Author, 1);
                }
                else
                {
                    var d = new Dictionary<string, int>();
                    d.Add(book_dict[order.BookId].Author, 1);
                    totSum.Add(reader_dict[order.ReaderId].FullName, d);
                }
            }

            foreach (var kvp in totSum)
            {
                Console.WriteLine($"{kvp.Key}: ");
                foreach (var kvpp in totSum[kvp.Key])
                    Console.WriteLine($"\t{kvpp.Key} - {kvpp.Value}");
            }
        }

        // 2. рейтинг читачiв за загальною кiлькiстю замовлених книг у форматi< прiзвище та iм’я читача - кiлькiсть 
        //     замовлених книг >;
        public static void Task2(Dictionary<uint, Reader> reader_dict, Dictionary<uint, Book> book_dict,
            List<Order> orders)
        {
            var totSum = new Dictionary<string, int>();
            foreach (var order in orders)
            {
                if (totSum.ContainsKey(reader_dict[order.ReaderId].FullName))
                    totSum[reader_dict[order.ReaderId].FullName] += 1;
                else
                    totSum.Add(reader_dict[order.ReaderId].FullName, 1);
            }
            totSum = totSum.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            foreach (var kvp in totSum)
                Console.WriteLine($"{kvp.Key}: {kvp.Value}");
        }

        
        // 3. для кожної книги кiлькiсть замовлень у форматi< автор - назва - кiлькiсть замовлень>;
        public static void Task3(Dictionary<uint, Reader> reader_dict, Dictionary<uint, Book> book_dict,
            List<Order> orders)
        {
            var totSum = new Dictionary<string, Dictionary<string, int>>();
            foreach (var order in orders)
            {
                if (totSum.ContainsKey(book_dict[order.BookId].Author))
                {
                    if (totSum[book_dict[order.BookId].Author].ContainsKey(book_dict[order.BookId].Name))
                        totSum[book_dict[order.BookId].Author][book_dict[order.BookId].Name] += 1;
                    else
                        totSum[book_dict[order.BookId].Author].Add(book_dict[order.BookId].Name, 1);
                }
                else
                {
                    var d = new Dictionary<string, int>();
                    d.Add(book_dict[order.BookId].Name, 1);
                    totSum.Add(book_dict[order.BookId].Author, d);
                }
            }

            foreach (var kvp in totSum)
            {
                Console.WriteLine($"{kvp.Key}: ");
                foreach (var kvpp in totSum[kvp.Key])
                    Console.WriteLine($"\t{kvpp.Key} - {kvpp.Value}");
            }
        }

        // 4. для кожного дня кiлькiсть виконаних замовлень та вiдмов.
        public static void Task4(Dictionary<uint, Reader> reader_dict, Dictionary<uint, Book> book_dict,
            List<Order> orders)
        {
            var totSum = new Dictionary<DateTime, Dictionary<string, int>>();
            foreach (var order in orders)
            {
                if (totSum.ContainsKey(order.Date))
                {
                    totSum[order.Date]["yes"] += Convert.ToInt32(order.Done);
                    totSum[order.Date]["no"] += Convert.ToInt32(!order.Done);
                }
                else
                {
                    var d = new Dictionary<string, int>()
                    {
                        {"yes", Convert.ToInt32(order.Done)}, 
                        {"no",  Convert.ToInt32(!order.Done)}
                    };
                    totSum.Add(order.Date, d);
                }
            }
            
            foreach (var kvp in totSum)
            {
                Console.WriteLine($"{kvp.Key.ToShortDateString()}: ");
                foreach (var kvpp in totSum[kvp.Key])
                    Console.WriteLine($"\t{kvpp.Key} - {kvpp.Value}");
            }
        }

        public static void Main(string[] args)
        {
            var readers = new List<Reader>();
            read_from_txt<Reader>(readers,
                "/Users/sophiyca/RiderProjects/Library_Sequences/Library_Sequences/readers.csv");
            var books = new List<Book>();
            read_from_txt<Book>(books,
                "/Users/sophiyca/RiderProjects/Library_Sequences/Library_Sequences/books.csv");
            var orders = new List<Order>();
            read_from_txt<Order>(orders,
                "/Users/sophiyca/RiderProjects/Library_Sequences/Library_Sequences/orders.csv");
            
            
            var reader_dict = new Dictionary<uint, Reader>();
            foreach (var reader in readers) 
                reader_dict.Add(reader.Id, reader);
            
            var books_dict = new Dictionary<uint, Book>();
            foreach (var book in books) 
                books_dict.Add(book.Id, book);

            readers.Sort();
            foreach (var v in readers)
            {
                Console.WriteLine(v);
            }
            
            Console.WriteLine("---------------------------------------------");
            Task1(reader_dict, books_dict, orders);
            Console.WriteLine("---------------------------------------------");
            Task2(reader_dict, books_dict, orders);
            Console.WriteLine("---------------------------------------------");
            Task3(reader_dict, books_dict, orders);
            Console.WriteLine("---------------------------------------------");
            Task4(reader_dict, books_dict, orders);

        }
    }
}