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


                using (var transaction = context.Database.BeginTransaction())
                {
                    var winner = context.Winners.Include(x => x.Skin).Include(x => x.User).LastOrDefault(x => x.Id == id);

                    if (winner.State != Winner.WinnerState.None)
                    {
                        throw new Exception("wtf");
                    }

                    winner.State = Winner.WinnerState.Sold;

                    var user = await _userManager.GetUserAsync(HttpContext.User);

                    if (user.Id != winner.User.Id)
                    {
                        throw new Exception("wtf");
                    }

                    user.Balance += winner.Skin.Price * 0.8M;
                    try{
                        context.Update(winner);
                        context.Update(user);
                        var res = context.SaveChanges();

                        transaction.Commit();
                        return Json(BaseHttpResult.GenerateSuccess(winner.Skin.Price * 0.8M, "Item was sold"));
                    }
                    catch(Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }

                }

            } catch (Exception ex) {
                return BadRequest(BaseHttpResult.GenerateError("Server error", ResponseType.ServerError));
            }
        }

    }
}