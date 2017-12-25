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

        [Column("Rarity")]
        public string RarityString
        {
            get { return Rarity.ToString(); }
            private set { Rarity = value.ParseEnum<Skin.SkinRarity>(); }
        }
        [NotMapped]
        public Skin.SkinRarity Rarity { get; set; }

        public ICollection<Skin> Skins { get; set; }

        public Decimal Price { get; set; }
    }
}
