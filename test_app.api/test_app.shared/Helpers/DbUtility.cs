using System;
using System.Collections.Generic;
using System.Text;

namespace test_app.shared.Helpers
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
