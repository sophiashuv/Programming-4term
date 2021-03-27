using System;
using System.ComponentModel;

namespace Library_Sequences
{
    public class Order: IComparable<Order>
    {
        // Замовлення характеризується датою, облiковим номером книги,реєстрацiйним номером 
        // читача та ознакою виконання(так-нi).
        
        public Order(DateTime date, uint bookId, uint readerId, bool done)
        {
            Date = date;
            BookId = bookId;
            ReaderId = readerId;
            Done = done;
        }
        public Order()
        {
        }
        public DateTime Date { get; private set; }
        public uint BookId { get; private set; }
        public uint ReaderId { get; private set; }
        public bool Done { get; private set; }

        public int CompareTo(Order other)
        {
            return DateTime.Compare(Date, other.Date);
        }

        public override string ToString() {
            string res = "";
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this)) 
                res += ($"\t{prop.Name}: {prop.GetValue(this)}\n");
            return res;
        }
    }
}