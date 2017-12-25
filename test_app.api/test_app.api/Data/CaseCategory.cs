using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test_app.api.Data
{
    public class CaseCategory : BaseDataObject<Int64>
    {
        public String StaticName { get; set; }
    }
}