namespace module_04
{
    class PieceProduct : IProduct
    {
        public string Title { get; set; }
        public double PriceForItem { get; set; }

        public double CountPriceWithSale(Card card)
        {
            return PriceForItem * (1 - card.Discount / 100.0);
        }
    }
}