using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using test_app.api.Models;

namespace test_app.api.Data
{
    public class Winner : BaseDataObject<Int64>
    {
        public ApplicationUser User { get; set; }

        public Boolean IsSold { get; set; }

        public Boolean IsSent { get; set; }

        public Case Case { get; set; }

        public Skin Skin { get; set; }
    }
}
