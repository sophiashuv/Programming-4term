using System;
using System.Collections.Generic;
using System.Linq;

namespace ModuleLibrary
{
    public abstract class Edition
    {
        public Edition(int isbn, string title, int yearOfPublishing)
        {
            Isbn = isbn;
            Title = title;
            YearOfPublishing = yearOfPublishing;
        }

        public int Isbn { get; set; }

        public string Title { get; set;}

        public int YearOfPublishing { get; set;}

        public abstract double Payment();
        public abstract Edition BorrowEdititon();

        public override string ToString() => $"Isbn: {Isbn}\n" +
                                             $"Title: {Title}\n" +
                                             $"YearOfPublishing: {YearOfPublishing}\n";
    }
    
    public sealed class Book : Edition
    {
        public static double rate1 = 50;
        
        public Book(int isbn, string title, int yearOfPublishing, List<String> authorsOfTheBook) : base (isbn, title, yearOfPublishing)
        {
            AuthorsOfTheBook = authorsOfTheBook;
        }

        public List<String> AuthorsOfTheBook { get; set; }
        public override double Payment()
        {
            return rate1 / (DateTime.Now.Year - YearOfPublishing);
        }

        public override Edition BorrowEdititon()
        {
            return this;
        }

        public override string ToString()
        {
            return base.ToString() + $"Authors: {String.Join(" ", AuthorsOfTheBook)}\n";
        }
    }
    
    public class Article {
        
        public Article(string title, List<String> authorsOfTheArticle)
        {
            Title = title;
            AuthorsOfTheArticle = authorsOfTheArticle;
        }

        public string Title { get; set; }

        public List<String> AuthorsOfTheArticle { get; set; }

        public override string ToString()
        {
            return $"{Title}: {String.Join(" ", AuthorsOfTheArticle)}";
        }
    }

public class Journal : Edition
{
    public static double rate2 = 100;
    
    public Journal(int isbn, string title, int yearOfPublishing, int? prestige_coefficient_value, List<Article> articles)
        : base(isbn, title, yearOfPublishing)
    {
        Articles = articles;
        Prestige_coefficient_value = prestige_coefficient_value;
    }

    public int? Prestige_coefficient_value { get; set; }

    public List<Article> Articles { get; set; }

    public override double Payment()
    {
        return rate2 / (DateTime.Now.Year - YearOfPublishing + 1);
    }
    public override Edition BorrowEdititon()
    {
        if (Prestige_coefficient_value < 1 || Prestige_coefficient_value > 5 || Prestige_coefficient_value == null)
        {
            throw new JournalCannotBeBorrowed(Isbn, Title, Prestige_coefficient_value);
        }
        return this;
    }

    public override string ToString()
    {
        return $"{base.ToString()}" +
               $"Prestige_coefficient_value: {Prestige_coefficient_value}\n" +
               $"Arcles: \n{String.Join("\n", Articles)}\n";
    }
}

public class JournalCannotBeBorrowed : Exception
{
    public JournalCannotBeBorrowed(int isbn, string title, int? prestige_coefficient_value)
        : base($"The journal ({isbn} {title}) can\'t be borrowed because it has invalid prestige coeffecient = {prestige_coefficient_value}.")
    { }
}




class Library
{
    
    public Library(List<Book> books, List<Journal> journals)
    {
        Books = books;
        Journals = journals;
        BorrovedEditions = new List<Edition>();
    }

    public List<Book> Books { get; set; }

    public List<Journal> Journals { get; set; }
    
    public List<Edition> BorrovedEditions{ get; set;}
    
    public  void ClientBorrowsSomeEdition(Client client, Edition edition, DateTime current_date)
    {
        var fVal = BorrovedEditions.Find(x => (x.Isbn == edition.Isbn));
        
        if (fVal == null)
        {
            BorrovedEditions.Add(edition.BorrowEdititon());
            client.Editions.Add(edition.BorrowEdititon());
            Console.WriteLine($"Client {client.Surname} has borrowed the Edition({edition.Isbn}, {edition.Title}) {current_date}");

        }
        else
        {
            throw new Exception($"Book {edition.Title} is already borrowed");
        }
    }
}

public delegate void BorrowedEditionEventHandler(Client client, Edition edition, DateTime current_date);

public class Client
{
    public event BorrowedEditionEventHandler BorrowedEdition;

    //public event EventHandler<BorrowedEditionEventArgs> BorrowedEditionStandard;


    public Client(string name, string surname, List<Edition> wontToBorrow)
    {
        Name = name;
        Surname = surname;
        WontToBorrow = wontToBorrow;
        Editions = new List<Edition>();
    }
    public List<Edition> Editions { get; set; }
    public List<Edition> WontToBorrow { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    
    public DateTime CurrentDate { get; set; }
    
    public void BorrowEdition(Edition edition, DateTime current_date)
    {
        Editions.Add(edition);
        BorrowedEdition?.Invoke(this, edition, current_date);
        //RegisteredForCourseStandard?.Invoke(this, new StudentRegisteredEventArgs(course));
    }

}

