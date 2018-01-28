using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test_app.api.Models.ViewModels
{
    public class CaseViewModel
    {
        public Int64 Id { get; set; }

        public string StaticName { get; set; }

        public string FullName { get; set; }

        public string Image { get; set; }

        public decimal Price { get; set; }

        public decimal? PreviousPrice { get; set; }

        public int Index { get; set; }

        public string CategoryName { get; set; }

        public List<AdminSkinsViewModel> Skins { get; set; }
    }
}
