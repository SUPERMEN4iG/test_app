using System;
using System.Collections.Generic;
using System.Text;

namespace test_app.shared.ViewModels
{
    public class SuperSkinViewModel : SkinsViewModel
    {
        public decimal? Sum { get; set; }

        public int Count { get; set; }

        public float Chance { get; set; }
    }

    public class SkinViewModel
    {
        public string MarketHashName { get; set; }

        public long Id { get; set; }

        public double Chance { get; set; }

        public decimal Price { get; set; }

        public SkinViewModel(long id, string marketHashName, decimal price)
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
