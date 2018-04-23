using Microsoft.EntityFrameworkCore.Metadata.Builders;
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

        public virtual List<BotTradeoffer> Tradeoffers { get; set; }

        // Mapping
        internal class StockConfiguration : DbEntityConfiguration<Stock, Int64>
        {
            public override void Configure(EntityTypeBuilder<Stock> entity)
            {
            }
        }
    }
}
