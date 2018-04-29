using System;
using System.Collections.Generic;
using System.Text;
using test_app.shared.Data;

namespace test_app.shared.ViewModels
{
    public class LastWinnersViewModel
    {
        public string user_name { get; set; }

        public long winner_id { get; set; }

        public string skin_name { get; set; }

        public Skin.SkinRarity skin_rarity { get; set; }

        public string skin_image { get; set; }

        public string case_name { get; set; }

        public string case_static_name { get; set; }
    }

    public class WinnerViewModel
    {
        public string MarketHashName { get; set; }

        public decimal Price { get; set; }

        public string Image { get; set; }

        public Skin.SkinRarity Rarity { get; set; }

        public Skin Skin { get; set; }

        public DateTime DateCreate { get; set; }

        public Int64 Id { get; set; }
    }
}
