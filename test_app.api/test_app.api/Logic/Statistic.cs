using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test_app.api.Logic
{
    #region for statistic action
    public class AnonStatValue
    {
        public int DayRange { get; set; }

        public int Count { get; set; }

        public decimal? Sum { get; set; }
    }

    public class AnonStat
    {
        public enum StatState
        {
            None = 0,
            Sold = 1,
            Traded = 2,
            Total = 3,
            TotalToWithDraw = 4
        }

        public StatState State { get; set; }

        public List<AnonStatValue> Values { get; set; }
    }

    public class AnonStatCompare : IEqualityComparer<AnonStat>
    {
        public bool Equals(AnonStat x, AnonStat y)
        {
            return x.State == y.State;
        }
        public int GetHashCode(AnonStat codeh)
        {
            return codeh.State.GetHashCode();
        }
    }

    public class AnonStatValueCompare : IEqualityComparer<AnonStatValue>
    {
        public bool Equals(AnonStatValue x, AnonStatValue y)
        {
            return x.DayRange == y.DayRange;
        }
        public int GetHashCode(AnonStatValue codeh)
        {
            return codeh.DayRange.GetHashCode();
        }
    }
    #endregion
}
