using System;
using System.Collections.Generic;
using System.Text;
using test_app.shared.Data;

namespace test_app.shared.ViewModels
{
    public class WinnerStatisticViewModel
    {
        public Skin Skin { get; set; }

        public long Count { get; set; }

        public decimal Sum { get; set; }

        public decimal Chance { get; set; }
    }

    public class StatisticViewModel
    {
        public long TotalCount { get; set; }

        public decimal SumCase { get; set; }

        public decimal SumSkin { get; set; }

        public decimal Margine { get; set; }

        public List<WinnerStatisticViewModel> Drops { get; set; }
    }
}
