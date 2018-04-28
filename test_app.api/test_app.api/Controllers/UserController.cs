using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
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
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IWinnerRepository _winnerRepository;

        public UserController(
            UserManager<ApplicationUser> userManager,
            IUserRepository userRepository,
            IWinnerRepository winnerRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _winnerRepository = winnerRepository;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("getuserdata")]
        [HttpGet]
        public async Task<IActionResult> GetUserData()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            user.Roles = await _userManager.GetRolesAsync(user);
            return Json(user);
        }

        [Route("getuser")]
        [HttpGet]
        public IActionResult GetUser(string id)
        {
            var user = _userRepository.GetUserById(id);
            var wonItems = _winnerRepository.GetWinners(id);

            user.WonItems.Clear();
            user.WonItems.AddRange(wonItems);

            return Json(user);
        }

        public class UpdateTradeofferUrlModel {
            public string tradeofferurl { get; set; }
        }

        [Route("updatetradeoffer")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<IActionResult> UpdateTradeofferUrl([FromBody]UpdateTradeofferUrlModel req)
        {
            Regex regex = new Regex(@"^http[s]*:\/\/steamcommunity.com\/tradeoffer\/new\/\?partner=([0-9]+)&token=([a-zA-Z0-9]+)$");

            if (!regex.IsMatch(req.tradeofferurl)) {
                return BadRequest(BaseHttpResult.GenerateError("Tradeoffer url not valid", ResponseType.ValidationError));
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);

            user.TradeofferUrl = req.tradeofferurl;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Json(BaseHttpResult.GenerateSuccess(req.tradeofferurl, "success update"));
            }
            else
            {
                return BadRequest(BaseHttpResult.GenerateError("Server error", ResponseType.ServerError));
            }
        }
    }
}