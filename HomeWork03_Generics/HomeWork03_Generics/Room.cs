using System;
using System.Configuration;

namespace HomeWork03_Generics
{
    public class Room<T> where T: IShape, ICloneable, IComparable
    {
        public double Height { get; set; }
        
        public T Floor { get; set; }

        public double Volume => Height * Floor.Area();
        
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            var b = obj as Room<T>;
            return this.Floor.CompareTo(b.Floor);
        }
        
        public object Clone() => new Room<T> { Height = this.Height, Floor = this.Floor };
        
        public bool isRectangle()
        {
            return Floor.GetType().ToString() == "HomeWork03_Generics.Rectangle";
        }
        public override string ToString()
        {
            var res = Floor.GetType().ToString();
            return res;
        }
        
    }
}