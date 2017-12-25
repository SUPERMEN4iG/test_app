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
using test_app.api.Data;
using test_app.api.Models;

namespace test_app.api.Controllers
{
    [Produces("application/json")]
    [Route("api/skins")]
    public class SkinsController : Controller
    {
        public SkinsController(
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        [Route("getskins")]
        [HttpGet]
        public async Task<IActionResult> GetSkins()
        {
            var skins = new object();

            var context = (ApplicationDbContext)HttpContext.RequestServices.GetService(typeof(ApplicationDbContext));

            skins = context.Cases
                .Include(x => x.CaseSkins)
                .ThenInclude(x => x.Skin)
                .GroupBy(x => x.StaticName,
                    (key, group) => new
                    {
                        Case = key,
                        Skins = group.SelectMany(d => d.CaseSkins.Select(s => new { s.Skin.Id, s.Skin.MarketHashName, s.Skin.Price, s.Skin.Image }))
                    }).ToList();

            return Json(skins);
        }
    }
}