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
using test_app.api.Data;
using test_app.api.Logic;
using test_app.api.Logic.LastWinnersSocket;
using test_app.api.Models;

namespace test_app.api.Controllers
{
    [Produces("application/json")]
    [Route("api/Cases")]
    public class CasesController : Controller
    {

        public CasesController(
            UserManager<ApplicationUser> userManager,
            IMemoryCache cache)
        {
            _userManager = userManager;
            _cache = cache;
        }

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private IMemoryCache _cache;

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("getcases")]
        [HttpGet]
        public async Task<IActionResult> GetCases()
        {
            var context = (ApplicationDbContext)HttpContext.RequestServices.GetService(typeof(ApplicationDbContext));

            var cases = _cache.GetOrCreate<object>("casesReal", entry => {
                entry.SlidingExpiration = TimeSpan.FromHours(12);
                entry.Priority = CacheItemPriority.Normal;

                return context.Cases
                    .Include(x => x.Category)
                    .Where(x => x.IsAvalible == true)
                    .GroupBy(x => x.Category,
                        (key, group) => new
                        {
                            Category = new { key.Index, key.Id, key.StaticName, key.FullName },
                            Cases = group.Select(c => new
                            {
                                Id = c.Id,
                                StaticName = c.StaticName,
                                FullName = c.FullName,
                                Image = c.Image,
                                Price = c.Price,
                                PreviousPrice = c.PreviousPrice,
                                Index = c.Index,
                                CategoryName = c.Category.StaticName,
                                Skins = c.CaseSkins.Select(s => new { s.Skin.Id, s.Skin.MarketHashName, s.Skin.Image, s.Skin.Price }).ToList()
                            }).OrderBy(x => x.Index).ToList()
                        }).OrderBy(x => x.Category.Index).ToList();
            });

            return Json(cases);
        }

        [Route("opencase")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<IActionResult> OpenCase(int id)
        {
            var context = (ApplicationDbContext)HttpContext.RequestServices.GetService(typeof(ApplicationDbContext));

            var user = await _userManager.GetUserAsync(HttpContext.User);
            var casea = context.Cases.FirstOrDefault(x => x.Id == id);

            var openResult = new CaseLogic(context, casea, user).Open();

            if (openResult.IsSuccess)
            {
                var message = new {
                    skin_name = openResult.Winner.MarketHashName,
                    skin_image = openResult.Winner.Image,
                    skin_rarity = 1,
                    user_name = user.SteamUsername,
                    user_id = user.Id,
                    case_name = casea.FullName,
                    case_static_name = casea.StaticName
                };
                return Json(openResult);
            }
            else
            {
                return BadRequest(openResult);
            }
        }
    }
}