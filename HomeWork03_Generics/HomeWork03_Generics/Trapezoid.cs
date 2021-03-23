namespace HomeWork03_Generics
{
    public class Trapezoid: IShape
    {
        // Trapezoid(double a, double b)
        // {
        //     A = a;
        //     B = b;
        // }
        public double A { get; set; }
        public double B { get; set; }
        public double H { get; set; }
        
        public double Area() => H * (A + B) / 2;
        
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            var b = obj as Trapezoid;
            return this.Area().CompareTo(b.Area());
        }
        
        public object Clone() => new Trapezoid { A = this.A, B = this.B, H = this.H};
    }
}