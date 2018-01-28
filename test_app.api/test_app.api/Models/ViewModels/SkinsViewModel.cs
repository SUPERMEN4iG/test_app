using System;

namespace test_app.api.Models.ViewModels
{
    public class SkinsViewModel
    {
        public Int64 Id { get; set; }

        public string MarketHashName { get; set; }

        public string Image { get; set; }

        public decimal Price { get; set; }
    }

    public class AdminSkinsViewModel : SkinsViewModel
    {
        public decimal Chance { get; set; }
    }
}