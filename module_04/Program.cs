using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace module_04
{
  // Написати для класу System.String extension method ContainsPunctuationSymbols(), який
  // визначає чи стрічка містить знаки пунктуації (крапка, кома і т.д.) в своєму записі.
  // Вводимо з консолі стрічку-речення, де слова розділені пробілами. Порахувати кількість
  // слів, що містять знаки пунктуації. Сформувати і вивести на консоль нове речення,
  // видаливши всі слова, що містять знаки пунктуації.
  
  // 2. Дано масив книг Book(title, author, year). Вводимо книгу з консолі, вивести чи таку книгу
  // знайдено в масиві, перевантаживши для цього оператор == та метод Equals(). Для полів
  // класу передбачити відповідні проперті. На базі цього масиву книг сформувати і вивести
  // на консоль новий масив книг, які були опубліковані після певного року (рік вводиться з
  // консолі).
  
  // 3. Будівельний гіпермаркет продає товари та надає послуги (наприклад, доставка
  // великогабаритних товарів, встановлення їх і т.д.). Товари можуть продаватися поштучно
  // PieceProduct (title, priceForItem) та на вагу WeightProduct (title, priceForKg) (наприклад,
  // цвяхи). Клієнти гіпермаркета можуть оформити собі карточку знижок, яка працює
  // наступним чином:
  // ● ціна товару, що продається на вагу, зменшується на p% ( p може бути різним
  // для різних карточок клієнта, наприклад, один клієнт має карточку зі знижкою
  // 5%, в той час як інший – карточку зі знижкою 10%)
  // ● ціна товару, що продається поштучно, зменшується на p%, але при цьому ще й
  // на карточку цього клієнта зараховується productPrice*p% бонусів, які можуть
  // бути використані для оплати наступної покупки
  // ● за бажанням (за додаткову оплату) клієнт може активувати для своєї карточки
  // ще й p% знижки на послуги Service (title, price) (тобто, може існувати карточка
  // зі знижкою 5%, але знижки не поширюються на послуги, а інша карточка зі
  // знижкою 5% може бути застосована як до товарів, так і до послуг)
  //
  // Дано масив товарів і послуг, які хоче придбати клієнт за одну покупку і певна
  // конфігурація карточки знижок цього клієнта. Вивести суму, яку клієнт повинен
  // заплатити за ці товари та послуги, а також вивести суму накопичених бонусів за цю
  // покупку.
  // В другому випадку припустимо, що клієнт купує лише послуги Service. Посортувати
  // куплені клієнтом послуги за алфавітом.

  class Book
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
      return $"Name: {Name} \nAuthor: {Author} \nYear: {Year}";
    }
  }
  
  
  public static class StringExtension
  {

    public static bool ContainsPunctuationSymbols(this string str)
    {
      char[] symbols = new char[]{',', '.', '!', '?', '/', ';', ':'};
      foreach (var s in symbols)
        if (str.Contains(s))
          return false;
        
      return true;
    }
  } 
  
  internal class Program
  {
    public static void Task1()
    {
      Console.WriteLine("Enter Sentence: ");
      var sentence = Console.ReadLine();
      if (sentence != null)
      {
        var words = sentence.Split(' ');
        var withPunctuation = Array.FindAll(words, s => !s.ContainsPunctuationSymbols());
        Console.WriteLine($"\nThere All : {withPunctuation.Length} words with punctuation.");
 
        Console.WriteLine("\nNew Sentence: ");
        words = words.Except(withPunctuation).ToArray();
        var NewSentence = String.Join(",", words);
        Console.WriteLine(NewSentence);
      }
    }
    
    public static void Task2()
    {
      
      var m1 = new Book("Lord of the Rings", "J. R. R. Tolkien", 1954);
      var m2 = new Book("Harry Potter and Sorcerer's Stone", "J. K. Rowling", 1997);
      var m3 = new Book("Harry Potter and the Chamber of Secret", "J. K. Rowling", 1998);
      var m4 = new Book("Harry Potter and the Chamber of Secret", "J. K. Rowling", 1998);
      
      var books = new Book[] {m1, m2, m3, m4};

      var movieToCheck = new Book();
      foreach (var book in books)
      {
        if (book == movieToCheck) Console.WriteLine("There's is your movie in array.");
      }
            
      Console.WriteLine("Enter Year: ");
      int year;
      Int32.TryParse(Console.ReadLine(), out year);
      var filteredBooks = Array.FindAll(books, m => m.Year > year);
      Console.WriteLine($"\nAll books with year bigger than {year}: ");
      foreach (var book in filteredBooks)
        Console.WriteLine(book);
    }
    
        public static void Task3()
    {
      PieceProduct pieceProduct1 = new PieceProduct { Title = "Product1", PriceForItem = 120.6 };
      PieceProduct pieceProduct2 = new PieceProduct { Title = "Product2", PriceForItem = 100 };

      WeightProduct weightProduct1 = new WeightProduct { Title = "Product1", PriceForKg = 112.75 };
      WeightProduct weightProduct2 = new WeightProduct { Title = "Product2", PriceForKg = 90.5 };

      Service service1 = new Service { Title = "Aaass", Price = 250.75 };
      Service service2 = new Service { Title = "Weeer", Price = 400.5 };
      Service service3 = new Service { Title = "Mmmmt", Price = 100.5 };

      IProduct[] items = new IProduct[] { pieceProduct1, pieceProduct2, weightProduct1, weightProduct2, service1, service2, service3 };

      Card card = new Card { Client = "Client1", Bonuses = 0, UsingService = false, Discount = 5 };

      double sum = 0;
      for (int i = 0; i < items.Length; i++)
      {
        sum += items[i].CountPriceWithSale(card);
      }

      Console.WriteLine("All price: ");
      Console.Write(sum);
      Console.WriteLine("\n\nBonuses: ");
      Console.Write(card.Bonuses);

      Card card2 = new Card { Client = "Client2", Bonuses = 0, UsingService = true, Discount = 15 };
      Service[] services = new Service[] { service1, service2, service3 };
      Array.Sort(services, new ServiceComparer());
      Console.WriteLine("\n\nSorted array: ");

      for (int i = 0; i < services.Length; i++)
      {
        Console.WriteLine(services[i]);
      }
    }

    
    public static void Main(string[] args)
    {
      Task1();
      Console.WriteLine("-------------------------------");
      Task2();
      Console.WriteLine("-------------------------------");
    }
  }
}
