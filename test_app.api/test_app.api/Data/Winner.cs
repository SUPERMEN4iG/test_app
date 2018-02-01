using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using test_app.api.Models;

namespace test_app.api.Data
{
    public class Winner : BaseDataObject<Int64>
    {
        public enum WinnerState
        {
            None = 0,
            Sold = 1,
            Traded = 2
        }

        public ApplicationUser User { get; set; }

        public Case Case { get; set; }

        public Skin Skin { get; set; }

        public Stock Stock { get; set; }

        public Decimal? Price { get; set; }

        public WinnerState State { get; set; } = WinnerState.None;
    }
}
