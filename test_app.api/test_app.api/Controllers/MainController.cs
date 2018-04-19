using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using test_app.api.Data;
using test_app.api.Logic.LastWinnersSocket;

namespace test_app.api.Controllers
{
    [Produces("application/json")]
    [Route("api/main")]
    public class MainController : Controller
    {
        public MainController()
        {
        }

        [HttpGet]
        [Route("getdata")]
        public async Task<IActionResult> GetData()
        {
            var context = (ApplicationDbContext)HttpContext.RequestServices.GetService(typeof(ApplicationDbContext));
            var result = new GetDataResult();

            result.OpennedCases = 0;
            result.UsersRegistered = 0;
            result.Online = 0;
            result.LastWinners = new List<object>();

            try {
                result.OpennedCases = context.Winners.Count();
                result.UsersRegistered = context.Users.Count();

                result.LastWinners.AddRange(context.Winners
                    .Include(x => x.Skin)
                    .Include(x => x.User)
                    .Include(x => x.Case)
                    .OrderByDescending(x => x.DateCreate)
                    .Select(x => new {
                        user_name = x.User.SteamUsername,
                        winner_id = x.Id,
                        skin_name = x.Skin.MarketHashName,
                        skin_rarity = x.Skin.Rarity,
                        skin_image = x.Skin.Image,
                        case_name = x.Case.FullName,
                        case_static_name = x.Case.StaticName
                    })
                    .ToList().Take(8));

                result.Online = 0;
                result.ServerTime = DateTime.Now;
            }
            catch (Exception ex)
            {
                // TODO: Logs to need
            }

            return Json(result);
        }
    }

    public class GetDataResult
    {
        public Int64 OpennedCases { get; set; }
        public Int64 UsersRegistered { get; set; }
        public Int64 Online { get; set; }
        public DateTime ServerTime { get; set; }
        public List<object> LastWinners { get; set; }
    }
}