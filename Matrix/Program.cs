using System;

namespace Matrix
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Matrix test = new Matrix(3, 3);
                test.Generate();
                Console.WriteLine(test);
                Matrix second = new Matrix(3, 3);
                second.Generate();
                Console.WriteLine();
                Console.WriteLine(second);
                Matrix result = test * second;
                Console.WriteLine();
                Console.WriteLine(result);

                Matrix entering = new Matrix(2, 2);
                entering.EnterValues();
                Console.WriteLine(entering);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadLine();
        }
    }
}