using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using test_app.shared.Repositories;

namespace test_app.api_admin.Controllers
{
    [Produces("application/json")]
    public class HomeController : Controller
    {
        private readonly IUserRepository _userRepository;

        public HomeController(
            IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            return Json(new { ServerTime = DateTime.UtcNow, UsersCount = _userRepository.GetUsersCount() });
        }
    }
}