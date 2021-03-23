namespace module_04
{
    class WeightProduct : IProduct
    {
        public string Title { get; set; }
        public double PriceForKg { get; set; }
        public double CountPriceWithSale(Card card)
        {
            card.Bonuses += PriceForKg * (card.Discount / 100.0);
            return PriceForKg * (1 - card.Discount / 100.0);
        }
    }
}