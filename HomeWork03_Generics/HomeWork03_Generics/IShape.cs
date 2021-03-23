using System;

namespace HomeWork03_Generics
{
    public interface IShape: IComparable, ICloneable
    {
        double Area();
    }
}