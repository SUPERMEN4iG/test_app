using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test_app.api.Data
{
    public class Stock : BaseDataObject<Int64>
    {
        public Bot Bot { get; set; }

        public Skin Skin { get; set; }

        public Decimal Price { get; set; }
    }
}
