using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace module08_shuvar
{
    public interface IColor
    {
        string Color { get; set; }
    }
    
    public interface IShape: IComparable<IShape>
    {
        double Perimeter();
        int CompareTo(IShape other);

    }

    public struct ColoredSide : IColor
    {
        public ColoredSide(string str, double l)
        {
            Color = str;
            Length = l;
        }
        public string Color { get; set; }
        public double Length { get; set; }
        public override string ToString() => Color + " " + Length.ToString();
    }

    public class Rectangle : IShape
    {
        public Rectangle(double a, double b)
        {
            A = a;
            B = b;
        }
        
        public Rectangle()
        {
        }
        
        public int CompareTo(IShape other) => Perimeter().CompareTo(other.Perimeter());
        public double A { get; set; }
        public double B { get; set; }
        public double Perimeter() => (A + B) * 2;
        
        public override string ToString()
        {
            string res = this.GetType().Name + "\n";
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this))
                res += ($"\t{prop.Name}: {prop.GetValue(this)}\n");
            return res;
        }
    }
    
    public class ColoredTriangle : IShape
    {
        public ColoredSide A { get; set; }
        public ColoredSide B { get; set; }
        public ColoredSide C { get; set; }
        public ColoredTriangle(string a, double a_l, string b, double b_l, string c, double c_l)
        {
            A = new ColoredSide(a, a_l);
            B = new ColoredSide(b, b_l);
            C = new ColoredSide(c, c_l);
        }
        public string getSameColors() => A.Color == B.Color&& B.Color == C.Color ? A.Color : null;

        public ColoredTriangle()
        {
        }
        public double Perimeter() => A.Length + B.Length + C.Length;
        
        public int CompareTo(IShape other) => Perimeter().CompareTo(other.Perimeter());

        public override string ToString()
        {
            string res = this.GetType().Name + "\n";
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this))
                res += ($"\t{prop.Name}: {prop.GetValue(this)}\n");
            return res;
        }
    }
    
    public class Collection: IEnumerable
    {
        public Collection() => Lst = new List<IShape>();
        public List<IShape> Lst { get; set; }
        
        public override string ToString()
        {
            var res = "";
            foreach (var r in Lst)
                res += "\t" + r.ToString() + "\n";
            return res;
        }
        
        public static bool IsTriangle(IShape r) => r.GetType().Name == typeof(ColoredTriangle).Name;
        public IEnumerator<IShape> GetEnumerator() => Lst.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        
        public void SortByPerimeter() => Lst.OrderBy(x => x.Perimeter());
        
        public void read_from_txt(string fileName)
        {
            string[] s = File.ReadAllLines(fileName);
            for(int i = 0; i < s.Length; i++)
            {
                string[] s1 = s[i].Split(' ');
                if(s1[0] == "T")
                {
                    ColoredTriangle t = new ColoredTriangle(s1[1], Convert.ToDouble(s1[2]), s1[3], Convert.ToDouble(s1[4]), s1[5], Convert.ToDouble(s1[6]));
                    Lst.Add(t);
                }
                if (s1[0] == "R")
                {
                    Rectangle r = new Rectangle(Convert.ToDouble(s1[1]), Convert.ToDouble(s1[2]));
                    Lst.Add(r);
                }
            }
        }
        
    }
    
    internal class Program
    {
    // 1.В заданій квадратній дійсній матриці A розміру n обчислити суму додатніх елементів, 
    // які розташовані вище головної діагоналі включноз нею.
    // 2.Визначити інтерфейс IShape, 
    // який містить метод обчислення периметру, та інтерфейс IColor, який містить властивість 
    // доступу до кольору. Визначити структуру ColoredSide, яка представляє кольорову сторону, 
    // реалізує IColor і містить довжину. Визначити клас ColoredTriangle, який реалізує IShape 
    // і має три кольорові сторони та клас Rectangle, який реалізує IShapeі має довжину і
    // висоту. В текстовому файлі міститься інформація про кольорові трикутники та прямокутники. 
    // Зчитати дані в одну колекцію , вивести фігури, відсортовані за периметром. Визначити ті
    // трикутники, в яких всі сторони одного кольору,і записати в колекцію Dictionary пари 
    // вигляду колір –кількість трикутників цього кольору.
    
        public static void Task1()
        {
            Console.WriteLine("Enter n: ");
            int n = int.Parse(Console.ReadLine() ?? string.Empty);
            double[,] matrix = new double[n, n];
            Console.WriteLine("Enter your matrix: ");
            
            for (var i = 0; i < n; i++)
            {
                var line = Console.ReadLine();
                var spl = line.Split(' ');
                if (spl.Length != n) throw new FormatException();
                for (var j = 0; j < n; j++)
                    matrix[i, j] = int.Parse(spl[j]); ;
            }

            double sum = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = i; j < n; j++)
                    sum += matrix[i, j] > 0 ? matrix[i, j] : 0;
            }
            Console.Write($"Sum of all elements higher than diagon: {sum}");
        }

        public static void Task2()
        {
            var collection = new Collection();
            var file_name = "/Users/sophiyca/RiderProjects/module08_shuvar/module08_shuvar/data.txt";
            collection.read_from_txt(file_name);
            Console.WriteLine("Collection Sorted by Perimeter: ");
            collection.SortByPerimeter();
            Console.WriteLine(collection);
            
            Console.WriteLine("Triangles that hav three same colored sides: ");
            var triangleDict = new Dictionary<string, Collection>();
            var triangles = collection.Lst.FindAll(e => Collection.IsTriangle(e));
            foreach (var k in triangles)
            {
                var t = (ColoredTriangle) k;
                var color = t.getSameColors();
                if (color != null) {
                    if (triangleDict.ContainsKey(color))
                        triangleDict[color].Lst.Add(k);
                    else
                    {
                        var key = new Collection();
                        triangleDict.Add(color, key);
                        triangleDict[color].Lst.Add(k);
                    }
                }
            }
            foreach (var kvp in triangleDict)
                Console.WriteLine($"{kvp.Key}: \n{kvp.Value}");
        }

        public static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine(" \nEnter number of task (1, 2) to run the task or another number to exit: ");
                int task = int.Parse(Console.ReadLine() ?? string.Empty);
                ;
                switch (task)
                {
                    case 1:
                        Task1();
                        break;
                    case 2:
                        Task2();
                        break;
                    default:
                        Console.WriteLine("GOOD BYE!");
                        return;
                }
            }
        }
    }
}