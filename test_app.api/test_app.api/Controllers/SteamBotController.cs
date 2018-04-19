using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using test_app.api.Data;
using test_app.api.Logic;
using test_app.api.Logic.Bot;
using test_app.api.Logic.Extensions;

namespace test_app.api.Controllers
{
    [Produces("application/json")]
    [Route("api/steambot")]
    public class SteamBotController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SteamBotController(
            ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("ping")]
        [HttpGet]
        public async Task<IActionResult> Ping()
        {
            try
            {
                string ip = "";
                ip = this.HttpContext.GetIpAddress();

                var bot = _context.Bots.Where(x => x.Server == ip).FirstOrDefault();
                bot.SyncTime = DateTime.Now;

                _context.Update(bot);
                await _context.SaveChangesAsync();

                return Json(BaseHttpResult.GenerateSuccess(new { ip }, "ok"));
            }
            catch (Exception ex)
            {
                return Json(BaseHttpResult.GenerateError(ex.Message.ToString(), ResponseType.ServerError));
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
        public async Task<IActionResult> UpdateInventory([FromBody]UpdateInventoryRequestViewModel requestData)
        {
            try
            {
                long botId = (long)HttpContext.Items.FirstOrDefault(x => x.Key == "botId").Value;
                var bot = _context.Bots.Where(x => x.Id == botId).FirstOrDefault();

                var results = new ConcurrentQueue<UpdateInventoryResponseViewModel>();

                Parallel.ForEach(requestData.items, (item) => {
                    try
                    {
                        var stock = new Stock()
                        {
                            Skin = new Skin() { Id = item.id },
                            Bot = new Bot() { Id = botId }
                        };
                        _context.Stock.Add(stock);
                    }
                    catch (Exception ex)
                    {
                        results.Enqueue(new UpdateInventoryResponseViewModel() { item_id = item.id, status = "error", error = ex.Message.ToString() });
                    }
                });

                _context.SaveChanges();

                return Json(BaseHttpResult.GenerateSuccess(results, "ok"));
            }
            catch (Exception ex)
            {
                return Json(BaseHttpResult.GenerateError(ex.Message.ToString(), ResponseType.ServerError));
            }
        }
    }
}