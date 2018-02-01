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
    [Route("api/Winner")]
    public class WinnerController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public WinnerController(
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("sell")]
        [HttpGet]
        public async Task<IActionResult> Sell(Int32 id)
        {
            try
            {
                var context = (ApplicationDbContext)HttpContext.RequestServices.GetService(typeof(ApplicationDbContext));

                var winner = context.Winners.Include(x => x.Skin).Include(x => x.User).LastOrDefault(x => x.Id == id);

                if (winner.State != Winner.WinnerState.None) {
                    throw new Exception("wtf");
                }

                var stockItems = context.Stock.Include(x => x.Skin).Where(x => x.Skin.Id == winner.Skin.Id);
                var cnt = stockItems.Count();
                var stockItem = stockItems.Count() > 0 ? stockItems.FirstOrDefault() : null;

                winner.State = Winner.WinnerState.Sold;
                winner.Price = (stockItem == null) ? winner.Skin.Price : stockItem.Price;

                var user = await _userManager.GetUserAsync(HttpContext.User);

                if (user.Id != winner.User.Id)
                {
                    throw new Exception("wtf");
                }

                user.Balance += winner.Price.Value;

                context.Update(winner);
                context.Update(user);
                var res = await context.SaveChangesAsync();

                return Json(BaseHttpResult.GenerateSuccess(winner.Price, "Item was sold"));

            } catch (Exception ex) {
                return BadRequest(BaseHttpResult.GenerateError("Server error", ResponseType.ServerError));
            }
        }
    }
}