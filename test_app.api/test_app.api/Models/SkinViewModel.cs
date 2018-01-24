namespace test_app.api.Models
{
    public class SkinViewModel
    {
        public string MarketHashName { get; set; }
        
        public long Id { get; set; }
        
        public double Chance { get; set; }
        
        public decimal Price { get; set; }
        
        public SkinViewModel(long id,string marketHashName,decimal price)
        {
            Id = id;
            MarketHashName = marketHashName;
            Price = price;
        }

        public SkinViewModel()
        {
        }
    }
}