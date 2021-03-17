using System;
using System.ComponentModel;

namespace module_03
{
    public class Movie: IEquatable<Movie>
    {
        public Movie()
        {
            Console.WriteLine("Enter Movie: ");
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this))
            {
                Console.Write($"{prop.Name}: ");
                prop.SetValue(this, Convert.ChangeType(Console.ReadLine(), prop.PropertyType));
            }
        }
        
        public Movie(string title, string director, int year)
        {
            Title = title;
            Director = director;
            Year = year;
        }
        
        public string Title { get; set; }
        public string Director { get; set; }
        public int Year { get; set; }
        
        public static bool operator ==(Movie obj1, Movie obj2)
        {
            if (ReferenceEquals(obj1, obj2))
            {
                return true;
            }
            if (ReferenceEquals(obj1, null))
            {
                return false;
            }
            if (ReferenceEquals(obj2, null))
            {
                return false;
            }
            return obj1.Title == obj2.Title && obj1.Director == obj2.Director && obj1.Year == obj2.Year;
            // return obj1.Equals(obj2);
        }

        public static bool operator !=(Movie obj1, Movie obj2)
        {
            return !(obj1 == obj2);
        }
        
        public bool Equals(Movie other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            // return Title == other.Title && Director == other.Director && Year == other.Year;
            return false;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Movie) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Title != null ? Title.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Director != null ? Director.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Year;
                return hashCode;
            }
        }
        
        public Movie InputProduct()
        {
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this))
            {
                Console.Write($"{prop.Name}: ");
                prop.SetValue(this, Convert.ChangeType(Console.ReadLine(), prop.PropertyType));
            }
            return this;
        }
        
        public override string ToString() {
            string res = "";
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this)) 
                res += ($"{prop.Name}: {prop.GetValue(this)}\n");
            return res;
        }
    }
}