using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using test_app.api.Logic;
using test_app.api.Logic.Bot;
using test_app.api.Logic.Extensions;
using test_app.shared;
using test_app.shared.Data;
using test_app.shared.Repositories;
using test_app.shared.ViewModels;

namespace test_app.api.Controllers
{
    [Produces("application/json")]
    [Route("api/steambot")]
    public class SteamBotController : Controller
    {
        private readonly IBotRepository _botRepository;
        private readonly IStockRepository _stockRepository;
        private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;

        public SteamBotController(
            IUnitOfWork<ApplicationDbContext> unitOfWork,
            IStockRepository stockRepository,
            IBotRepository botRepository)
        {
            _botRepository = botRepository;
            _unitOfWork = unitOfWork;
            _stockRepository = stockRepository;
        }

        [Route("ping")]
        [HttpGet]
        public IActionResult Ping()
        {
            using (var transaction = _unitOfWork.Context.Database.BeginTransaction())
            {
                try
                {
                    string ip = "";
                    ip = this.HttpContext.GetIpAddress();

                    var bot = _botRepository.Get(x => x.Server == ip).FirstOrDefault();

                    if (bot == null) throw new Exception("bot not found");

                    bot.SyncTime = DateTime.Now;

                    _botRepository.Update(bot);
                    _unitOfWork.SaveChanges();
                    transaction.Commit();

                    return Json(BaseHttpResult.GenerateSuccess(new { ip }, "ok"));
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return Json(BaseHttpResult.GenerateError(ex.Message.ToString(), ResponseType.ServerError));
                }
            }
        }

        public class UpdateInventoryRequestViewModel
        {
            public class InventoryViewModel
            {
                public int id { get; set; }

                public string market_hash_name { get; set; }
            }

            public int app_id { get; set; }

            public List<InventoryViewModel> items { get; set; }
        }

        public class UpdateInventoryResponseViewModel
        {
            public int item_id { get; set; }

            public string status { get; set; }

            public string error { get; set; }
        }

        [Route("updateinventory")]
        [HttpPost]
        [ClaimRequirement("Premission", "CanBotUses")]
        public IActionResult UpdateInventory([FromBody]UpdateInventoryRequestViewModel requestData)
        {
            using (var transaction = _unitOfWork.Context.Database.BeginTransaction())
            {
                try
                {
                    long botId = (long)HttpContext.Items.FirstOrDefault(x => x.Key == "botId").Value;
                    var bot = _botRepository.Get(x => x.Id == botId).FirstOrDefault();

                    var results = new ConcurrentQueue<UpdateInventoryResponseViewModel>();

                    Parallel.ForEach(requestData.items, (item) =>
                    {
                        try
                        {
                            var stock = new Stock()
                            {
                                Skin = new Skin() { Id = item.id },
                                Bot = new Bot() { Id = botId }
                            };
                            _stockRepository.Add(stock);
                        }
                        catch (Exception ex)
                        {
                            results.Enqueue(new UpdateInventoryResponseViewModel() { item_id = item.id, status = "error", error = ex.Message.ToString() });
                        }
                    });

                    _unitOfWork.SaveChanges();
                    transaction.Commit();

                    return Json(BaseHttpResult.GenerateSuccess(results, "ok"));
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return Json(BaseHttpResult.GenerateError(ex.Message.ToString(), ResponseType.ServerError));
                }
            }
        }
    }
}