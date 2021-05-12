using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace module_10
{
    // Текстовий файл Text.txt містить інформацію про ВНЗ міста, кожен ВНЗ містить список факультетів. 
    // Розробити базовий клас Факультет (назва, кількість студентів, спеціальність) та похідні класи 
    // Стаціонар і Заочний, а також клас ВНЗ (назва, рік заснування та колекція факультетів). 
    //   - Зчитати з файлу дані про кілька вишів, вивести на консоль, впорядкувавши для кожного 
    //     ВНЗ список факультетів за назвою. 
    //   - Серіалізувати у файл Data.xml в xml-форматі інформацію  про найстаріший ВНЗ.
    //   - Обчислити загальну кількість студентів стаціонару у кожному ВНЗ та інформацію зберегти в 
    //     колекцію Dictionary.
    //   - Вивести інформацію про всі ВНЗ, які готують випускників заданої спеціальності у форматі 
    //     назва ВНЗ – рік заснування. 
    //   - Задати назву факультету, та згенерувати відповідну подію, якщо у ВНЗ є такий факультет. 
    //     Обробник події виводить на консоль назву ВНЗ, рік заснування і назву факультету. 
    //   - Передбачити обробку виключних ситуацій.

     public static class Validation
    {
        public static uint ValidateYear(uint value)
        {
            Regex regex = new Regex(@"\d{4}");
            Match match = regex.Match(value.ToString());
            if (!match.Success)
                throw new ArgumentException("Not a valid Year");
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

    [XmlInclude(typeof(Staz))]
    [XmlInclude(typeof(Zao))]
    [Serializable]
    public abstract class Faculty
    {
        public string Title { get; set; }
        
        public uint StudentsAmount { get; set; }
        
        public String Specialty{ get; set; }

        public override string ToString()
        {
            string res = "\t" + this.GetType().Name + "\n";
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this))
                res += ($"\t{prop.Name}: {prop.GetValue(this)}\n");
            return res;
        }
    }
    
    [Serializable]
    public class Staz : Faculty
    {
    }
    
    [Serializable]
    public class Zao : Faculty
    {
    }
    
    //(назва, рік заснування та колекція факультетів)
    [Serializable]
    public class University
    {
        private uint _year;
        public static event EventHandler<EventArgs> MyEvent;
        public string Title { get; set; }
        public uint Year
        {
            get => _year;
            set => _year = Validation.ValidateYear(value); 
        }
        public List<Faculty> Faculties{get; set; }

        public static List<University> read_from_txt(string fileName)
        {
            var universities = new List<University>();
            using (var reader = new StreamReader(fileName))
            {
                while (!reader.EndOfStream)
                {
                    var university = new University();
                    try {
                        var uniValues = reader.ReadLine()?.Split(':');
                        var firstUniValues = uniValues?[0].Split(' ');
                        university.Title = firstUniValues?[0];
                        university._year = uint.Parse(firstUniValues?[1] ?? string.Empty);
                        university.Faculties = new List<Faculty>();
                        var faculties = uniValues?[1].Split(',');
                        if (faculties != null)
                            foreach (var faculty in faculties)
                            {
                                var facultyValues = faculty.Split(' ');
                                try
                                {
                                    if (facultyValues[0] == "Staz")
                                    {
                                        university.Faculties.Add(new Staz
                                        {
                                            Title = facultyValues[1],
                                            StudentsAmount = uint.Parse(facultyValues[2]),
                                            Specialty = facultyValues[3]
                                        });
                                    }
                                    else if (facultyValues[0] == "Zao")
                                    {
                                        university.Faculties.Add(new Zao
                                        {
                                            Title = facultyValues[1],
                                            StudentsAmount = uint.Parse(facultyValues[2]),
                                            Specialty = facultyValues[3]
                                        });
                                    }
                                }
                                catch (ArgumentException e)
                                {
                                    Console.WriteLine($"{e.Message}");
                                }
                            }

                        universities.Add(university);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Wrong txt file format. {e.Message}");
                    }
                }
            }
            return universities;
        }
        
        public void FindBook(string faculty)
        {
            var find = Faculties.Find(i => i.Title == faculty);
            if (find != null)
                MyEvent?.Invoke(new {University = Title, Year, Faculty = faculty}, new EventArgs());
        }
        
        public override string ToString()
        {
            var fuculties = String.Join("\n", Faculties);
            return$"{Title} {Year}:\n{fuculties}";
        }
    }
    
    internal class Program
    {
        //  - вивести на консоль, впорядкувавши для кожного ВНЗ список факультетів за назвою. 
        public static void Task1(List<University> universities)
        {
            var q = universities.Select(el => new University{
                Title = el.Title, 
                Year = el.Year, 
                Faculties = el.Faculties.OrderBy(k => k.Title).ToList()
            });

            foreach (var uni in q)
                Console.WriteLine(uni);
        }
        
        // - Серіалізувати у файл Data.xml в xml-форматі інформацію  про найстаріший ВНЗ.
        public static void Task2(List<University> universities, string xf)
        {
            var oldestUniversity = universities.Where(el => el.Year == universities.Min(k => k.Year)).ToList();
            var xmldict = new XmlSerializer(typeof(List<University>));
            using (var file = new FileStream(xf, FileMode.OpenOrCreate))
            {
                xmldict.Serialize(file, oldestUniversity);
            }
        }
        
        //- Обчислити загальну кількість студентів стаціонару у кожному ВНЗ та інформацію зберегти в 
        // колекцію Dictionary.
        public static void Task3(List<University> universities)
        {
            var quvery = universities.ToDictionary(el => el.Title,
                el => el.Faculties.
                    Where(k => k.GetType().Name == "Staz").
                    Sum(k => k.StudentsAmount));
            
            foreach (var kvp in quvery)
                Console.WriteLine($"{kvp.Key}: {kvp.Value}");
        }

        //    - Вивести інформацію про всі ВНЗ, які готують випускників заданої спеціальності у форматі 
        //     назва ВНЗ – рік заснування. 
        public static void Task4(List<University> universities)
        {
            var spez = "Specialty3";
            
            var query = universities.FindAll(el => el.Faculties.
                    Any(k => k.Specialty == spez)).
                ToDictionary(el => el.Title, el => el.Year);
            
            foreach (var kvp in query)
                Console.WriteLine($"{kvp.Key}: {kvp.Value}");
        }
        
        //   - Задати назву факультету, та згенерувати відповідну подію, якщо у ВНЗ є такий факультет. 
        //     Обробник події виводить на консоль назву ВНЗ, рік заснування і назву факультету. 
        public static void Task5(object ob, EventArgs args)
        {
            Console.WriteLine($"Faculty info: {(ob)}");
        }
        
        public static void Main(string[] args)
        {
            var filemame = "../../universities.txt";
            var xf = "../../Data.XML";
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
            
            var universities = University.read_from_txt(filemame);
            
            Console.WriteLine("Task1");
            Task1(universities);
            
            Console.WriteLine("Task2");
            Task2(universities, xf);
            
            Console.WriteLine("Task3");
            Task3(universities);
            
            Console.WriteLine("Task4");
            Task4(universities);
            
            Console.WriteLine("Task5");
            University.MyEvent += Task5;
            foreach (var uni in universities)
                uni.FindBook("Faculty3");
        }
    }
}