using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test_app.shared.Data
{
    public class CasesDrop : BaseDataObject<Int64>
    {
        public Int64 CaseId { get; set; }
        public virtual Case Case { get; set; }

        public Int64 SkinId { get; set; }
        public virtual Skin Skin { get; set; }

        public double Chance { get; set; }

        // Mapping
        internal class CasesDropConfiguration : DbEntityConfiguration<CasesDrop, Int64>
        {
            public override void Configure(EntityTypeBuilder<CasesDrop> entity)
            {
                entity.HasKey(t => new { t.CaseId, t.SkinId });

                entity
                    .HasOne(cs => cs.Skin)
                    .WithMany(c => c.CaseSkins)
                    .HasForeignKey(cs => cs.SkinId);

                entity
                    .HasOne(cs => cs.Case)
                    .WithMany(c => c.CaseSkins)
                    .HasForeignKey(cs => cs.CaseId);

                //entity
                    //.Property(x => x.Chance).HasColumnType("decimal(9, 8)");
            }
        }
    }
}
