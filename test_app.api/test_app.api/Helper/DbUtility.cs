using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test_app.api.Helper
{
    public static class DbUtility
    {
        public static int DateDiff(string diffType, DateTime startDate, DateTime endDate)
        {
            var comp = (int)Math.Abs(Math.Floor((startDate - endDate).TotalDays));
            return comp;
        }
    }
}
