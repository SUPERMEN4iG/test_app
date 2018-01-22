using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using test_app.api.Models;

namespace test_app.api.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return PartialView("Index");
        }
    }
}
