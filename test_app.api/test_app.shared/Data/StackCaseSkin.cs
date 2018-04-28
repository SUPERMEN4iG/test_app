using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test_app.shared.Data
{
    public class StackCaseSkin : BaseDataObject<Int64>
    {
        public Int64 StackCaseId { get; set; }
        public StackCase StackCase { get; set; }

        public Int64 SkinId { get; set; }
        public Skin Skin { get; set; }

        // Mapping
        internal class StackCaseSkinConfiguration : DbEntityConfiguration<StackCaseSkin, Int64>
        {
            public override void Configure(EntityTypeBuilder<StackCaseSkin> entity)
            {
                entity.HasKey(t => new { t.StackCaseId, t.SkinId });

                entity
                    .HasOne(cs => cs.Skin)
                    .WithMany(c => c.StackCaseSkins)
                    .HasForeignKey(cs => cs.SkinId);

                entity
                    .HasOne(cs => cs.Skin)
                    .WithMany(c => c.StackCaseSkins)
                    .HasForeignKey(cs => cs.SkinId);
            }
        }
    }
}
