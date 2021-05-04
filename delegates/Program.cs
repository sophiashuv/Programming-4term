using System;

namespace delegates
{

    public class MyInt
    {
        public static void Add(int a, int b)
        {
            Console.WriteLine(a + b);
        }
        
        public static void Sub(int a, int b)
        {
            Console.WriteLine(a - b);
        }
    }
    
    delegate void dlegate1(int x, int y);
    delegate double dlegateIntegral(double x);
    
    internal class Program
    {
        public static double getIntegral(dlegateIntegral fun, double a, double b, int n)
        {
            var h = (b - a) / n;
            var sum = (fun(a) + fun(b)) / 2;
            for (int i = 1; i < n; i++)
            {
                sum += fun(a + i * h);
            }
            return sum * h;
        }
        
        public static void Main(string[] args)
        {
            // dlegate1 fun = new dlegate1(MyInt.Add);
            // fun += MyInt.Sub;
            //
            // fun -= MyInt.Add;
            //
            // fun += (i, i1) => Console.WriteLine(i * i1);
            // fun(5, 6);
            // fun += delegate(int i, int i1) { Console.WriteLine(Math.Pow(i, i1));};
            // fun(5, 6);
            // Action<int, int> func2 = delegate(int i, int i1) { Console.WriteLine(Math.Pow(i, i1));};
            // Func<int, int, double> func3 = delegate(int i, int i1) { return Math.Pow(i, i1);};
            // func2(1, 2);
            // Console.WriteLine(func3(1, 2));
            // // foreach (var v in fun.GetInvocationList())
            // // {
            // //     v.DynamicInvoke(5, 6);
            // // }
            
            dlegateIntegral fun = new dlegateIntegral((i) => i*i);
            Console.WriteLine(getIntegral(fun, 1, 10, 100));
        }
    }
}