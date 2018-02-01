using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using test_app.api.Data;

namespace test_app.api.Models.ViewModels
{
    public class StatisticViewModel
    {
        public Case Case { get; set; }

        public DateTime Date { get; set; }

        public Winner.WinnerState State { get; set; }

        public int Count { get; set; }

        public decimal Sum { get; set; }
    }

    public class AdminStatisticResultViewModel 
    {
        public Case Case { get; set; }

        public AdminStatisticViewModel Statistic { get; set; }
    }

    public class AdminStatisticViewModel
    {
        public StatisticViewModel Traded { get; set; }

        public StatisticViewModel Returned { get; set; }

        public StatisticViewModel Total { get; set; }

        public StatisticViewModel TotalToWithdraw { get; set; }
    }
}
