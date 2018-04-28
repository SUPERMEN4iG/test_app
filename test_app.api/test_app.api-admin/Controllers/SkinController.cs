using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using test_app.shared.Repositories;
using test_app.shared.ViewModels;

namespace test_app.api_admin.Controllers
{
    [Produces("application/json")]
    [Route("api/Skin")]
    public class SkinController : Controller
    {
        private readonly ISkinRepository _skinRepository;

        public SkinController(
            ISkinRepository skinRepository)
        {
            _skinRepository = skinRepository;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [Route("getskinsdata")]
        [HttpGet]
        public IActionResult GetSkinsData()
        {
            try
            {
                var skins = _skinRepository
                    .GetSkins(x => new AdminSkinsViewModel()
                    {
                        Id = x.Id,
                        MarketHashName = x.MarketHashName,
                        Image = x.Image,
                        Price = x.Price,
                        Chance = 0
                    });

                return Json(skins);
            }
            catch (Exception ex)
            {
                return BadRequest(BaseHttpResult.GenerateError(ex.Message.ToString(), ResponseType.ServerError));
            }
        }
    }
}