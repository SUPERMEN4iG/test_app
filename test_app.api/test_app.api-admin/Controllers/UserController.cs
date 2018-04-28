using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using test_app.shared.Data;
using test_app.shared.Repositories;

namespace test_app.api_admin.Controllers
{
    [Produces("application/json")]
    [Route("api/User")]
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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [Route("getuserdata")]
        [HttpGet]
        public async Task<IActionResult> GetUserData()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            user.Roles = await _userManager.GetRolesAsync(user);
            return Json(user);
        }
    }
}