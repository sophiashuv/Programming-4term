using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace LINQ_6
{
    // Дано дві послідовності А і С, що містять наступні поля:
    // А-<Код споживача><Прізвище><Вулиця проживання> 
    // С-<Код споживача><Знижка (%)><Назва магазину>
    //
    // -Для кожного магазину вивести дані про споживача, що має у цьому магазині максимальну знижку, 
    // якщо таких осіб є декілька, вивести споживача з найменшим кодом. Інформацію виводити у
    // вигляді:назва магазину, код споживача, прізвище, знижка, впорядковано за назвою магазину 
    // (у алфавітному порядку).
    //
    // -Для кожного споживача, вказаного в А, обчислити кількістьмагазинів, у яких він має знижку, 
    // якщо таких нема, вивести 0. Інформацію вивести у вигляді:кількість магазинів, код споживача, 
    // прізвище споживача, впорядковано за спаданням кількості магазинів, а якщо кількість однакова, 
    // за зростанням коду споживача.
    //
    // -Для кожного магазину і кожної вулиці обчислити кількість споживачів, 
    // які проживають на цій вулиці і мають знижку в цьому магазині. Якщо для якоїсь пари 
    // магазин-вулиця споживачів не знайдено, дані про неї не виводити. Інформацію вивести 
    // у вигляді: назва магазину, назва вулиці, кількість споживачів впорядковано за 
    // назвами магазинів в алфавітному порядку, а для однакових магазинів –за назвою вулиці.
    
    
    
    internal class Program
    {
        // -Для кожного магазину вивести дані про споживача, що має у цьому магазині максимальну знижку, 
        // якщо таких осіб є декілька, вивести споживача з найменшим кодом. Інформацію виводити у
        // вигляді:назва магазину, код споживача, прізвище, знижка, впорядковано за назвою магазину 
        // (у алфавітному порядку).
        
        public static void Task1(List<A> a, List<C> c)
        {
            var query =
                a.Join(c,
                    aa => aa.Code,
                    cc => cc.Code,
                    (aa, cc) => new {A = aa, C = cc}).GroupBy(el => el.C.Shop).
                    ToDictionary(el => el.Key, el =>
                        el.Where(k => k.C.Discount == el.Max(s => s.C.Discount))).
                    OrderBy(el => el.Key);

            foreach (var kvp in query)
            {
                var r = kvp.Value.First(el => el.C.Code == kvp.Value.Min(k => k.C.Code)).A;
                Console.WriteLine($"{kvp.Key}: \n{r}");
            }
        }

        // -Для кожного споживача, вказаного в А, обчислити кількістьмагазинів, у яких він має знижку, 
        // якщо таких нема, вивести 0. Інформацію вивести у вигляді:кількість магазинів, код споживача, 
        // прізвище споживача, впорядковано за спаданням кількості магазинів, а якщо кількість однакова, 
        // за зростанням коду споживача.
        public static void Task2(List<A> a, List<C> c)
        {
            var query =
                a.GroupJoin(c.Where(el => el.Discount > 0),
                        aa => aa.Code,
                        cc => cc.Code,
                        (aa, cc) => new {Code = aa.Code, Surname = aa.Surname, C = cc}).
                    ToDictionary(el => new {el.Code, el.Surname}, el => el.C.Count()).OrderByDescending(el => el.Value)
                    .ThenBy(el => el.Key.Code);
            
            foreach (var kvp in query)
            {
                Console.WriteLine($"{kvp.Value}: {kvp.Key.Code}  {kvp.Key.Surname}");
            }
        }
        
        // -Для кожного магазину і кожної вулиці обчислити кількість споживачів, 
        // які проживають на цій вулиці і мають знижку в цьому магазині. Якщо для якоїсь пари 
        // магазин-вулиця споживачів не знайдено, дані про неї не виводити. Інформацію вивести 
        // у вигляді: назва магазину, назва вулиці, кількість споживачів впорядковано за 
        // назвами магазинів в алфавітному порядку, а для однакових магазинів –за назвою вулиці.
        public static void Task3(List<A> a, List<C> c)
        {
            var query =
                c.GroupBy(el => el.Shop).ToDictionary(el => el.Key, el => a.Join(el,
                    
                    aa => aa.Code,
                    cc => cc.Code,
                    (aa, cc) => new
                    {
                        A = aa,
                        C = cc
                    }).GroupBy(s => s.A.Street).
                        ToDictionary(s => s.Key, s => s.Count()).
                        OrderBy(s => s.Key)).
                    OrderBy(el => el.Key);

            foreach (var kvp in query)
            {
                Console.WriteLine($"{kvp.Key}:");
                foreach (var kvpp in kvp.Value)
                {
                    Console.WriteLine($"\t{kvpp.Key} - {kvpp.Value}");
                }
            }

        }
        
        public static void Main(string[] args)
        {
            var a = new List<A>
            {
                new A {Code = 123, Surname = "Surname1", Street = "Street1"},
                new A {Code = 456, Surname = "Surname3", Street = "Street2"},
                new A {Code = 234, Surname = "Surname7", Street = "Street1"},
                new A {Code = 789, Surname = "Surname2", Street = "Street1"},
                new A {Code = 345, Surname = "Surname4", Street = "Street2"},
                new A {Code = 167, Surname = "Surname4", Street = "Street3"}
            };

            var c = new List<C>
            {
                new C{Code = 123, Discount = 45, Shop = "Shop1"},
                new C{Code = 345, Discount = 69, Shop = "Shop1"},
                new C{Code = 123, Discount = 45, Shop = "Shop1"},
                new C{Code = 789, Discount = 90, Shop = "Shop2"},
                new C{Code = 234, Discount = 50, Shop = "Shop3"},
                new C{Code = 123, Discount = 45, Shop = "Shop1"},
                new C{Code = 345, Discount = 23, Shop = "Shop2"},
                new C{Code = 234, Discount = 75, Shop = "Shop1"},
                new C{Code = 789, Discount = 70, Shop = "Shop3"},
                new C{Code = 456, Discount = 70, Shop = "Shop3"}
            };
            Task1(a, c);
            Task2(a, c);
            Task3(a, c);
        }
    }
}