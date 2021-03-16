using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Book:IComparable<Book>
    {
        
        public string Name { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }

        public static IComparer<Book> Sort_by_author { get => new AuthorComp(); }
        public int CompareTo(Book other)
        {
            return Name.CompareTo(other.Name);
        }
        public override string ToString()
        {
            return $"Name: {Name} \n Author: {Author} \n Year: {Year}";
        }
    }

    class Collection:IEnumerable<Book>
    {
        private List<Book> coll = new List<Book>();

        public void Add(Book to_add)
        {
            this.coll.Add(to_add);
        }

        public void Delete_by_index(int index)
        {
            this.coll.RemoveAt(index);
        }

        public IEnumerator<Book> GetEnumerator() => coll.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Print()
        {
            foreach (var item in coll)
            {
                Console.WriteLine(item.ToString());
            }
        }
        public void Sort(IComparer<Book> comparer)
        {
            coll.Sort(comparer);
        }
        public Book this[int index]
        {
            get => coll[index];
            set => coll[index] = value;
        }
    }

    class AuthorComp : IComparer<Book>
    {
        public int Compare(Book x, Book y)
        {
            return x.Author.CompareTo(y.Author);
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            Collection col = new Collection();
            Book a = new Book() {Name = "name1", Author = "author1", Year = 2020};
            col.Add(a);
            Book b = new Book() { Name = "name5", Author = "author4", Year = 2021};
            Book c = new Book() { Name = "name3", Author = "author3", Year = 2022};
            col.Add(b);
            col.Add(c);
            col.Print();
            Console.WriteLine("---------------------");
            col.Sort(Book.Sort_by_author);
            //col.Sort(new AuthorComp());
            col.Print();
            Console.WriteLine("---------------------");
            foreach (var item in col)
            {
                Console.WriteLine(item.ToString());
            }
            Console.WriteLine("---------------------");
            col[0] = b;
            foreach (var item in col)
            {
                Console.WriteLine(item.ToString());
            }
        }
    }
}
