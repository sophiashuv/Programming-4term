namespace module_01
{
    public class ToFullBank: ASC
    {
        public ToFullBank(string marka, double price, double count, int discount) : base(marka, price, count)
        {
        }
        
        public ToFullBank() : base()
        {
        }
        
        public static int Discount { get; set; }

        public override double AllPayment => Price * Count * (100 - Discount)/100;
    }
}