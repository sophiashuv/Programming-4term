using System;

namespace module_01
{
    public class Payment: ASC
    {
        public Payment(string marka, double price, double count, long cardNum) : base(marka, price, count)
        {
            CardNum = cardNum;
        }
        
        public Payment() : base()
        {
        }
        
        public static int Discount { get; set; }
        public long CardNum { get; set; }
        public override double AllPayment => Price * Count * (100 - Discount)/100;
    }
}