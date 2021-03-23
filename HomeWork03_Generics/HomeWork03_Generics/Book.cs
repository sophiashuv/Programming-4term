using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace HomeWork03_Generics.Properties
{
    class Book:  IComparable
    {
        public Book()
        {
            Console.WriteLine("Enter Book: ");
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this))
            {
                Console.Write($"{prop.Name}: ");
                prop.SetValue(this, Convert.ChangeType(Console.ReadLine(), prop.PropertyType));
            }
        }
        
        public Book(string name, string author, int year)
        {
            Name = name;
            Author = author;
            Year = year;
        }
        public string Name { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
      
        public override bool Equals(object obj)
        {
            if (obj != null && obj is Book)
            {
                Book otherBook = (Book)obj;
                if (Name == otherBook.Name && Author == otherBook.Author && Year == otherBook.Year)
                    return true;
                else
                    return false;
            }
            return false;
        }
      
        public override int GetHashCode()
        {
            int hashCode = -1481573794;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Author);
            hashCode = hashCode * -1521134295 + Year.GetHashCode();
            return hashCode;
        }
      
        public static bool operator ==(Book book1, Book book2)
        {
            return book1.Equals(book2);
        }
        public static bool operator !=(Book book1, Book book2)
        {
            return book1.Equals(book2);
        }

        public override string ToString()
        {
            return $"Name: {Name} \nAuthor: {Author} \nYear: {Year}\n";
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            Book b = obj as Book;
            return this.Name.CompareTo(b.Name);
        }
    }
}