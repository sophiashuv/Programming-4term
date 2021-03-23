using System.ComponentModel.DataAnnotations;

namespace Polygon 
{
    public enum Color
    {
        Red, Blue, Green, Yellow, Pink
    }
    
    public class Polygon
    {
        private uint length;
        private Point[] points;
        private Color polygonColor;

        public Polygon()
        {
            length = 0;
            points = new Point[length];
            
        }
        
        public Polygon(Color _polygonColor, params Point[] _points)
        {
            polygonColor = _polygonColor;
            points = (Point[]) _points.Clone();
            length = (uint) points.Length;
        }
        
        public object Clone()
        {
            return new Polygon(polygonColor, points);
        }
        
        public override string ToString()
        {
            var res = "{ ";
            
            foreach (var point in points)
            {
                res += point + " ";
            }
            
            res += $": {polygonColor}";
            return res + " }";
        }

        public double GetPerimeter()
        {
            var p = 0.0;
            for (int i = 0; i < length - 1; i++)
            {
                p += points[i].Length(points[i + 1]);
            }
            p += points[length - 1].Length(points[0]);
            return p;
        }
    }
}