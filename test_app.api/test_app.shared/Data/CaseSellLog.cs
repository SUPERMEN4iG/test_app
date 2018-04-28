using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test_app.shared.Data
{
    public class CaseSellLog : BaseDataObject<Int64>
    {
        public ApplicationUser User { get; set; }

        public Winner Winner { get; set; }

        public Skin Skin { get; set; }

        public Decimal Price { get; set; }

        public String Source { get; set; }

        // Mapping
        internal class CaseSellLogConfiguration : DbEntityConfiguration<CaseSellLog, Int64>
        {
            public override void Configure(EntityTypeBuilder<CaseSellLog> entity)
            {
            }
        }
    }
}
