using System;
using System.Linq;

namespace module_03
{
   
    internal class Program
    {
        public static void Task1()
        {
            Console.WriteLine("Enter Sentence: ");
            var sentence = Console.ReadLine();
            if (sentence != null)
            {
                var words = sentence.Split(',');
                var palindromes = Array.FindAll(words, s => s.IsPalindrome());
                Console.WriteLine("\nAll palindromes: ");
                foreach (var  palindrome in palindromes)
                    Console.WriteLine(palindrome);
                
                Console.WriteLine("\nAll words that are not palindromes: ");
                words = words.Except(palindromes).ToArray();
                var NewSentence = String.Join(",", words);
                Console.WriteLine(NewSentence);
                
            }
        }

        public static void Task2()
        {
            var m1 = new Movie("Lord of the Rings", "Peter Jackson", 2002);
            var m2 = new Movie("Harry Potter and Sorcerer's Stone", "Chris Columbus", 2001);
            var m3 = new Movie("Harry Potter and the Chamber of Secret", "Chris Columbus", 2002);
            var m4 = new Movie("Harry Potter and the Chamber of Secret", "Chris Columbus", 2002);
            var m5 = new Movie("Star Wars: A New Hope (Episode IV)", "George Lucas",  1977);
            var movies = new Movie[] {m1, m2, m3, m4, m5};

            var movieToCheck = new Movie();
            foreach (var movie in movies)
            {
                if (movie == movieToCheck) Console.WriteLine("There's is your movie (checked with ==).");
                if (movie.Equals(movieToCheck)) Console.WriteLine("There's is your movie (checked with Equals).");
            }
            
            Console.WriteLine("Enter Year: ");
            int year;
            Int32.TryParse(Console.ReadLine(), out year);
            var filteredMovies = Array.FindAll(movies, m => m.Year > year);
            Console.WriteLine($"\nAll movies with year bigger than {year}: ");
            foreach (var  movie in filteredMovies)
                Console.WriteLine(movie);
        }

        public static void Main(string[] args)
        {
            Task1();
            Console.WriteLine("------------------------------------------");
            Task2();
            Console.WriteLine("------------------------------------------");
            
        }
    }
}