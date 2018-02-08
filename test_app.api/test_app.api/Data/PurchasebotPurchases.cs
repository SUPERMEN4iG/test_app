using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test_app.api.Data
{
    public class PurchasebotPurchases : BaseDataObject<Int64>
    {
        public PurshaseBotQueue BotQueue { get; set; }

        public string MarketHashName { get; set; }

        public decimal PriceUSD { get; set; }

        public string Platform { get; set; }

        public Bot Bot { get; set; }
    }
}
