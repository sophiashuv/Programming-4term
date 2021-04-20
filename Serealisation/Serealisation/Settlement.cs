using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Xml.Serialization;

namespace Serealisation
{
    [XmlInclude(typeof(City))]
    [XmlInclude(typeof(Village))]
    [Serializable]
    public abstract class Settlement
    {
        public string Name { get; set;}
        public double Area { get; set;}
        
        public override string ToString()
        {
            string res = this.GetType().Name + "\n";
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this))
                res += ($"\t{prop.Name}: {prop.GetValue(this)}\n");
            return res;
        }

    }
    
    public class City: Settlement
    {
        
    }
    public class Village: Settlement
    {
        
    }
    [Serializable]
    public class Collection: IEnumerable<Settlement>
    {
        public Collection() => Lst = new List<Settlement>();
        public List<Settlement> Lst { get; set; }
        
        public override string ToString()
        {
            var res = "";
            foreach (var r in Lst)
                res += "\t" + r.ToString() + "\n";
            return res;
        }
        
        public static bool IsTriangle(Settlement r) => r.GetType().Name == typeof(Settlement).Name;
        public IEnumerator<Settlement> GetEnumerator() => Lst.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        
        public void SortByPerimeter() => Lst.OrderBy(x => x.Name);

        public void Add(Settlement item)
        {
            Lst.Add(item);
        }

    }

}