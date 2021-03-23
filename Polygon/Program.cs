using System;

namespace Polygon
{
    class Program
    {
        static void Main(string[] args)
        {
            var n = 4;
            var p1 = new Point(1, 2);
            var p2 = new Point(2, 3);
            var p3 = new Point(4, 5);
            var p4 = new Point(7, 8);
            var p5 = (Point)p4.Clone();
            var points = new Point[]{p1, p2, p3, p4, p5};
    
            foreach (var point in points)
            {
                Console.WriteLine(point);
            }

            var poly = new Polygon(Color.Blue, p1, p2, p3, p4);
            Console.WriteLine(poly);
            Console.WriteLine(poly.GetPerimeter());

        }
    }
}