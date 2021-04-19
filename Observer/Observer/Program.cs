using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace Observer
{
    
    // Завдання 1.1. Клас має метод для генерування пар точок(x1, y1) i(x2, y2) з випадковими 
    // цiлочисловими координатами з вiдрiзка[a, b]. Якщо x16=x2iy16=y2,то таку пару точок зберiгають 
    // в колекцiї i за умови наявностi хоч одного зареєстрованого спостерiгача генерують подiю про 
    // запис такої пари. Якщожодного спостерiгача немає, то генерування точок припиняють.Два 
    // об’єкти-обсервери використовують пiдходящу пару точок як лiву верхню i праву нижню вершини 
    // прямокутникiв. Один обсервер будує колекцiю зn1прямокутникiв, розмiщених у першому квадрантi,
    // другий – зn2прямокутникiв у верхнiй пiвплощинi.Отримати обидвi колекцiї та серiалiзувати їх 
    // у xml-файли. На консольвивести загальну кiлькiсть згенерованих точок.
    
    using System;
    using System.Collections.Generic;

    public delegate bool CheckDelegate((Point, Point) pts);
    class Program
    {
        public static bool ChackP1((Point, Point) pts)
        {
            return (pts.Item1.Y >= 0 && pts.Item2.Y >= 0) ;
        }
        
        public static bool ChackP2((Point, Point) pts)
        {
            return ((pts.Item1.Y >= 0 && pts.Item1.X >= 0 && pts.Item2.X >= 0 && pts.Item2.Y >= 0)) ;
        }
        
        private static void Main(string[] args)
        {
            CheckDelegate ch1 = new CheckDelegate(ChackP1);
            CheckDelegate ch2 = new CheckDelegate(ChackP2);
            
            var p = new Generator(-3, 10);
            var o = new Observer(10, p, ch1);
            var o1 = new Observer(15, p, ch2);
            
            
            while (p.CheckIfSubscribed())
            {
                p.Generate();
            }

            Console.WriteLine(p.Points.Count);

            var writer = new System.Xml.Serialization.XmlSerializer(typeof(List<(Point, Point)>));
            var wfile = new System.IO.StreamWriter(@"/Users/sophiyca/RiderProjects/Observer/Observer/observer1.xml");
            
            writer.Serialize(wfile, o.Rectangles);
            wfile.Close();
            
            var wfile1 = new System.IO.StreamWriter(@"/Users/sophiyca/RiderProjects/Observer/Observer/observer2.xml");
            writer.Serialize(wfile1, o1.Rectangles);
            wfile1.Close();
        }
    }

    public class Point
    {
        public int X { set; get; }
        public int Y { set; get; }

        public Point()
        {
        }

        public Point(int xx, int yy)
        {
            X = xx;
            Y = yy;
        }

        public override string ToString()
        {
            return $"{X} {Y}";
        }
    }

    public delegate void Check(Generator gen, uint ind);

    public sealed class Generator
    {
        public int LowerLimit { set; get; }
        private int UpperLimit { set; get; }

        public Dictionary<uint, (Point, Point)> Points { get; }

        private uint _ind;
        public Generator(int b, int t)
        {
            LowerLimit = b;
            UpperLimit = t + 1;
            Points = new Dictionary<uint, (Point, Point)>();
            _ind = 0;
        }

        public event Check GenerateEvent;
        Random rng = new Random();

        public void Generate()
        {
            var x1 = rng.Next(LowerLimit, UpperLimit);
            var y1 = rng.Next(LowerLimit, UpperLimit);
            var x2 = rng.Next(LowerLimit, UpperLimit);
            var y2 = rng.Next(LowerLimit, UpperLimit);
            if (x1 == x2 || y1 == y2) return;
            Points[_ind] = (new Point(x1, y1), new Point(x2, y2));
            RaiseGenerateEvent();
        }

        public bool CheckIfSubscribed()
        {
            return GenerateEvent != null;
        }

        private void RaiseGenerateEvent()
        {
            GenerateEvent?.Invoke(this, _ind);
            _ind++;
        }
    }

    class Observer
    {
        private uint N { set; get; }

        private CheckDelegate Ch { set; get;}

        public List<(Point, Point)> Rectangles { get; }

        public Observer(uint n, Generator gen, CheckDelegate ch)
        {
            N = n;
            Rectangles = new List<(Point, Point)>();
            Ch = ch;
            gen.GenerateEvent += CheckPoint;
        }

        private void CheckPoint(Generator gen, uint ind)
        {
            var pts = gen.Points[ind];
            if (Ch(pts))
            {
                Rectangles.Add(pts);
                if (Rectangles.Count == N)
                {
                    gen.GenerateEvent -= CheckPoint;
                }
            }
        }
    }
}
    