using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using test_app.shared.Extensions;

namespace test_app.shared.Data
{
    public partial class Skin : BaseDataObject<Int64>
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

        public String SteamAnalystUrl { get; set; }

        public String SteamUrl { get; set; }

        [Column("Rarity")]
        public string RarityString
        {
            get { return Rarity.ToString(); }
            private set { Rarity = value.ParseEnum<Skin.SkinRarity>(); }
        }
        [NotMapped]
        public Skin.SkinRarity Rarity { get; set; }

        public ICollection<CasesDrop> CaseSkins { get; set; }

        public ICollection<StackCaseSkin> StackCaseSkins { get; set; }

        // Mapping
        internal class SkinConfiguration : DbEntityConfiguration<Skin, Int64>
        {
            public override void Configure(EntityTypeBuilder<Skin> entity)
            {
                entity.HasKey(x => x.Id);
                entity.HasIndex(p => new { p.Id, p.MarketHashName }).IsUnique();
            }
        }
    }
}
