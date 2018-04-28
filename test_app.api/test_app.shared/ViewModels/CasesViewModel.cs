using System;
using System.Collections.Generic;
using System.Text;

namespace test_app.shared.ViewModels
{
    public class SkinsViewModel
    {
        public Int64 Id { get; set; }

        public string MarketHashName { get; set; }

        public string Image { get; set; }

        public decimal Price { get; set; }
    }

    public class AdminSkinsViewModel : SkinsViewModel
    {
        public double Chance { get; set; }
    }

    public class CasesViewModel
    {
        public Int64 Id { get; set; }

        public string StaticName { get; set; }

        public string FullName { get; set; }

        public string Image { get; set; }

        public decimal Price { get; set; }

        public decimal? PreviousPrice { get; set; }

        public int Index { get; set; }

        public string CategoryName { get; set; }

        public List<SkinsViewModel> Skins { get; set; }
    }

    public class CaseCategoryViewModel
    {
        public class CategoryViewModel
        {
            public Int64 Id { get; set; }

            public String StaticName { get; set; }

            public String FullName { get; set; }

            public Int32 Index { get; set; }
        }

        public CategoryViewModel Category { get; set; }

        public IList<CasesViewModel> Cases { get; set; }
    }
}
