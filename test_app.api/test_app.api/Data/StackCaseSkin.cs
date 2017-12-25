using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test_app.api.Data
{
    public class StackCaseSkin : BaseDataObject<Int64>
    {
        public Int64 StackCaseId { get; set; }
        public StackCase StackCase { get; set; }

        public Int64 SkinId { get; set; }
        public Skin Skin { get; set; }
    }
}
