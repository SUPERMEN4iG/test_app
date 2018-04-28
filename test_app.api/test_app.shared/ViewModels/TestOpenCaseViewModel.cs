using System;
using System.Collections.Generic;
using System.Text;

namespace test_app.shared.ViewModels
{
    public class TestOpenCaseViewModel
    {
        public class TestOpenCaseTotalsViewModel
        {
            public double TotalCasePrice { get; set; }

            public double TotalSkinPrice { get; set; }
        }

        public TestOpenCaseTotalsViewModel Totals { get; set; } = new TestOpenCaseTotalsViewModel();

        public List<object> Result { get; set; } = new List<object>();
    }
}
