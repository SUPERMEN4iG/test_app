using System;
using System.Collections.Generic;
using System.Text;
using test_app.shared.Data;

namespace test_app.shared.ViewModels
{
    public class UserWinnerViewModel
    {
        public Int64 Id { get; set; }

        public DateTime DateCreate { get; set; }

        public string MarketHashName { get; set; }

        public string Image { get; set; }

        public Winner.WinnerState State { get; set; }

        public decimal Price { get; set; }
    }
}
