using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HomeWork03_Generics
{
    public class Building: IEnumerable
    {
        public List<Room<IShape>> Rooms{ get; set; }
        
        static bool IsRectangle(Room<IShape> r)
        {
            return r.Floor.GetType().ToString() == "HomeWork03_Generics.Rectangle";
        }
        
        public IEnumerator GetEnumerator(){
            return new FilteredEnumerator<Room<IShape>>(Rooms, IsRectangle);
        }
        
        public override string ToString()
        {
            var res = "";
            foreach (var r in Rooms)
            {
                res += r.ToString() + "\n";
            }
            return res;
        }
    }
    
    public class FilteredEnumerator<T> : System.Collections.Generic.IEnumerator<T>
    {
        readonly System.Collections.Generic.IEnumerator<T> enumerator;
        readonly System.Func<T, bool> filter;

        public FilteredEnumerator(System.Collections.Generic.IEnumerable<T> enumerable, System.Func<T, bool> filter=null)
        {
            if (enumerable == null)
                throw new System.ArgumentNullException();
            this.enumerator = enumerable.GetEnumerator();
            this.filter = filter;
        }
        public T Current => enumerator.Current;

        public void Dispose() => enumerator.Dispose();

        object System.Collections.IEnumerator.Current => Current;

        public bool MoveNext()
        {
            while (enumerator.MoveNext())
                if (filter == null || filter(enumerator.Current))
                    return true;
            return false;
        }
        
        public void Reset() => enumerator.Reset();
    }
}