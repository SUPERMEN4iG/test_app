using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test_app.api.Data
{
    public class BotsPurcasesFullHistory : BaseDataObject<Int64>
    {
        public string MarketHashName { get; set; }

        public DateTime BoughtAt { get; set; }

        public DateTime ListedAt { get; set; }

        public decimal Price { get; set; }

        public string Platform { get; set; }

        public Bot Bot { get; set; }

        // Mapping
        internal class BotsPurcasesFullHistoryConfiguration : DbEntityConfiguration<BotsPurcasesFullHistory, Int64>
        {
            public override void Configure(EntityTypeBuilder<BotsPurcasesFullHistory> entity)
            {
            }
        }
    }
}
