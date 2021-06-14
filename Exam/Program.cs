using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;


namespace Exam
{
    // Розробити типи для оформлення онлайн-замовлень на товари.
    //
    // Товар характеризуються цiлочисловим iдентифiкатором, назвою, кате-
    // горiєю, одиницею фасування та цiною одиницi васування.
    //
    // Замовники характеризуються iдентифiкацiйним номером, прiзвищем та
    // iм’ям, мiсцем проживання.
    //
    // Замовлення задано у форматi < iдентифiкатор замовника, iдентифiка-
    // тор товару, кiлькiсть в одиницях фасування >
    //
    // Усi данi задано окремими файлами.
    // ————————
    // Завдання виконати з використанням linq
    // ————————
    // 1. Отримати текстовий файл, де для кожного замовника ( у форматi
    // <прiзвище, iнiцiал iменi >, у лексико-графiчному порядку без повторень)
    // вказано перелiк його замовлень з такими даними < назва товару, кiлькiсть,
    // вартiсть>, впорядкований за вартiстю.
    // ————————
    //
    // 2. Отримати xml-файл, де для кожного замовника ( у форматi <прiзви-
    // ще, iм’я >, у лексико-графiчному порядку без повторень ) вказано перелiк
    //
    // його замовлень з такими даними < назва товару, кiлькiсть, вартiсть>, впо-
    // рядкований за категорiями.
    //
    // ————————
    // 3. Отримати з xml-файлу, записаного за умовою завдання 2, новий файл
    // з перелiком замовникiв, погрупованим за мiсцем проживання (назву якого
    // не повторювати, вказуючи для кожного мiсця загальну суму замовлень.
    internal class Program
    {
        public static void read_from_txt<T>(List<T> listContainer, string fileName) where T :  new()
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
        
        
        // 1. Отримати текстовий файл, де для кожного замовника ( у форматi
        // <прiзвище, iнiцiал iменi >, у лексико-графiчному порядку без повторень)
        // вказано перелiк його замовлень з такими даними < назва товару, кiлькiсть,
        // вартiсть>, впорядкований за вартiстю.
        public static void Task1(List<Item> items, List<Order> orders, List<Customer> customers)
        {
            var task1 = from i in items
                join o in orders on i.Id equals o.TovarId
                join c in customers on o.UserId equals c.Id
                orderby  c.Initials , o.Amount * i.Price
                select new
                {
                    Surname = c.Initials,
                    Name = i.Name,
                    Amount = o.Amount,
                    Price = o.Amount * i.Price * i.Unit
                };

            var result = from r in task1
                group r by r.Surname
                into grouped
                select new
                {
                    Surname = grouped.Key,
                    Tovars = from g in grouped
                        select new
                        {
                            Name = g.Name,
                            Amount = g.Amount,
                            Price = g.Price
                        }
                };


            using (StreamWriter writer = new StreamWriter("../../task1.txt"))
            {
                foreach (var v in result)
                {
                    writer.WriteLine(v.Surname);
                    foreach (var vv in v.Tovars)
                        writer.WriteLine($"\t{vv.Name},{vv.Amount},{vv.Price}");
                }
            }
        }
        
        
        // 2. Отримати xml-файл, де для кожного замовника ( у форматi <прiзви-
        // ще, iм’я >, у лексико-графiчному порядку без повторень ) вказано перелiк
        //
        // його замовлень з такими даними < назва товару, кiлькiсть, вартiсть>, впо-
        // рядкований за категорiями.
        public static void Task2(List<Item> items, List<Order> orders, List<Customer> customers)
        {
            var task2 = from i in items
                join o in orders on i.Id equals o.TovarId
                join c in customers on o.UserId equals c.Id
                orderby  i.Category
                select new
                {
                    Surname = c.Initials,
                    Name = i.Name,
                    Amount = o.Amount,
                    Price = o.Amount * i.Price,
                    Location = c.Location
                };

            var result = new XElement("Task2", from r in task2
                group r by r.Surname
                into grouped
                select new XElement("Customer", 
                    new XAttribute("Surname", grouped.Key), 
                    from g in grouped 
                    select new XElement("Tovar", 
                        new XElement("Name", g.Name),
                        new XElement("Amount", g.Amount),
                        new XElement("Price", g.Price),
                        new XElement("Location", g.Location)))
                
                );
            result.Save("../../task2.XML");
        }

        
        // 3. Отримати з xml-файлу, записаного за умовою завдання 2, новий файл
        // з перелiком замовникiв, погрупованим за мiсцем проживання (назву якого
        // не повторювати, вказуючи для кожного мiсця загальну суму замовлень.
        public static void Task3(List<Item> items, List<Order> orders, List<Customer> customers)
        {
            XDocument xdoc = XDocument.Load("../../task2.XML");
            var lv1s = from lv1 in xdoc.Descendants("Customer")
                from lv2 in lv1.Descendants("Tovar")
                        select new
                        {
                            Surname = lv1.Attribute("Surname").Value,
                            Name = lv2.Descendants("Name").First().Value,
                            Amount = lv2.Descendants("Amount").First().Value,
                            Price = Double.Parse(lv2.Descendants("Price").First().Value),
                            Location = lv2.Descendants("Location").First().Value
                            
                        };
            
            var result = new XElement("Task3", from r in lv1s
                group r by r.Location
                into grouped
                let totPrice = grouped.Sum(el => el.Price)
                select new XElement("Location", new XAttribute("Loc", grouped.Key),
                    new XElement("Customers",
                        new XAttribute("TotalSum", totPrice),
                            from i in grouped
                            group i by i.Surname
                            into gg
                            select new XElement("Customer",
                                new XAttribute("Surname", gg.Key), 
                                from VAR in gg 
                                select new XElement("Tovar", 
                                    new XElement("Name", VAR.Name),
                                    new XElement("Amount", VAR.Amount),
                                    new XElement("Price", VAR.Price))))));
            
            result.Save("../../task3.XML");
        }
        
        public static void Main(string[] args)
        {
            var items = new List<Item>();
            var orders = new List<Order>();
            var customers = new List<Customer>();
            read_from_txt(items, "../../items.txt");
            read_from_txt(orders, "../../orders.txt");
            read_from_txt(customers, "../../customers.txt");

            Task1(items, orders, customers);
            Task2(items, orders, customers);
            Task3(items, orders, customers);
        }
    }
}