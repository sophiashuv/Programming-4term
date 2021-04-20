using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace Serealisation
{
    internal class Program
    {
            // На основі базового класу НАСЕЛЕНИЙ ПУНКТ створити похідні класи МІСТО і СЕЛО
            // Створити клас-колекцію об’єктів базового класу, заповнити об’єктами похідних класів. 
            // Впорядкувати за одним із критеріїв. Здійснити серіалізацію та десеріалізацію цієї 
            // колекції у двійковому та xml-форматі. Передбачити обробку виключних ситуацій.


        public static void read_fromXML(Collection collection)
        {
            var xf = "/Users/sophiyca/RiderProjects/Serealisation/Serealisation/settlements.xml";
            var xmldict = new XmlSerializer(typeof(Collection));
            using (var file = new FileStream(xf, FileMode.OpenOrCreate))
            {
                xmldict.Serialize(file, collection);
            }
            
            using (var file = new FileStream(xf, FileMode.OpenOrCreate) )
            {
                var newcust = xmldict.Deserialize(file) as Collection;
                Console.WriteLine(newcust);
            }
        }
        
        public static void read_fromBinary(Collection collection)
        {
            var bf = "/Users/sophiyca/RiderProjects/Serealisation/Serealisation/settlements2.bin";
          
            var formatter = new BinaryFormatter();
            using (var file = new FileStream(bf, FileMode.OpenOrCreate))
            {
                formatter.Serialize(file, collection);
            }
            
            using (var file = new FileStream(bf, FileMode.OpenOrCreate) )
            {
                var newcust = formatter.Deserialize(file) as Collection;
                Console.WriteLine(newcust);
            }
        }

        
        
        public static void Main(string[] args)
        {
            Settlement s1 = new City {Name = "Lviv", Area = 183.01};
            Settlement s2 = new City {Name = "Kyiv", Area = 839};
            Settlement s3 = new Village {Name = "Synevir", Area = 135};
            Settlement s4 = new City {Name = "Kharkiv", Area = 350};
            Settlement s5 = new Village {Name = "Chervone", Area = 89};

            List<Settlement> lst = new List<Settlement>{s1, s2, s3, s4, s5};
            var collection = new Collection {Lst = lst};
            
            // Console.WriteLine(collection);
            // read_fromXML(collection);
            read_fromBinary(collection);
            // var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//SerializationOverview.xml"; 
        }
    }
}