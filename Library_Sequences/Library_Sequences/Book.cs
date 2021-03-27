using System;
using System.ComponentModel;

namespace Library_Sequences
{
    public class Book: IComparable<Book>
    {
        public Book()
        {
        }
        
        public Book(uint id, string name, string author, string genre)
        {
            Id = id;
            Name = name;
            Author = author;
            Genre = genre;
        }
        public uint Id { get; private set; }
        public string Name { get; private set; }
        public string Author { get; private set; }
        public int Year { get; private set; }
        public string Genre { get; private set; }

        public string Info => Author + " - " + Name;
        
        public override string ToString() {
            string res = "";
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this)) 
                res += ($"\t{prop.Name}: {prop.GetValue(this)}\n");
            return res;
        }

        public int CompareTo(Book other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            var idComparison = Id.CompareTo(other.Id);
            if (idComparison != 0) return idComparison;
            var nameComparison = string.Compare(Name, other.Name, StringComparison.Ordinal);
            if (nameComparison != 0) return nameComparison;
            var authorComparison = string.Compare(Author, other.Author, StringComparison.Ordinal);
            if (authorComparison != 0) return authorComparison;
            var yearComparison = Year.CompareTo(other.Year);
            if (yearComparison != 0) return yearComparison;
            return string.Compare(Genre, other.Genre, StringComparison.Ordinal);
        }
    }
}