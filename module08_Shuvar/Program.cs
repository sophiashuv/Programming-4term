using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;

namespace module08_Shuvar
{
    public abstract class EducationalInstitution: IComparable<EducationalInstitution>
    {
        public string Title { get; set; }
        public double Students { get; set; }
        public string City { get; set; }

        public int CompareTo(EducationalInstitution other) => Title.CompareTo(other.Title);

        public override string ToString() {
            string res = "\t" + this.GetType().Name + "\n";
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this)) 
                res += ($"\t{prop.Name}: {prop.GetValue(this)}\n");
            return res;
        }
    }
    public abstract class HighEducationalInstitution : EducationalInstitution
    {
    }
    public abstract class MiddleEducationalInstitution : EducationalInstitution
    {
    }
    public class University : HighEducationalInstitution
    {
    }
    
    public class Academy : HighEducationalInstitution
    {
    }
    public class School : MiddleEducationalInstitution
    {
    }
    public class Lyceum : MiddleEducationalInstitution
    {
    }
    public class StudentComparrer : IComparer<EducationalInstitution>
    {
        public int Compare(EducationalInstitution x, EducationalInstitution y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (ReferenceEquals(null, y)) return 1;
            if (ReferenceEquals(null, x)) return -1;
            return y.Students.CompareTo(x.Students);
        }
    }
    
    public class Collection : IEnumerable
    {
        public Collection() => Lst = new List<EducationalInstitution>();
        public List<EducationalInstitution> Lst { get; set; }
        static bool IsHigh(EducationalInstitution r) => r.GetType().BaseType.Name == typeof(HighEducationalInstitution).Name;
        public static bool IsSchool(EducationalInstitution r) => r.GetType().Name == typeof(School).Name;
        public IEnumerator GetEnumerator() => new FilteredEnumerator<EducationalInstitution>(Lst, IsHigh);

        public override string ToString()
        {
            var res = "";
            foreach (var r in Lst)
                res += r.ToString() + "\n";
            return res;
        }

        public void SortByTitle() => Lst.Sort();

        public void SortByLength() => Lst.Sort(new StudentComparrer());
        
        public void read_from_csv(string fileName)
        {
            using (var reader = new StreamReader(fileName))
            {
                var atters = reader.ReadLine().Split(',');
                while (!reader.EndOfStream)
                {
                    var values = reader.ReadLine().Split(',');
                    if (values[0] == "University")
                    {
                        var ob = new University();
                        for (int i = 1; i < atters.Length; i++)
                        {
                            PropertyInfo propertyInfo = ob.GetType().GetProperty(atters[i]);
                            propertyInfo.SetValue(ob, Convert.ChangeType(values[i], propertyInfo.PropertyType),
                                null);
                        }
                        Lst.Add(ob);
                    }

                    if (values[0] == "Academy")
                    {
                        var ob = new Academy();
                        for (int i = 1; i < atters.Length; i++)
                        {
                            PropertyInfo propertyInfo = ob.GetType().GetProperty(atters[i]);
                            propertyInfo.SetValue(ob, Convert.ChangeType(values[i], propertyInfo.PropertyType),
                                null);
                        }
                        Lst.Add(ob);
                    }

                    if (values[0] == "School")
                    {
                        var ob = new School();
                        for (int i = 1; i < atters.Length; i++)
                        {
                            PropertyInfo propertyInfo = ob.GetType().GetProperty(atters[i]);
                            propertyInfo.SetValue(ob, Convert.ChangeType(values[i], propertyInfo.PropertyType),
                                null);
                        }
                        Lst.Add(ob);
                    }

                    if (values[0] == "Lyceum")
                    {
                        var ob = new Lyceum();
                        for (int i = 1; i < atters.Length; i++)
                        {
                            PropertyInfo propertyInfo = ob.GetType().GetProperty(atters[i]);
                            propertyInfo.SetValue(ob, Convert.ChangeType(values[i], propertyInfo.PropertyType),
                                null);
                        }
                        Lst.Add(ob);
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
                a *= (-1 * Math.Pow(x, 2)) / ((2 * n - 1) * (2 * n));
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
            for (double x = a; x <= b; x += h)
                Console.WriteLine($"cos({x}) = {CountSum(x, e)}");
        }
        
        public static void Task2()
        {
            var collection = new Collection();
            collection.read_from_csv("/Users/sophiyca/RiderProjects/module08_Shuvar/module08_Shuvar/educationalInstitutions.csv");
            Console.WriteLine("EducationalInstitution sorted by Title:");
            collection.SortByTitle();
            Console.WriteLine(collection);
            Console.WriteLine("------------");
            Console.WriteLine("HighEducationalInstitution sorted by Students:");
            collection.SortByLength();
            foreach (var el in collection)
                Console.WriteLine(el);
            Console.WriteLine("------------");
            Console.WriteLine("Citys and their Schools:");
            
            var channels = new Dictionary<string, Collection>();
            var schools = collection.Lst.FindAll(e => Collection.IsSchool(e));
            foreach (var k in schools)
            {
                if (channels.ContainsKey(k.City))
                    channels[k.City].Lst.Add(k);
                else
                {
                    var key = new Collection();
                    channels.Add(k.City, key);
                    channels[k.City].Lst.Add(k);
                }
            }
            foreach (var kvp in channels)
                Console.WriteLine($"{kvp.Key}: {kvp.Value}");
        }
        public static void Main(string[] args)
        {
            Task2();
            
        }
    }
}