using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        public UserController(
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        private readonly UserManager<ApplicationUser> _userManager;

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("getuserdata")]
        [HttpGet]
        public async Task<IActionResult> GetUserData()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            return Json(user);
        }

        [Route("getuser")]
        [HttpGet]
        public IActionResult GetUser(string id)
        {
            var context = (ApplicationDbContext)HttpContext.RequestServices.GetService(typeof(ApplicationDbContext));

            var user = context.Users.Where(x => x.Id == id).Select(x => new
            {
                x.Id,
                x.SteamAvatar,
                x.Email,
                x.UserName,
                wonItems = new List<object>()
            }).FirstOrDefault();

            var wonItems = context.Winners
                .Where(x => x.User.Id == user.Id)
                .Include(x => x.Skin)
                .Select(x => new
                {
                    x.Skin.Id,
                    x.DateCreate,
                    x.Skin.MarketHashName,
                    x.Skin.Image,
                    x.IsSent,
                    x.IsSold
                }).ToList();

            user.wonItems.Clear();
            user.wonItems.AddRange(wonItems);

            return Json(user);
        }
    }
}