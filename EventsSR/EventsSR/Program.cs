using System;
using System.IO;

namespace EventsSR
{
    // Зчитати з текстового файлу дані в колекцію об’єктів та обробити 
    // події, які при цьому генеруються. Для події передбачити два обробники,
    // які по різному будуть її обробляти, наприклад, один виводить повідомлення
    // на консоль, інший записує повідомлення у файл.
    // 4. Написати клас СТУДЕНТ, що містить наступні поля: прізвище, група, оцінки 
    // за останню сесію, а також подію, що генерується тоді, якщо сесія не здана 
    // (є хоча б одна двійка).


    internal class Program
    {
        static void helper1(object ob, EventArgs args)
        {
            string Name = "/Users/sophiyca/RiderProjects/EventsSR/EventsSR/result.txt";
            StreamWriter streamWriter = new StreamWriter(Name, true);
            streamWriter.WriteLine($"Session Failed: {(ob as Student)}");
            streamWriter.Close();
        }
        
        static void helper2(object ob, EventArgs args)
        {
            Console.WriteLine($"Session Failed: {(ob as Student)}");
        }
        
        public static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            var collection = new Collection();
            collection.ReadTxt("/Users/sophiyca/RiderProjects/EventsSR/EventsSR/students.txt");
            Console.WriteLine(collection);
            
            collection.myEvent += helper1;
            collection.myEvent += helper2;
            collection.Failed();
        }
    }
}