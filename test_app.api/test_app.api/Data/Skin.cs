using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test_app.api.Data
{
    public class Skin : BaseDataObject<Int64>
    {
        public enum SkinRarity
        {
            Covert,
            Milspec,
            Classified,
            Rare
        }

        public String MarketHashName { get; set; }

        public Decimal Price { get; set; }

        public String Image { get; set; }

        public ICollection<CasesDrop> CaseSkins { get; set; }

        public ICollection<StackCaseSkin> StackCaseSkins { get; set; }
    }
}
