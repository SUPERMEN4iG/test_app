﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using test_app.api.Data;
using test_app.api.Models;
using test_app.api.Models.ViewModels;
using test_app.api.Helper;

namespace test_app.api.Controllers
{
    [Produces("application/json")]
    [Route("api/Admin")]
    public class AdminController : Controller
    {
        public AdminController(
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        private readonly UserManager<ApplicationUser> _userManager;

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [Route("getcasesdata")]
        [HttpGet]
        public async Task<IActionResult> GetCasesData()
        {
            var context = (ApplicationDbContext)HttpContext.RequestServices.GetService(typeof(ApplicationDbContext));
            var cases = context.Cases
                .Include(x => x.Category)
                .Where(x => x.IsAvalible == true)
                .GroupBy(x => x.Category,
                    (key, group) => new CasesCategoryViewModel()
                    {
                        Category = new CategoryViewMode() { Index = key.Index, Id = key.Id, StaticName = key.StaticName, FullName = key.FullName },
                        Cases = group.Select(c => new Models.ViewModels.CaseViewModel()
                        {
                            Id = c.Id,
                            StaticName = c.StaticName,
                            FullName = c.FullName,
                            Image = c.Image,
                            Price = c.Price,
                            PreviousPrice = c.PreviousPrice,
                            Index = c.Index,
                            CategoryName = c.Category.StaticName,
                            Skins = c.CaseSkins.Select(s => new AdminSkinsViewModel() { Chance = s.Chance, Id = s.Skin.Id, MarketHashName = s.Skin.MarketHashName, Image = s.Skin.Image, Price = s.Skin.Price }).ToList()
                        }).OrderBy(x => x.Index).ToList()
                    }).OrderBy(x => x.Category.Index).ToList();

            return Json(cases);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [Route("getskinsdata")]
        [HttpGet]
        public async Task<IActionResult> GetSkinsData()
        {
            var context = (ApplicationDbContext)HttpContext.RequestServices.GetService(typeof(ApplicationDbContext));
            var skins = context.Skins.ToList().Select(x => new AdminSkinsViewModel() { Id = x.Id, Image = x.Image, MarketHashName = x.MarketHashName, Price = x.Price, Chance = 0 }).ToList();

            return Json(skins);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [Route("caclulatechances")]
        [HttpGet]
        public async Task<IActionResult> CaclulateChances(int caseId)
        {
            var context = (ApplicationDbContext)HttpContext.RequestServices.GetService(typeof(ApplicationDbContext));

            var founded = context.Cases.FirstOrDefault(x => x.Id == caseId);

            var calculator = new ChanceCalc(context);
            var calced = calculator.Calc(caseId, (double)founded.Price, new List<long>());

            return Json(calced);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [Route("savecase")]
        [HttpPost]
        public async Task<IActionResult> SaveCase([FromBody]CaseViewModel caseData)
        {
            var context = (ApplicationDbContext)HttpContext.RequestServices.GetService(typeof(ApplicationDbContext));

            var currentCase = context.Cases
                .Include(x => x.CaseSkins)
                .FirstOrDefault(x => x.Id == caseData.Id);

            currentCase.CaseSkins = currentCase.CaseSkins.Where(x => caseData.Skins.Any(s => s.Id == x.SkinId)).ToList();

            currentCase.FullName = caseData.FullName;
            currentCase.StaticName = currentCase.FullName.ToLower().Replace(' ', '-');
            currentCase.Price = caseData.Price;

            caseData.Skins.ForEach((skin) => {
                var founded = currentCase.CaseSkins.FirstOrDefault(x => x.SkinId == skin.Id);
                if (founded != null)
                {
                    founded.Chance = skin.Chance;
                }
                else
                {
                    currentCase.CaseSkins.Add(new CasesDrop() {
                        CaseId = currentCase.Id,
                        SkinId = skin.Id,
                        Chance = skin.Chance
                    });
                }
            });

            context.Update(currentCase);
            context.SaveChanges();

            return Json("OK");
        }
    }
}