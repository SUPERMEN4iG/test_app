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
using test_app.api.Logic;
using test_app.api.Models;
using test_app.shared;
using test_app.shared.Data;
using test_app.shared.Repositories;
using test_app.shared.ViewModels;

namespace test_app.api.Controllers
{
    [Produces("application/json")]
    [Route("api/Winner")]
    public class WinnerController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;
        private readonly IWinnerRepository _winnerRepository;

        public WinnerController(
            UserManager<ApplicationUser> userManager,
            IUnitOfWork<ApplicationDbContext> unitOfWork,
            IWinnerRepository winnerRepository)
        {
            _userManager = userManager;
            _winnerRepository = winnerRepository;
            _unitOfWork = unitOfWork;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("sell")]
        [HttpGet]
        public async Task<IActionResult> Sell(Int32 id)
        {
            try
            {
                using (var transaction = _unitOfWork.Context.Database.BeginTransaction())
                {
                    var winner = _winnerRepository.GetWinnerLast(id);

                    //TODO: Нужно проверять есть ли не завершенные трейды!!!!


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
                    try
                    {
                        _unitOfWork.Context.Update(winner);
                        _unitOfWork.Context.Update(user);
                        var res = _unitOfWork.SaveChanges();

                        transaction.Commit();
                        return Json(BaseHttpResult.GenerateSuccess(winner.Skin.Price * 0.8M, "Item was sold"));
                    }
                    catch(Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }

                }

            }
            catch (Exception ex)
            {
                return BadRequest(BaseHttpResult.GenerateError("Server error", ResponseType.ServerError));
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("take")]
        [HttpGet]
        public IActionResult Take(Int32 id)
        {
            return BadRequest(BaseHttpResult.GenerateError("Server error", ResponseType.ServerError));
        }




    }
}