using System;
using System.Collections.Generic;
using System.Text;

namespace test_app.shared.ViewModels
{
    public class TestOpenCaseViewModel
    {
        public class TestOpenCaseTotalsViewModel
        {
            public decimal TotalCasePrice { get; set; }

            public decimal TotalSkinPrice { get; set; }
        }

        public TestOpenCaseTotalsViewModel Totals { get; set; } = new TestOpenCaseTotalsViewModel();

        public List<object> Result { get; set; } = new List<object>();
    }
}
