using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test_app.shared.Data
{
    public class Application : BaseDataObject<Int64>
    {
        public Boolean IsInitialized { get; set; }
        public String Name { get; set; }
        public Int64 DailyBonusCaseId { get; set; }
    }
}
