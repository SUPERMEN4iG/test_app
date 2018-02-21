using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test_app.api.Data
{
    public class G2AIPNLog : BaseDataObject<Int64>
    {
        public string Request { get; set; }

        public string Response { get; set; }
    }
}
