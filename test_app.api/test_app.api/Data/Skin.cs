using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using test_app.api.Extensions;
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
    }
}
