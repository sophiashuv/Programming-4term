using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;

namespace module07_Shuvar
{
    internal class Program
    {
        public static double CountSum(double x, double e)
        {
            int n = 0;
            double prev = 0;
            double a = 1;
            double res = a;
            while (Math.Abs(prev - a) > e)
            {
                n++;
                prev = a;
                a *= (-1 * Math.Pow(x, 2)) / ((2 * n) * (2 * n + 1));
                res += a;
            }
            return res;
        }

        public static void Task1()
        {
            Console.WriteLine("Enter a: ");
            double a = double.Parse(Console.ReadLine() ?? string.Empty);
            Console.WriteLine("Enter b: ");
            double b = double.Parse(Console.ReadLine() ?? string.Empty);
            Console.WriteLine("Enter h: ");
            double h = double.Parse(Console.ReadLine() ?? string.Empty);
            Console.WriteLine("Enter e: ");
            double e = double.Parse(Console.ReadLine() ?? string.Empty);
            for (double x = a; x < b; x += h)
                Console.WriteLine($"sin({x})/{x} = {CountSum(x, e)}");
        }

        public abstract class TVshow: IComparable<TVshow>
        {
            public string Title { get; set; }
            public double Length { get; set; }
            public string Channel { get; set; }

            public int CompareTo(TVshow other)
            {
                return Title.CompareTo(other.Title);
            }

            public override string ToString() {
                string res = "\t" + this.GetType().Name + "\n";
                foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this)) 
                    res += ($"\t{prop.Name}: {prop.GetValue(this)}\n");
                return res;
            }
        }

        public abstract class EntertainmentTVshow : TVshow
        {
        }
        
        public abstract class InfoTVshow : TVshow
        {
        }
        
        public class News : InfoTVshow
        {
        }
        
        public class Cartoon : EntertainmentTVshow
        {
        }
        
        public class Show : EntertainmentTVshow
        {
        }
        
        public class LengthComparer : IComparer<TVshow>
        {
            public int Compare(TVshow x, TVshow y)
            {
                if (ReferenceEquals(x, y)) return 0;
                if (ReferenceEquals(null, y)) return 1;
                if (ReferenceEquals(null, x)) return -1;
                return y.Length.CompareTo(x.Length);
            }
        }

        public class TVprogram : IEnumerable
        {
            private List<TVshow> program;
            public TVprogram()
            {
                Program = new List<TVshow>();
            }

            public List<TVshow> Program
            {
                get;
                set;
            }

            // public IEnumerator<TVshow> GetEnumerator() => Program.GetEnumerator();
            //
            // IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
            //
            static bool IsInfo(TVshow r)
            {
                return r.GetType().BaseType.Name == typeof(InfoTVshow).Name;
            }
        
            public IEnumerator GetEnumerator(){
                return new FilteredEnumerator<TVshow>(Program, IsInfo);
            }


            public override string ToString()
            {
                var res = "";
                foreach (var r in Program)
                    res += r.ToString() + "\n";
                return res;
            }

            public void SortByTitle()
            {
                Program.Sort();
            }
            public void SortByLength()
            {
                Program.Sort(new LengthComparer());
            }

            public void read_from_csv(string fileName)
            {
                using (var reader = new StreamReader(fileName))
                {
                    var atters = reader.ReadLine().Split(',');
                    while (!reader.EndOfStream)
                    {
                        var values = reader.ReadLine().Split(',');
                        if (values[0] == "News")
                        {
                            News ob = new News();
                            for (int i = 1; i < atters.Length; i++)
                            {
                                PropertyInfo propertyInfo = ob.GetType().GetProperty(atters[i]);
                                propertyInfo.SetValue(ob, Convert.ChangeType(values[i], propertyInfo.PropertyType),
                                    null);
                            }
                            Program.Add(ob);
                        }

                        if (values[0] == "Cartoon")
                        {
                            Cartoon ob = new Cartoon();
                            for (int i = 1; i < atters.Length; i++)
                            {
                                PropertyInfo propertyInfo = ob.GetType().GetProperty(atters[i]);
                                propertyInfo.SetValue(ob, Convert.ChangeType(values[i], propertyInfo.PropertyType),
                                    null);
                            }

                            Program.Add(ob);
                        }

                        if (values[0] == "Show")
                        {
                            Show ob = new Show();
                            for (int i = 1; i < atters.Length; i++)
                            {
                                PropertyInfo propertyInfo = ob.GetType().GetProperty(atters[i]);
                                propertyInfo.SetValue(ob, Convert.ChangeType(values[i], propertyInfo.PropertyType),
                                    null);
                            }
                            Program.Add(ob);
                        }
                    }
                }
            }
        }

        public class FilteredEnumerator<T> : System.Collections.Generic.IEnumerator<T>
        {
            readonly System.Collections.Generic.IEnumerator<T> enumerator;
            readonly System.Func<T, bool> filter;

            public FilteredEnumerator(System.Collections.Generic.IEnumerable<T> enumerable, System.Func<T, bool> filter=null)
            {
                if (enumerable == null)
                    throw new System.ArgumentNullException();
                this.enumerator = enumerable.GetEnumerator();
                this.filter = filter;
            }
            public T Current => enumerator.Current;

            public void Dispose() => enumerator.Dispose();

            object System.Collections.IEnumerator.Current => Current;

            public bool MoveNext()
            {
                while (enumerator.MoveNext())
                    if (filter == null || filter(enumerator.Current))
                        return true;
                return false;
            }
        
            public void Reset() => enumerator.Reset();
        }
        public static void Task2()
        {
            var tVprogram = new TVprogram();
            tVprogram.read_from_csv("/Users/sophiyca/RiderProjects/module07_Shuvar/module07_Shuvar/shows.csv");
            
            Console.WriteLine("TVshows sorted by Title:");
            tVprogram.SortByTitle();
            Console.WriteLine(tVprogram);
            Console.WriteLine("------------");
            Console.WriteLine("Info TVshows sorted by Length:");
            tVprogram.SortByLength();
            foreach (var el in tVprogram)
                Console.WriteLine(el);

            Console.WriteLine("------------");
            Console.WriteLine("Channels and their TVprograms:");
            var channels = new Dictionary<string, TVprogram>();
            foreach (var el in tVprogram.Program)
            {
                var k = (TVshow) el;
                if (channels.ContainsKey(k.Channel))
                    channels[k.Channel].Program.Add(k);
                else
                {
                    var key = new TVprogram();
                    channels.Add(k.Channel, key);
                    channels[k.Channel].Program.Add(k);
                }
            }
            foreach (var kvp in channels)
                Console.WriteLine($"{kvp.Key}: {kvp.Value}");
        }

        public static void Main(string[] args)
        {
            Task1();
            Task2();
        }
    }
}