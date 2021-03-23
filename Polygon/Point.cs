using System;
using System.Threading;

namespace Polygon
{
    public class Point: ICloneable
    {
        private double x;
        private double y; 
        
        public Point()
        {
            x = 0;
            y = 0;
        }
        
        public Point(double _x, double _y)
        {
            x = _x;
            y = _y;
        }
        
        public override string ToString() => ($"({x}, {y})");
        
        public object Clone()
        {
            return new Point(x, y);
        }

        public double Length(Point p) => Math.Sqrt((x - p.x) * (x - p.x) + (y - p.y)*(y - p.y));
        
    }
}