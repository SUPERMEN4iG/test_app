using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using test_app.api.Logic;
using test_app.api.Logic.Bot;
using test_app.api.Models;
using test_app.shared;
using test_app.shared.Data;
using test_app.shared.Repositories;
using test_app.shared.ViewModels;

namespace test_app.api.Controllers
{
    [Produces("application/json")]
    [Route("api/purchasebot")]
    public class PurchaseBotController : Controller
    {
        private const int QUEUE_ITEMS_LIMIT_PER_REQUEST = 7;

        private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBotRepository _botRepository;

        public PurchaseBotController(
            UserManager<ApplicationUser> userManager,
            IUnitOfWork<ApplicationDbContext> unitOfWork,
            IBotRepository botRepository)
        {
            _userManager = userManager;
            _botRepository = botRepository;
            _unitOfWork = unitOfWork;
        }

        [Route("getqueue")]
        [ClaimRequirement("Premission", "CanBotUses")]
        [HttpGet]
        public IActionResult GetQueue(int count)
        {
            using (var transaction = _unitOfWork.Context.Database.BeginTransaction())
            {
                try
                {
                    var botId = (long)HttpContext.Items.FirstOrDefault(x => x.Key == "botId").Value;

                    if (!_botRepository.Any(botId))
                    {
                        throw new Exception("bot not found");
                    }

                    var items = _botRepository.GetQueue(count);

                    items.ForEach((it) =>
                    {
                        it.TriesCount += 1;
                        it.LastBot.Id = botId;
                        it.DateLastRequest = DateTime.Now;
                    });

                    _unitOfWork.Context.UpdateRange(items);
                    _unitOfWork.Context.SaveChanges();

                    transaction.Commit();

                    return Json(BaseHttpResult.GenerateSuccess(items, "ok"));
                }
                catch (Exception ex)
                {
                    if (ex.GetType() == typeof(SqlException))
                    {
                        transaction.Rollback();
                    }
                    
                    return BadRequest(BaseHttpResult.GenerateError(String.Format("server error: {0}", ex.Message), ResponseType.ServerError));
                }
            }
        }

        //[Route("releaselocks")]
        //[ClaimRequirement("Premission", "CanBotUses")]
        //[HttpGet]
        //public async Task<IActionResult> ReleaseLocks(string ids)
        //{
        //    var idArray = ids.Split(',');

        //    using (var transaction = _context.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            var botId = (long)HttpContext.Items.FirstOrDefault(x => x.Key == "botId").Value;
        //            var result = _context.PurshaseBotQueues.Where(x => ids.Contains(x.Id.ToString())).ToList();
        //            result.ForEach((it) =>
        //            {
        //                it.Locked = false;
        //                _context.Update(it);
        //            });

        //            _context.SaveChanges();
        //            transaction.Commit();

        //            return Json(BaseHttpResult.GenerateSuccess(idArray, "success"));
        //        }
        //        catch (Exception ex)
        //        {
        //            transaction.Rollback();
        //            return BadRequest(BaseHttpResult.GenerateError(ex.Message, ResponseType.ServerError));
        //        }
        //    }
        //}

        [Route("insertnewpurchase")]
        [ClaimRequirement("Premission", "CanBotUses")]
        [HttpPost]
        public IActionResult InsertNewPurchase([FromBody] List<InsertNewPurchaseViewModel> list)
        {
            using (var transaction = _unitOfWork.Context.Database.BeginTransaction())
            {
                try
                {
                    var botId = (long)HttpContext.Items.FirstOrDefault(x => x.Key == "botId").Value;

                    var deleted = _botRepository.GetQueue(list);

                    var added = list.Select(x => new PurchasebotPurchases
                    {
                        Bot = new Bot() { Id = botId },
                        MarketHashName = x.market_hash_name,
                        Platform = x.platform,
                        PriceUSD = x.price_usd
                    });

                    _unitOfWork.Context.AddRange(added);
                    _unitOfWork.Context.RemoveRange(deleted);

                    _unitOfWork.Context.SaveChanges();

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

        //[Route("savehistory")]
        //[ClaimRequirement("Premission", "CanBotUses")]
        //[HttpPost]
        //public async Task<IActionResult> SaveHistory([FromBody] List<SaveHistoryViewModel> list)
        //{
        //    using (var transaction = _context.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            var botId = (long)HttpContext.Items.FirstOrDefault(x => x.Key == "botId").Value;

        //            list.ForEach((it) => 
        //            {
        //                var added = new BotsPurcasesFullHistory();
        //                added.Bot.Id = botId;
        //                added.MarketHashName = it.market_hash_name;
        //                added.BoughtAt = it.bought_at;
        //                added.Platform = it.platform;
        //                added.Price = it.price;
        //                added.ListedAt = it.listed_at;
        //                _context.Add(added);
        //            });

        //            _context.SaveChanges();

        //            transaction.Commit();

        //            return Json(BaseHttpResult.GenerateSuccess(botId, "success"));
        //        }
        //        catch (Exception ex)
        //        {
        //            transaction.Rollback();
        //            return BadRequest(BaseHttpResult.GenerateError(ex.Message, ResponseType.ServerError));
        //        }
        //    }
        //}
    }
}