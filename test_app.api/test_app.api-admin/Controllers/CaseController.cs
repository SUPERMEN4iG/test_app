using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using test_app.shared;
using test_app.shared.Data;
using test_app.shared.Logic;
using test_app.shared.Repositories;
using test_app.shared.ViewModels;
using test_app.shared.Extensions;

namespace test_app.api_admin.Controllers
{
    [Produces("application/json")]
    [Route("api/Case")]
    public class CaseController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICaseRepository _caseRepository;
        private readonly ICaseCategoryRepository _caseCategoryRepository;
        private readonly ICaseDropRepository _caseDropRepository;
        private readonly IWinnerRepository _winnerRepository;
        private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;
        private IMemoryCache _cache;


        public CaseController(
            UserManager<ApplicationUser> userManager,
            IUnitOfWork<ApplicationDbContext> unitOfWork,
            ICaseRepository caseRepository,
            ICaseCategoryRepository caseCategoryRepository,
            ICaseDropRepository caseDropRepository,
            IWinnerRepository winnerRepository,
            IMemoryCache cache)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _cache = cache;
            _caseRepository = caseRepository;
            _caseCategoryRepository = caseCategoryRepository;
            _caseDropRepository = caseDropRepository;
            _winnerRepository = winnerRepository;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [Route("getcasesdata")]
        [HttpGet]
        public IActionResult GetCasesData()
        {
            var cases = _cache.GetOrCreate<object>("casesAdmin", entry => {
                entry.SlidingExpiration = TimeSpan.FromHours(12);
                entry.Priority = CacheItemPriority.Normal;

                return _caseRepository.GetCases(x => new AdminSkinsViewModel()
                {
                    Id = x.Skin.Id,
                    MarketHashName = x.Skin.MarketHashName,
                    Image = x.Skin.Image,
                    Price = x.Skin.Price,
                    Chance = x.Chance
                });
            });

            return Json(cases);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [Route("getcategorydata")]
        [HttpGet]
        public IActionResult GetCategoryData()
        {
            var cases = _cache.GetOrCreate<object>("caseCategories", entry => {
                entry.SlidingExpiration = TimeSpan.FromHours(12);
                entry.Priority = CacheItemPriority.Normal;

                return _caseCategoryRepository
                    .Get()
                    .Select(x => new CaseCategoryDataViewModel()
                    {
                        Id = x.Id,
                        FullName = x.FullName,
                        Index = x.Index
                    })
                    .ToList();
            });

            return Json(cases);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [Route("caclulatechances")]
        [HttpGet]
        public IActionResult CaclulateChances(int caseId, decimal margine)
        {
            var founded = _caseRepository.Get(x => x.Id == caseId).FirstOrDefault();
            var ignoredIds = new List<long>();

            var calculator = new ChanceCalc();

            var skinPool = new List<SkinViewModel>();
            skinPool = _caseDropRepository.Get(x => x.Case.Id == caseId && !ignoredIds.Contains(x.Skin.Id)).Select(skin => new SkinViewModel
            {
                Id = skin.SkinId,
                Price = skin.Skin.Price * 0.8M
            }).ToList();

            var skinPoolIgnored = new List<SkinViewModel>();
            skinPoolIgnored = _caseDropRepository.Get(x => x.Case.Id == caseId && ignoredIds.Contains(x.Skin.Id)).Select(skin => new SkinViewModel
            {
                Id = skin.SkinId,
                Price = skin.Skin.Price * 0.8M,
                Chance = 0
            }).ToList();

            var calced = calculator.Calc((double)margine, (double)founded.Price, skinPool, skinPoolIgnored);

            return Json(calced);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [Route("createcase")]
        [HttpPost]
        public IActionResult CreateCase([FromBody]CaseViewModel caseData)
        {
            try
            {
                var currentCase = new Case();
                currentCase.FullName = caseData.FullName;
                currentCase.StaticName = currentCase.FullName.ToLower().Replace(' ', '-');
                currentCase.Price = caseData.Price;
                currentCase.IsAvalible = caseData.IsAvalible;
                currentCase.Image = caseData.Image;

                if (caseData.Category == null) throw new Exception("please select category");

                if (caseData.Category.Id != 0)
                {
                    currentCase.Category = _caseCategoryRepository.Get(x => x.Id == caseData.Category.Id).Include(x => x.Cases).FirstOrDefault();
                    currentCase.Category.Cases.Add(currentCase);
                }
                else
                {
                    var cases = new List<Case>();
                    cases.Add(currentCase);
                    currentCase.Category = new CaseCategory()
                    {
                        Index = _caseCategoryRepository.Get().Count() - 1,
                        FullName = caseData.FullName,
                        StaticName = caseData.FullName.ToLower().Replace(' ', '-'),
                        Cases = cases
                    };
                }

                if (caseData.IsNeedRecalc)
                {
                    var calculator = new ChanceCalc();
                    var calced = calculator.Calc(
                        20,
                        (double)caseData.Price,
                        caseData.Skins.Select(x => new SkinViewModel()
                        {
                            Id = x.Id,
                            MarketHashName = x.MarketHashName,
                            Chance = 0,
                            Price = x.Price * 0.8M
                        }).ToList(),
                        new List<SkinViewModel>());

                    caseData.Skins.ForEach((skin) =>
                    {
                        skin.Chance = calced.FirstOrDefault(x => x.Id == skin.Id).Chance;
                    });
                }

                currentCase.CaseSkins = new List<CasesDrop>();

                caseData.Skins.ForEach((skin) => {
                    currentCase.CaseSkins.Add(new CasesDrop()
                    {
                        CaseId = currentCase.Id,
                        SkinId = skin.Id,
                        Chance = skin.Chance
                    });
                });

                _caseRepository.Update(currentCase);
                _unitOfWork.SaveChanges();

                _cache.Remove("casesAdmin");
                _cache.Remove("casesReal");

                return Json(BaseHttpResult.GenerateSuccess(null, "case added"));
            }
            catch (Exception ex)
            {
                return BadRequest(BaseHttpResult.GenerateError(ex.Message, ResponseType.ServerError));
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [Route("savecase")]
        [HttpPost]
        public IActionResult SaveCase([FromBody]CaseViewModel caseData)
        {
            var currentCase = _caseRepository.GetCaseSkins(caseData.Id);

            currentCase.CaseSkins = currentCase.CaseSkins.Where(x => caseData.Skins.Any(s => s.Id == x.SkinId)).ToList();

            currentCase.FullName = caseData.FullName;
            currentCase.StaticName = currentCase.FullName.ToLower().Replace(' ', '-');
            currentCase.Price = caseData.Price;
            currentCase.IsAvalible = caseData.IsAvalible;

            caseData.Skins.ForEach((skin) => {
                var founded = currentCase.CaseSkins.FirstOrDefault(x => x.SkinId == skin.Id);
                if (founded != null)
                {
                    founded.Chance = skin.Chance;
                }
                else
                {
                    currentCase.CaseSkins.Add(new CasesDrop()
                    {
                        CaseId = currentCase.Id,
                        SkinId = skin.Id,
                        Chance = skin.Chance
                    });
                }
            });

            _caseRepository.Update(currentCase);
            _unitOfWork.SaveChanges();

            _cache.Remove("casesAdmin");
            _cache.Remove("casesReal");

            return Json("OK");
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [Route("getdroprate")]
        [HttpGet]
        public IActionResult GetDroprate(Int64 caseId)
        {
            try
            {
                var res = _winnerRepository.GetDropRate(caseId);
                return Json(res);
            }
            catch (Exception ex)
            {
                return BadRequest(BaseHttpResult.GenerateError(ex.Message.ToString(), ResponseType.ServerError));
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [Route("testopencase")]
        [HttpGet]
        public async Task<IActionResult> TestOpenCase(Int64 caseId, Int64 count)
        {
            var casea = _caseRepository.GetCaseSkins(caseId);
            var caseDrops = casea.CaseSkins.ToList();

            List<CaseOpenResult> openResults = new List<CaseOpenResult>();

            for (int i = 0; i < count; i++)
            {
                var redd = _caseRepository.OpenCaseTest(caseDrops);
                openResults.Add(redd);
            }

            var res = new TestOpenCaseViewModel();

            res.Totals.TotalCasePrice = casea.Price * count;
            res.Totals.TotalSkinPrice = openResults.Sum(x => x.Winner.Price);

            res.Result.AddRange(openResults
                .GroupBy(x => ((WinnerViewModel)x.Winner).Skin)
                .Select(g => new
                {
                    Skin = new { g.Key.MarketHashName, g.Key.Id, g.Key.Price },
                    Count = g.Count(),
                    Chance = (float)g.Count() / count
                })
                .OrderBy(x => x.Skin.Id)
                .ToList());
            res.Totals.TotalMarginality = (100 - (res.Totals.TotalSkinPrice / res.Totals.TotalCasePrice * 100)) / 100;

            return Json(res);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [Route("getstatistic")]
        [HttpGet]
        public IActionResult GetStatistic(Int64 caseId)
        {
            var winnersCount = _winnerRepository.Get(x => x.CaseId == caseId).Count();
            var statCase = new StatisticViewModel();
            statCase.TotalCount = winnersCount;

            if (winnersCount > 0)
            {
                statCase.SumCase = _caseRepository.Get(x => x.Id == caseId).FirstOrDefault().Price * winnersCount;
                statCase.SumSkin = _winnerRepository
                    .Get(x => x.CaseId == caseId)
                    .Include(x => x.Skin)
                    .Sum(x => x.Skin.Price * 0.8M);
                statCase.Margine = (100 - (statCase.SumSkin / statCase.SumCase * 100)) / 100;
            }

            statCase.Drops = _winnerRepository
                .Get(x => x.CaseId == caseId)
                .Include(x => x.Skin)
                .Select(x => new
                {
                    x.Id,
                    x.State,
                    Skin = x.Skin,
                })
                .GroupBy(x => x.Skin, (key, g) => new WinnerStatisticViewModel()
                {
                    Skin = key,
                    Count = g.LongCount(),
                    Sum = g.Sum(p => p.Skin.Price),
                    Chance = (decimal)g.Count() / winnersCount,
                })
                .ToList();

            return Json(statCase);
        }
    }
}