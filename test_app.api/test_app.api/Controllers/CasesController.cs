using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using test_app.api.Logic;
using test_app.api.Logic.LastWinnersSocket;
using test_app.api.Models;
using test_app.shared;
using test_app.shared.Data;
using test_app.shared.Repositories;

namespace test_app.api.Controllers
{
    [Produces("application/json")]
    [Route("api/Cases")]
    public class CasesController : Controller
    {
        public CasesController(
            UserManager<ApplicationUser> userManager,
            IMemoryCache cache,
            ICaseRepository caseRepository,
            LastWinnersHandler lastWinnersHandler)
        {
            _userManager = userManager;
            _cache = cache;
            _lastWinnersHandler = lastWinnersHandler;
            _caseRepository = caseRepository;
        }

        private readonly ICaseRepository _caseRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private LastWinnersHandler _lastWinnersHandler { get; set; }
        private IMemoryCache _cache;

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("getcases")]
        [HttpGet]
        public IActionResult GetCases()
        {
            var cases = _cache.GetOrCreate<object>("casesReal", entry => {
                entry.SlidingExpiration = TimeSpan.FromHours(12);
                entry.Priority = CacheItemPriority.Normal;

                return _caseRepository.GetCases(x => 
                    new shared.ViewModels.SkinsViewModel
                    {
                        Id = x.Skin.Id,
                        MarketHashName = x.Skin.MarketHashName,
                        Image = x.Skin.Image,
                        Price = x.Skin.Price * 0.8M
                    });
            });

            return Json(cases);
        }

        [Route("opencase")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<IActionResult> OpenCase(int id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var casea = _caseRepository.Get(x => x.Id == id).FirstOrDefault();

            var openResult = _caseRepository.OpenCase(user, casea);

            if (openResult.IsSuccess)
            {
                var message = new {
                    skin_name = openResult.Winner.MarketHashName,
                    skin_image = openResult.Winner.Image,
                    skin_rarity = openResult.Winner.Rarity,
                    user_name = user.SteamUsername,
                    user_id = user.Id,
                    case_name = casea.FullName,
                    case_static_name = casea.StaticName
                };
                await _lastWinnersHandler.SendMessageToAllAsync(JsonConvert.SerializeObject(message));
                return Json(openResult);
            }
            else
            {
                return BadRequest(openResult);
            }
        }
    }
}