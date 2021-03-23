using System;
using System.Collections.Generic;

namespace HomeWork03_Generics
{
    public class RoomComparerByVolume<T> : Comparer<Room<T>> where T: IShape
    {
        public override int Compare(Room<T> x, Room<T> y)
        {
            return x.Volume.CompareTo(y.Volume);
        }
    }
}