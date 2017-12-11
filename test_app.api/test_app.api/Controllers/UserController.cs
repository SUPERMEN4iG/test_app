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
    }
}