    internal class Program
    {
        // public static void ClientBorrowsSomeEdition(Client client, Edition edition, DateTime current_date)
        // {
        //     Console.WriteLine($"Client {client.Surname} has borrowed the Edition({edition.Isbn}, {edition.Title}) {current_date}");
        // }
        
        static void Main(string[] args)
        {
            Journal.rate2 = 30;
            Book.rate1 = 100;
            List<String> authorsOfTheBooks1 = new List<String>() { "author1", "author2" };
            List<String> authorsOfTheBooks2 = new List<String>() { "author2" };

            List<String> authorsOfTheArticles1 = new List<String>() { "author2", "author3" };
            List<String> authorsOfTheArticles2 = new List<String>() { "author4" };

            Book book1 = new Book(4521, "Book1", 2010, authorsOfTheBooks1);
            Book book2 = new Book(8712, "Book2", 1989, authorsOfTheBooks2);
            Book book3 = new Book(3069, "Book3", 2000, authorsOfTheBooks1);

            Article article1 = new Article("Article1", authorsOfTheArticles1);
            Article article2 = new Article("Article2", authorsOfTheArticles1);
            Article article3 = new Article("Article3", authorsOfTheArticles2);

            List<Article> articles1 = new List<Article>() { article1, article2, article3 };
            List<Article> articles2 = new List<Article>() { article1, article2 };

            Journal journal1 = new Journal(2103, "Journal1", 2001, 2, articles1);
            Journal journal2 = new Journal(6512, "Journal2", 2015, 5, articles1);
            Journal journal3 = new Journal(2102, "Journal3", 1985, 8, articles2);
            Journal journal4 = new Journal(5036, "Journal4", 1996, 4, articles1);


            var library = new Library(new List<Book>() {book1, book2, book3},
                new List<Journal>() {journal1, journal2, journal3, journal4});
            // List<Edition> editions = new List<Edition>() { book1, book2, book3, journal1, journal2, journal3, journal4 };
            // List<Edition> borrowed = new List<Edition>() { };
            //List<Journal> journals = new List<Journal>() { journal1, journal2, journal3, journal4 };
            Client client = new Client("John", "Smith", new List<Edition>(){journal3, journal2, book1});

            foreach (Edition edition in client.WontToBorrow)
            {
                try
                {
                    library.BorrovedEditions.Add(edition.BorrowEdititon());
                    client.Editions.Add(edition.BorrowEdititon());
                }
                catch (JournalCannotBeBorrowed e)
                {
                    Console.WriteLine(e.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{e.Message} ");
                }
            }
            
            Console.WriteLine();
            foreach (var item in client.Editions)
            {
                Console.WriteLine(item);
            }
            
            double sum = 0;
            foreach (var item in client.Editions)
            {
                sum += item.Payment();
            }
            
            Console.WriteLine("Sum of borrowed editions: " + sum);
            
              // TASK 2
            Dictionary<string, List<Edition>> AuthersDictionary = new Dictionary<string, List<Edition>>();
              
            foreach (var book in library.Books)
            {
                foreach (var author in book.AuthorsOfTheBook)
                {
                    if (AuthersDictionary.ContainsKey(author))
                        AuthersDictionary[author].Add(book);
                    else
                        AuthersDictionary.Add(author, new List<Edition>(){book}); 
                }
            }
            foreach (var journal in library.Journals)
            { 
                foreach (var article in journal.Articles)
                { 
                    foreach (var author in article.AuthorsOfTheArticle)
                    { 
                        if (AuthersDictionary.ContainsKey(author))
                              AuthersDictionary[author].Add(journal);
                        else
                              AuthersDictionary.Add(author, new List<Edition>() {journal}); 
                    } 
                } 
            }
            
            foreach (var kvp in AuthersDictionary){
                var ar = String.Join("\n", kvp.Value);
                Console.WriteLine($"{kvp.Key}:\n{ar}");
            } 
            
            // Task 3
            Client client2 = new Client("Ira", "Sloboda", new List<Edition>(){journal3, journal1, book1});
            
            client2.BorrowedEdition += library.ClientBorrowsSomeEdition;
            foreach (var edition in client2.WontToBorrow)
            { 
                try
                {
                    client2.BorrowEdition(edition, DateTime.Now.Date);
                }
                catch (JournalCannotBeBorrowed e)
                { 
                    Console.WriteLine(e.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Exception message is: { e.Message} ");
                }
            }
        }
    }
}