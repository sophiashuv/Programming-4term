using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace LINQ_5
{
  internal class Program
  {
  
    // Завдання 1.Розробити типи для змагання з бiгу по маршруту, який складається зтрьох етапiв. 
    //   Бiгун характеризується iменем i прiзвищем, а також номером.Результат на етапi х
    //   арактеризується номером бiгуна, часом у секундах, заякий спортсмен подолав етап, 
    //   номером етапу i його протяжнiстю (в км). Вiдомо, що не всi добiгли до фiнiшу. 
    //   Iнформацiя про бiгунiв i результати зетапiв подано окремими xml-файлами. 
    //   Отримати з використанням linq:
    //    1. на консолi iмена i прiзвища трьох бiгунiв,  якi очолювали бiг на ко-жному етапi;
    //    2. xml-файл у форматi: прiзвище бiгуна, їхнiй загальний час; врахуватилише тих
    //       бiгунiв, якi пробiгли усi етапи маршруту, i впорядкувати вiдпо-вiдно до рейтингу за
    //       загальним часом;
    //    3. csv-файл у форматi: прiзвище бiгуна, номер етапу, де вiн мав найбiль-шу середню
    //       швидкiсть, i величина цiєї швидкостi;
    //   вивести данi для кожногобiгуна.
      
    
    public static void read_from_txt<T>(List<T> container, string fileName) 
      where T :  new()
    {
     
      using(var reader = new StreamReader(fileName))
      {
        var atters = reader.ReadLine().Split(',');
        while (!reader.EndOfStream)
        {
          var values = reader.ReadLine().Split(',');
          var asc = new T();
          for (int i = 0; i < atters.Length; i++)
          {
            PropertyInfo propertyInfo = asc.GetType().GetProperty(atters[i]);
            var v = propertyInfo.Name == "Etp" ? (Etap) Enum.Parse(typeof(Etap), values?[i], true): 
              Convert.ChangeType(values[i], propertyInfo.PropertyType);
            propertyInfo.SetValue(asc, v, null);
          }
          container.Add(asc);
        }
      }
    }
    
    public static void write_toXML<T>(List<T> studentList, string xf)
    {
      var xmldict = new XmlSerializer(typeof(List<T>));
      using (var file = new FileStream(xf, FileMode.OpenOrCreate))
      {
        xmldict.Serialize(file, studentList);
      }
    }
    

    public static List<T> read_fromXML<T>(string xf)where T :  new()
    {
      var xmldict = new XmlSerializer(typeof(List<T>));
      using (var file = new FileStream(xf, FileMode.OpenOrCreate) )
      {
        var newcust = xmldict.Deserialize(file) as List<T>;
        return newcust;
      }
    }

    //  1. на консолi iмена i прiзвища трьох бiгунiв,  якi очолювали бiг на ко-жному етапi;
    public static void Task1(List<Runner> runners, List<EtapResults> results)
    {
      var joined_res = from res in results
        from run in runners 
        where res.RunnerId == run.Id && (int)res.Etp == res.Length
        select new {Etp = res.Etp, Time = res.Time, Runner = run.FullName};

      var final_res = from j in joined_res
        group j by j.Etp
        into grouped
        select new
        {
          Etp = grouped.Key,
          MaxRunner = grouped.Where(el => el.Time == grouped.Min(k => k.Time)).Select(el => el.Runner)
        };

      Console.WriteLine("Task 1");
      foreach (var v in final_res)
      {
        var st = String.Join(" ", v.MaxRunner);
        Console.WriteLine($"{v.Etp}: {st}");
      }
    }
    
    
    //    2. xml-файл у форматi: прiзвище бiгуна, їхнiй загальний час; врахуватилише тих
    //       бiгунiв, якi пробiгли усi етапи маршруту, i впорядкувати вiдпо-вiдно до рейтингу за
    //       загальним часом;
    public static void Task2(List<Runner> runners, List<EtapResults> results)
    {
      var result = from run in runners
        from res in results
        where run.Id == res.RunnerId && (int)res.Etp == res.Length
        select new {Runner = run, Result = res};

      var res2 = new XElement("TotalTime", from res in result
        group res by res.Runner.FullName
        into grouped
        where grouped.Count() == 3
        let s = grouped.Sum(k => k.Result.Time)
        orderby s 
        select new XElement("Result", new XElement("Runner", grouped.Key), new XElement("Time", s)));
      
      
      var xmldict = new XmlSerializer(typeof(XElement));
      using (var file = new FileStream("/Users/sophiyca/RiderProjects/LINQ_5/LINQ_5/result.XML", FileMode.OpenOrCreate))
      {
        xmldict.Serialize(file, res2);
      }

    }
    
    //    3. csv-файл у форматi: прiзвище бiгуна, номер етапу, де вiн мав найбiль-шу середню
    //       швидкiсть, i величина цiєї швидкостi;
    public static void Task3(List<Runner> runners, List<EtapResults> results)
    {
      var query = from run in runners
        join res in results on run.Id equals res.RunnerId into joined
        let maxVelocity = joined.Max(e => e.Speed)
        let etp = joined.Where(e => e.Speed == maxVelocity).Select(el => el.Etp)
        select new {Runner = run.Surname, Etp = etp, Speed = maxVelocity};

      using (StreamWriter writer = new StreamWriter("/Users/sophiyca/RiderProjects/LINQ_5/LINQ_5/task3.csv"))
      {
        writer.WriteLine($"Runner,Speed,Etp");
        foreach (var v in query)
        {
          foreach (var vv in v.Etp)
            writer.WriteLine($"{v.Runner},{v.Speed},{vv}");
        }
      }
    }

    public static void Main(string[] args)
    {
      var runnes = new List<Runner>();
      var results = new List<EtapResults>();
      read_from_txt(runnes, "/Users/sophiyca/RiderProjects/LINQ_5/LINQ_5/runners.csv");
      read_from_txt(results, "/Users/sophiyca/RiderProjects/LINQ_5/LINQ_5/results.csv");
      
      write_toXML(runnes, "/Users/sophiyca/RiderProjects/LINQ_5/LINQ_5/runners.XML");
      write_toXML(results, "/Users/sophiyca/RiderProjects/LINQ_5/LINQ_5/results.XML");
      var all_runnes = read_fromXML<Runner>("/Users/sophiyca/RiderProjects/LINQ_5/LINQ_5/runners.XML");
      var all_results = read_fromXML<EtapResults>("/Users/sophiyca/RiderProjects/LINQ_5/LINQ_5/results.XML");
      
      Console.WriteLine(String.Join("\n", all_runnes));
      Console.WriteLine(String.Join("\n", all_results));

      Task1(all_runnes, all_results);
      Task2(all_runnes, all_results);
      Task3(all_runnes, all_results);
    }
  }
}