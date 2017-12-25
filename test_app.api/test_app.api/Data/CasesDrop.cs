using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test_app.api.Data
{
    public class CasesDrop : BaseDataObject<Int64>
    {
        public Int64 CaseId { get; set; }
        public Case Case { get; set; }

        public Int64 SkinId { get; set; }
        public Skin Skin { get; set; }

        public Decimal Chance { get; set; }
    }
}
