using System;

namespace module_04
{
    class Service : IProduct
    {
        public string Title { get; set; }
        public double Price { get; set; }

        public override string ToString()
        {
            String result = "";

            result += Title;
            result += "\t";
            result += Price;
            result += "\t";

            return result;
        }

        public double CountPriceWithSale(Card card)
        {
            if (card.UsingService)
            {
                return Price * (1 - card.Discount / 100.0);
            }
            else { return Price; }
        }
    }
}