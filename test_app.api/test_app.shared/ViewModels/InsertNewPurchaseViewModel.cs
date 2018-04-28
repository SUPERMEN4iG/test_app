using System;
using System.Collections.Generic;
using System.Text;

namespace test_app.shared.ViewModels
{
    public class InsertNewPurchaseViewModel
    {
        public string market_hash_name { get; set; }

        public Int64 queue_id { get; set; }

        public decimal price_usd { get; set; }

        public string platform { get; set; }

        public Int64 bot_id { get; set; }
    }

}
