using System;

namespace HomeWork03_Generics
{
    public class Rectangle: IShape
    {
        // Rectangle(double a, double b)
        // {
        //     A = a;
        //     B = b;
        // }
        public double A { get; set; }
        public double B { get; set; }
        public double Area() => A * B;
        
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            var b = obj as Rectangle;
            return this.Area().CompareTo(b.Area());
        }

        public object Clone() => new Rectangle { A = this.A, B = this.B };
    }
}