using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace test_app.api.Data
{
    public class Case : BaseDataObject<Int64>
    {
        public int Index { get; set; }

        public String StaticName { get; set; }

        public String FullName { get; set; }

        public CaseCategory Category { get; set; }

        public Decimal? PreviousPrice { get; set; }

        public Decimal Price { get; set; }

        public String Image { get; set; }

        public Boolean IsAvalible { get; set; }

        public virtual ICollection<CasesDrop> CaseSkins { get; set; }
    }
}
