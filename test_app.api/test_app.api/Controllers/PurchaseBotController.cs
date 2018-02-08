using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using test_app.api.Data;
using test_app.api.Logic;
using test_app.api.Logic.Bot;
using test_app.api.Models;

namespace test_app.api.Controllers
{
    [Produces("application/json")]
    [Route("api/purchasebot")]
    public class PurchaseBotController : Controller
    {
        private const int QUEUE_ITEMS_LIMIT_PER_REQUEST = 7;

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PurchaseBotController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [Route("getqueue")]
        [ClaimRequirement("Premission", "CanBotUses")]
        [HttpGet]
        public async Task<IActionResult> GetQueue(int botId)
        {
            if (!_context.Bots.Any(x => x.Id == botId))
            {
                return BadRequest(BaseHttpResult.GenerateError("bot not found", ResponseType.NotFound));
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var items = _context.PurshaseBotQueues
                       .Where(x => x.Locked == false)
                       .OrderBy(x => x.TriesCount)
                       .Take(QUEUE_ITEMS_LIMIT_PER_REQUEST)
                       .ToList();

                    items.ForEach((it) =>
                    {
                        it.Locked = true;
                        it.LastBot.Id = botId;
                        it.DateLastRequest = DateTime.Now;
                    });

                    _context.Update(items);
                    _context.SaveChanges();

                    transaction.Commit();

                    return Json(BaseHttpResult.GenerateSuccess(items, "ok"));
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest(BaseHttpResult.GenerateError(String.Format("server error: {0}", ex.Message), ResponseType.ServerError));
                }
            }
        }

        [Route("releaselocks")]
        [ClaimRequirement("Premission", "CanBotUses")]
        [HttpGet]
        public async Task<IActionResult> ReleaseLocks(string ids)
        {
            var idArray = ids.Split(',');

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var result = _context.PurshaseBotQueues.Where(x => ids.Contains(x.Id.ToString())).ToList();
                    result.ForEach((it) =>
                    {
                        it.Locked = false;
                    });

                    _context.Update(result);
                    _context.SaveChanges();
                    transaction.Commit();

                    return Json(BaseHttpResult.GenerateSuccess(idArray, "success"));
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest(BaseHttpResult.GenerateError(ex.Message, ResponseType.ServerError));
                }
            }
        }

        public class InsertNewPurchaseViewModel
        {
            public string market_hash_name { get; set; }

            public Int64 queue_id { get; set; }

            public decimal price_usd { get; set; }

            public string platform { get; set; }

            public Int64 bot_id { get; set; }
        }

        [Route("insertnewpurchase")]
        [ClaimRequirement("Premission", "CanBotUses")]
        [HttpPost]
        public async Task<IActionResult> InsertNewPurchase([FromBody] List<InsertNewPurchaseViewModel> list)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var botId = (long)HttpContext.Items.FirstOrDefault(x => x.Key == "botId").Value;

                    list.ForEach((it) =>
                    {
                        var added = new PurchasebotPurchases();
                        added.Bot.Id = botId;
                        added.MarketHashName = it.market_hash_name;
                        added.Platform = it.platform;
                        added.PriceUSD = it.price_usd;

                        var deleted = _context.PurshaseBotQueues.FirstOrDefault(x => x.Id == it.queue_id);

                        _context.Add(added);
                        _context.Remove(deleted);
                    });

                    _context.SaveChanges();

                    transaction.Commit();

                    return Json(BaseHttpResult.GenerateSuccess(botId, "success"));
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest(BaseHttpResult.GenerateError(ex.Message, ResponseType.ServerError));
                }
            }
        }

        public class SaveHistoryViewModel
        {
            public string market_hash_name { get; set; }

            public DateTime bought_at { get; set; }

            public DateTime listed_at { get; set; }

            public decimal price { get; set; }

            public string platform { get; set; }
        }

        [Route("savehistory")]
        [ClaimRequirement("Premission", "CanBotUses")]
        [HttpPost]
        public async Task<IActionResult> SaveHistory([FromBody] List<SaveHistoryViewModel> list)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var botId = (long)HttpContext.Items.FirstOrDefault(x => x.Key == "botId").Value;

                    list.ForEach((it) => 
                    {
                        var added = new BotsPurcasesFullHistory();
                        added.Bot.Id = botId;
                        added.MarketHashName = it.market_hash_name;
                        added.BoughtAt = it.bought_at;
                        added.Platform = it.platform;
                        added.Price = it.price;
                        added.ListedAt = it.listed_at;
                    });

                    _context.SaveChanges();

                    transaction.Commit();

                    return Json(BaseHttpResult.GenerateSuccess(botId, "success"));
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest(BaseHttpResult.GenerateError(ex.Message, ResponseType.ServerError));
                }
            }
        }
    }
}