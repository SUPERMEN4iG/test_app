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
using test_app.api.Logic;
using test_app.api.Models;

namespace test_app.api.Controllers
{
    [Produces("application/json")]
    [Route("api/Cases")]
    public class CasesController : Controller
    {
        public CasesController(
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("getcases")]
        [HttpGet]
        public async Task<IActionResult> GetCases()
        {
            var cases = new object();

            var context = (ApplicationDbContext)HttpContext.RequestServices.GetService(typeof(ApplicationDbContext));

            cases = context.Cases
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
                            Index = c.Index,
                            Skins = c.CaseSkins.Select(s => new { s.Skin.Id, s.Skin.MarketHashName, s.Skin.Image, s.Skin.Price })
                        }).OrderBy(x => x.Index).ToList()
                    }).OrderBy(x => x.Category.Index);

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
                return Json(openResult);
            }
            else
            {
                return BadRequest(openResult);
            }
        }
    }
}