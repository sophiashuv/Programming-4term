using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeWork_01
{
    class Program
    {
        static void Task1() 
            /*
             * 1. Розкласти задане ціле число на прості множники і вивести цей розклад на екран.
             */
        {
            List<int> res = new List<int>();
            int a, b;
            Console.WriteLine("Enter your integer: ");
            a = int.Parse(Console.ReadLine() ?? string.Empty);

            for (b = 2; a > 1; b++)
                if (a % b == 0)
                    for (;a % b == 0; a /= b) res.Add(b);
            
            Console.WriteLine(String.Join(", ", res));
        }
        
        
        static void Task2() 
            /*
             * 2. Дано масив дійсних чисел. Вивести номер того числа, яке найближче до цілого..
             */
        {
            Console.WriteLine("Enter your doubles: ");
            double[] arr = Array.ConvertAll(Console.ReadLine().Split(' '), double.Parse);
            double[] differ = new double[arr.Length];
            for (int i = 0; i < arr.Length; i++)
                differ[i] = Math.Min(Math.Ceiling(arr[i]) - arr[i], arr[i] - Math.Floor(arr[i]));
            
            var m = differ.Min();
            Console.WriteLine(arr[Array.IndexOf(differ, m)]);
        }
        
        
        static void Task3() 
            /*
             * 3. Дано масив з N різних дійсних чисел. Обчислити суму значень, розташованих між
             *    найбільшим і найменшим значеннями (включно з ними).
             */
        {
            Console.WriteLine("Enter your doubles: ");
            double[] arr = Array.ConvertAll(Console.ReadLine().Split(' '), double.Parse);
            var max = arr.Select((n, i) => (Number: n, Index: i)).Max();
            var min = arr.Select((n, i) => (Number: n, Index: i)).Min();
            var min_i = max.Index > min.Index ? min.Index : max.Index;
            var max_i = max.Index > min.Index ? max.Index : min.Index;
            double[] result = new Double[max_i - min_i + 1];
            Array.Copy(arr, min_i, result, 0, max_i - min_i + 1);
            double sum = result.Aggregate((total, next) => total + next);
            Console.WriteLine($"Sum: {sum}");
        }
        
        
        static void Task4() 
            /*
             * 4. Дано натуральне число n, дійсні числа an,...a0. Використовуючи схему Горнера,
             *    обчислити значення многочлена в заданій точці .
             */
        {
            Console.WriteLine("Enter n: ");
            int n = int.Parse(Console.ReadLine() ?? string.Empty);
            Console.WriteLine("Enter x: ");
            int x = int.Parse(Console.ReadLine() ?? string.Empty);
            Console.WriteLine("Enter your doubles: ");
            double[] arr = Array.ConvertAll(Console.ReadLine().Split(' '), double.Parse);
            double res = 0;
            for (int i = 0; i < n; i++, n--)
                res += arr[n - i - 1] * Math.Pow(x, n);
            Console.WriteLine($"Sum: {res}");
        }
        
        
        static void Task5() 
            /*
             * 5. Впорядкувати рядки матриці цілих чисел розміру n*m за зростанням сум їх елементів.
             */
        {
            Console.WriteLine("Enter n: ");
            int n = int.Parse(Console.ReadLine() ?? string.Empty);
            Console.WriteLine("Enter m: ");
            int m = int.Parse(Console.ReadLine() ?? string.Empty);
            int[,] matrix = new int[n, m];
            int[] sums = new int[n];
            Console.WriteLine("Enter your matrix: ");
            
            for (var i = 0; i < n; i++)
            {
                var line = Console.ReadLine();
                var spl = line.Split(' ');
                if (spl.Length != m) throw new FormatException();
                for (var j = 0; j < m; j++)
                {
                    matrix[i, j] = int.Parse(spl[j]);
                    sums[i] += matrix[i, j];
                }
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (sums[i] > sums[j])
                    {
                        for (int k = 0; k < m; k++)
                        {
                            int temp = matrix[i, k];
                            matrix[i, k] = matrix[j, k];
                            matrix[j, k] = temp;
                        }
                    }
                }
            }
            // var res = matrix.OrderBy(x => x.Sum()).ToArray();
            
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    Console.Write(string.Format("{0} ", matrix[i, j]));
                }
                Console.Write(Environment.NewLine);
            }
        }
        
        
        static void Task6() 
            /*
             * 6. По заданій матриці дійсних чисел розміру n*m побудувати послідовність елементів
             *    b1,…bn, де bk – кількість від’ємних елементів у k–му рядку.
             */
        {
            Console.WriteLine("Enter n: ");
            int n = int.Parse(Console.ReadLine() ?? string.Empty);
            Console.WriteLine("Enter m: ");
            int m = int.Parse(Console.ReadLine() ?? string.Empty);
            int[,] matrix = new int[n, m];
            int[] am = new int[n];
            Console.WriteLine("Enter your matrix: ");
            
            for (var i = 0; i < n; i++)
            {
                var line = Console.ReadLine();
                var spl = line.Split(' ');
                if (spl.Length != m) throw new FormatException();
                for (var j = 0; j < m; j++)
                {
                    matrix[i, j] = int.Parse(spl[j]);
                    if ( matrix[i, j] < 0) am[i] += 1;
                }
            }
            Console.WriteLine(String.Join(", ", am));
        }
        
        
        static void Task7() 
            /*
             * 4. Обчислити для заданого з заданою точністю суму
             */
        {
            Console.WriteLine("Enter x: ");
            double x = double.Parse(Console.ReadLine() ?? string.Empty);
            Console.WriteLine("Enter e: ");
            double e = double.Parse(Console.ReadLine() ?? string.Empty);
            int n = 1;
            double res = 0;
            var f = 1;
            var a = x;
            while (a > e)
            {
                n += 1;
                f *= (2 * n - 1) * (2 * n - 2);
                a = Math.Pow(-1, n) * Math.Pow(x, 2 * n - 1) / f;
                res += a;
            }
            Console.WriteLine(res);
        }
        
        
        static void Main(string[] args)
        {
            // Task1();
            // Task2();
            // Task3();
            // Task4();
            // Task5();
            // Task6();
            Task7();
        }
    }
}