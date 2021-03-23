using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeWork03_Generics.Properties;

namespace HomeWork03_Generics
{
    //
    // 1. Визначити узагальнений статичний метод, який підраховує кількість елементів, 
    // відмінних від вказаного i-го елемента в масиві.
    //
    // 2. Визначити узагальнений статичний метод, який в деякому масиві елементів 
    // здійснює такі дії: найбільший елемент переміщає на початок масиву, а найменший 
    // елемент – у його кінець. Всі решта елементів зсуває до середини, не міняючи їх 
    // порядку. Нехай задано три масиви: масив цілих чисел, масив рядків, масив об’єктів
    // вашого класу, застосувати до них розроблений метод.
    //
    // 3. Визначити інтерфейс IShape з методом Area() та класи Rectangle і 
    // Trapezoid, що його імплементують. Визначити узагальнений клас Room<>, 
    // параметр якого реалізує IShape, з властивостями Height та Floor (типу T). Реалізувати в 
    // цьому класі метод Volume() для обчислення об’єму кімнати, імплементувати ICloneable для глибокого
    // копіювання та IComparable для порівняння за площею підлоги. Визначити клас RoomComparerByVolume<> 
    // для порівняння кімнат за об’ємом. Визначити клас Building, що містить колекцію кімнат. Реалізувати 
    // необхідні методи та інтерфейс IEnumerable, так щоб перелічувати лише прямокутні кімнати.
    //
    // 4. Розробити власний узагальнений клас однозв’язний список MyList<>. Передбачити набір стандартних 
    // методів, наприклад, додати елемент на початок, в кінець списку, видалити перший/останній елемент і т.п.
    
    internal class Program
    {
        private static int countElements<T>(T[] arr, int index)
        {
            var k = from word in arr  
                where !Equals(word, arr[index])  
                select word;
            return k.Count();
        }

        private static void Task1()
        {
            int[] arr = {1, 2, 3, 4, 3, 5, 3, 8};
            Console.WriteLine("YOUR ARRAY: ");
            Console.WriteLine(string.Join(", ", arr));
            Console.WriteLine($"There are : {countElements(arr, 2)} elements different from element on index {2}");
        }

        private static void CircleMove<T>(T[] arr)
        {
            T maxEl = arr.Max();
            int maxElIndex = Array.IndexOf(arr, maxEl);
            
            T minEl = arr.Min();
            int minElIndex = Array.IndexOf(arr, minEl);

            for (int i = maxElIndex; i > 0; i--)
                arr[i] = arr[i - 1];

            for (int i = minElIndex; i < arr.Length-1; i++)
                arr[i] = arr[i + 1];
            arr[0] = maxEl;
            arr[arr.Length - 1] = minEl;
        }

        private static void Task2()
        {
            int[] arr_int = {1, 2, 3, 4, 3, 5, 3, 8};
            Console.WriteLine("YOUR ARRAY INT: ");
            Console.WriteLine(string.Join(", ", arr_int));
            CircleMove(arr_int);
            Console.WriteLine("NEW ARRAY INT: ");
            Console.WriteLine(string.Join(", ", arr_int));
            
            string[] arr_string = {"Sophisa", "BlaBlaBla", "Waffles", "Mmm"};
            Console.WriteLine("YOUR ARRAY STRING: ");
            Console.WriteLine(string.Join(", ", arr_string));
            CircleMove(arr_string);
            Console.WriteLine("NEW ARRAY STRING: ");
            Console.WriteLine(string.Join(", ", arr_string));
            
            var m1 = new Book("Lord of the Rings", "J. R. R. Tolkien", 1954);
            var m2 = new Book("ZHarry Potter and Sorcerer's Stone", "J. K. Rowling", 1997);
            var m3 = new Book("AHarry Potter and the Chamber of Secret", "J. K. Rowling", 1998);
            var m4 = new Book("Harry Potter and the Chamber of Secret", "J. K. Rowling", 1998);
            
            var arr_books = new Book[] {m1, m2, m3, m4};
            Console.WriteLine("YOUR ARRAY BOOK: ");
            foreach (var book in arr_books) Console.WriteLine(book);
            CircleMove(arr_books);
            Console.WriteLine("NEW ARRAY BOOK: ");
            foreach (var book in arr_books) Console.WriteLine(book);
        }

        private static void Task3()
        {
            var f1 = new Rectangle {A = 4, B = 7};
            var f2 = new Rectangle {A = 7, B = 8};
            var f3 = new Trapezoid {A = 4, B = 7, H = 8};
            var f4 = new Trapezoid {A = 4, B = 7, H = 3};
            var r1 = new Room<IShape> {Height = 4, Floor = f1};
            var r2 = new Room<IShape> {Height = 9, Floor = f2};
            var r3 = new Room<IShape> {Height = 4, Floor = f3};
            var r4 = new Room<IShape> {Height = 4, Floor = f4};
            
            Building building = new Building(){Rooms = new List<Room<IShape>>(){r1, r2, r3, r4}};
            var res = "";
            foreach (var r in building)
                Console.WriteLine($"{r.ToString()} \n");

        }

        private static void Task4()
        {
            var lst = new MyList<int>();
            lst.InsertLast(1);
            lst.InsertFront(9);
            lst.InsertFront(7);
            lst.InsertFront(8);
            lst.InsertLast(3);
            lst.InsertLast(0);
            lst.InsertLast(2);
            Console.WriteLine(lst);
            lst.Clear();
            Console.WriteLine(lst);
        }



        public static void Main(string[] args)
        {
            Console.WriteLine("\n---------------TASK 1---------------");
            Task1();
            Console.WriteLine("\n---------------TASK 2---------------");
            Task2();
            Console.WriteLine("\n---------------TASK 3---------------");
            Task3();
            Console.WriteLine("\n---------------TASK 4---------------");
            Task4();
        }
    }
}