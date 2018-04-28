using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test_app.shared.Data
{
    public class CaseDiscount : BaseDataObject<Int64>
    {
        public ApplicationUser User { get; set; }

        public Case Case { get; set; }

        public Decimal Percent { get; set; }

        // Mapping
        internal class CaseDiscountConfiguration : DbEntityConfiguration<CaseDiscount, Int64>
        {
            public override void Configure(EntityTypeBuilder<CaseDiscount> entity)
            {
            }
        }
    }
}
