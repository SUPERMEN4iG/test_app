using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test_app.api.Models.ViewModels
{
    public class CategoryViewMode
    {
        public Int64 Id { get; set; }

        public int Index { get; set; }

        public string StaticName { get; set; }

        public string FullName { get; set; }
    }

    public class CasesCategoryViewModel
    {
        public List<CaseViewModel> Cases { get; set; }

        public CategoryViewMode Category { get; set; }
    }

    public class AdminGetCasesDataModel
    {
        public List<CasesCategoryViewModel> Cases { get; set; }

        public List<AdminSkinsViewModel> Skins { get; set; }
    }
}
