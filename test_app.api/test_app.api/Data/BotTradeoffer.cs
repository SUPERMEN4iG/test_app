using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test_app.api.Data
{
    public class BotTradeoffer : BaseTimestampsDataObject<Int64>
    {
        // TODO: Может быть строка?
        public Nullable<Int64> TradeofferId { get; set; }

        public Nullable<Int64> TradeId { get; set; }

        public Bot Bot { get; set; }

        public TradeOfferState TradeOfferState { get; set; }

        public Int64 SteamIdOther { get; set; }

        public Int64 ItemsToGive { get; set; }

        public DateTime DateExpiration { get; set; }

        public DateTime DateInsertion { get; set; }

        public virtual Stock StockItem { get; set; }
    }

    public enum TradeOfferState : Int32
    {
        Invalid = 1,
        Active = 2,
        Accepted = 3,
        Countered = 4,
        Expired = 5,
        Canceled = 6,
        Declined = 7,
        InvalidItems = 8,
        CreatedNeedsConfirmation = 9, // offer не был отправлен или находится на стадии подтверждения
        CanceledBySecondFactor = 10, // offer ожидает подтверждения mobile guard
        InEscrow = 11, // Tradeoffer приостановлен
    }
}
