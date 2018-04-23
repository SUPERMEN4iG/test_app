using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test_app.api.Data
{
    public class CaseCategory : BaseDataObject<Int64>
    {
        public String StaticName { get; set; }
        public String FullName { get; set; }
        public Int32 Index { get; set; }
        public virtual List<Case> Cases { get; set; }

        // Mapping
        internal class CaseCategoryConfiguration : DbEntityConfiguration<CaseCategory, Int64>
        {
            public override void Configure(EntityTypeBuilder<CaseCategory> entity)
            {
            }
        }
    }
}