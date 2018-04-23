using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using test_app.api.Extensions;

namespace test_app.api.Data
{
    public class StackCase : BaseDataObject<Int64>
    {
        public String Image { get; set; }

        public String Name { get; set; }

        public ICollection<Skin> Skins { get; set; }

        public Decimal Price { get; set; }

        // Mapping
        internal class StackCaseConfiguration : DbEntityConfiguration<StackCase, Int64>
        {
            public override void Configure(EntityTypeBuilder<StackCase> entity)
            {
            }
        }
    }
}
