using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test_app.api.Data
{
    public class Bot : BaseDataObject<Int64>
    {
        public String Login { get; set; }

        public String SteamId { get; set; }

        public String Server { get; set; }

        public DateTime SyncTime { get; set; }

        public Boolean IsAdminsOnly { get; set; }

        public Boolean IsAwaiting { get; set; }

        public Boolean IsHidden { get; set; }

        public string Token { get; set; }

        /// <summary>
        /// URL трейдоффера
        /// </summary>
        public String TradeOffer { get; set; }

        // Mapping
        internal class BotConfiguration : DbEntityConfiguration<Bot, Int64>
        {
            public override void Configure(EntityTypeBuilder<Bot> entity)
            {
            }
        }
    }
}
