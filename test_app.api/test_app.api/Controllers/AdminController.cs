using System;
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
using test_app.api.Logic;
using Microsoft.Extensions.Caching.Memory;

namespace test_app.api.Controllers
{
    [Produces("application/json")]
    [Route("api/Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private IMemoryCache _cache;


        public AdminController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            IMemoryCache cache)
        {
            _userManager = userManager;
            _context = context;
            _cache = cache;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [Route("getcasesdata")]
        [HttpGet]
        public async Task<IActionResult> GetCasesData()
        {
            var context = (ApplicationDbContext)HttpContext.RequestServices.GetService(typeof(ApplicationDbContext));

            var cases = _cache.GetOrCreate<object>("casesAdmin", entry => {
                entry.SlidingExpiration = TimeSpan.FromHours(12);
                entry.Priority = CacheItemPriority.Normal;

                return context.Cases
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
            });

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

            _cache.Remove("casesAdmin");
            _cache.Remove("casesReal");

            return Json("OK");
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [Route("getstatistic")]
        [HttpGet]
        public async Task<IActionResult> GetStatistic()
        {
            var data = new AdminStatisticViewModel();

            // TODO: need to refactor
            Func<List<AnonStat>> generateTemplate = () => {
                return new List<AnonStat> {
                    new AnonStat() {
                        State = AnonStat.StatState.None,
                        Values = new List<AnonStatValue> {
                            new AnonStatValue() {
                                DayRange = 1,
                                Count = 0,
                                Sum = 0
                            },
                            new AnonStatValue() {
                                DayRange = 7,
                                Count = 0,
                                Sum = 0
                            },
                            new AnonStatValue() {
                                DayRange = 30,
                                Count = 0,
                                Sum = 0
                            }
                        }
                    },
                    new AnonStat() {
                        State = AnonStat.StatState.Sold,
                        Values = new List<AnonStatValue> {
                            new AnonStatValue() {
                                DayRange = 1,
                                Count = 0,
                                Sum = 0
                            },
                            new AnonStatValue() {
                                DayRange = 7,
                                Count = 0,
                                Sum = 0
                            },
                            new AnonStatValue() {
                                DayRange = 30,
                                Count = 0,
                                Sum = 0
                            },
                        }
                    },
                    new AnonStat() {
                        State = AnonStat.StatState.Traded,
                        Values = new List<AnonStatValue> {
                            new AnonStatValue() {
                                DayRange = 1,
                                Count = 0,
                                Sum = 0
                            },
                            new AnonStatValue() {
                                DayRange = 7,
                                Count = 0,
                                Sum = 0
                            },
                            new AnonStatValue() {
                                DayRange = 30,
                                Count = 0,
                                Sum = 0
                            },
                        }
                    },
                    new AnonStat() {
                        State = AnonStat.StatState.Total,
                        Values = new List<AnonStatValue> {
                            new AnonStatValue() {
                                DayRange = 1,
                                Count = 0,
                                Sum = 0
                            },
                            new AnonStatValue() {
                                DayRange = 7,
                                Count = 0,
                                Sum = 0
                            },
                            new AnonStatValue() {
                                DayRange = 30,
                                Count = 0,
                                Sum = 0
                            },
                        }
                    },
                };
            };

            var res = _context.Winners
                .Where(x => x.State != Winner.WinnerState.None && DbUtility.DateDiff("day", x.DateCreate, DateTime.Today) <= 30)
                .GroupBy(x => x.Case)
                .Select(x => new {
                    Values = x
                        .GroupBy(s => s.State).Select(d => new AnonStat()
                        {
                            State = (AnonStat.StatState)d.Key,
                            Values = d.Select(inv => new {
                                DayRange = DbUtility.DateDiff("day", inv.DateCreate, DateTime.Today) >= 0 && DbUtility.DateDiff("day", inv.DateCreate, DateTime.Today) <= 1 ? 1 :
                                           DbUtility.DateDiff("day", inv.DateCreate, DateTime.Today) > 1 && DbUtility.DateDiff("day", inv.DateCreate, DateTime.Today) <= 7 ? 7 :
                                           DbUtility.DateDiff("day", inv.DateCreate, DateTime.Today) > 7 && DbUtility.DateDiff("day", inv.DateCreate, DateTime.Today) <= 30 ? 30 : 0,
                                Price = inv.Price,
                                State = inv.State,
                                Case = inv.Case,
                                Skin = inv.Skin
                            })
                            .GroupBy(inv => inv.DayRange)
                            .Select(g => new AnonStatValue()
                            {
                                DayRange = g.Key,
                                Count = g.Count(),
                                Sum = g.Sum(ss =>
                                             (ss.State == Winner.WinnerState.None) ? 0 :
                                             (ss.State == Winner.WinnerState.Sold) ? ss.Case.Price - ss.Price :
                                             (ss.State == Winner.WinnerState.Traded) ? ss.Case.Price - _context.Stock.FirstOrDefault(l => l.Skin == ss.Skin).Price : 0)
                            }).ToList()
                        }).ToList(),
                    Case = x.Key,
                })
                .ToList();

            // тут жёсткий замут с union
            foreach (var c in res)
            {
                var temp1 = c.Values.ToList();
                c.Values.RemoveAll(x => x != null);
                c.Values.AddRange(
                    temp1
                    .Union(generateTemplate())
                    .Distinct(new AnonStatCompare())
                    .ToList().OrderBy(x => x.State));

                foreach (var s in c.Values)
                {
                    var temp2 = s.Values.ToList();
                    s.Values.RemoveAll(x => x != null);
                    var temp3 = temp2.Union(generateTemplate().Where(d => d.State == s.State)
                                    .SelectMany(ff => ff.Values))
                                    .Distinct(new AnonStatValueCompare())
                                    .OrderBy(x => x.DayRange)
                                    .ToList();

                    if (s.State == AnonStat.StatState.Total)
                    {
                        temp3.ForEach(x => 
                        {
                            var solded = c.Values
                                .FirstOrDefault(d => d.State == AnonStat.StatState.Sold)
                                .Values
                                .FirstOrDefault(d => d.DayRange == x.DayRange);

                            var traded = c.Values
                                .FirstOrDefault(d => d.State == AnonStat.StatState.Traded)
                                .Values
                                .FirstOrDefault(d => d.DayRange == x.DayRange);

                            x.Sum = solded.Sum + traded.Sum;
                            x.Count = solded.Count + traded.Count;
                        });
                    }

                    temp3.ForEach(x =>
                    {
                        switch (x.DayRange)
                        {
                            case 7:
                                if (s.State != AnonStat.StatState.Total)
                                {
                                    var f = temp3.FirstOrDefault(ss => ss.DayRange == 1);
                                    x.Count += f.Count;
                                    x.Sum += f.Sum;
                                }

                                break;

                            case 30:
                                if (s.State != AnonStat.StatState.Total)
                                {
                                    var f = temp3.FirstOrDefault(ss => ss.DayRange == 7);
                                    x.Count += f.Count;
                                    x.Sum += f.Sum;
                                }
                                break;

                            default:
                                break;
                        }

                    });

                    s.Values.AddRange(temp3);
                }
            }

            foreach (var c in res)
            {
                foreach (var s in c.Values)
                {
                    if (s.State == AnonStat.StatState.None)
                    {
                        s.Values.ForEach(x =>
                        {
                            var solded = c.Values
                                .FirstOrDefault(d => d.State == AnonStat.StatState.Sold)
                                .Values
                                .FirstOrDefault(d => d.DayRange == x.DayRange);

                            var traded = c.Values
                                .FirstOrDefault(d => d.State == AnonStat.StatState.Traded)
                                .Values
                                .FirstOrDefault(d => d.DayRange == x.DayRange);

                            var count = solded.Count + traded.Count;
                            var sum = count * c.Case.Price;

                            x.Sum = sum;
                            x.Count = count;
                        });
                    }
                }
            }

            return Json(res);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [Route("getdroprate")]
        [HttpGet]
        public async Task<IActionResult> GetDroprate(Int64 caseId)
        {
            var totalWins = _context.Winners.Where(x => x.Case.Id == caseId).Count();

            var res = _context.Winners
                .Where(x => x.Case.Id == caseId)
                .GroupBy(x => x.Skin)
                .Select(x => new
                {
                    MarketHashName = x.Key.MarketHashName,
                    Id = x.Key.Id,
                    Image = x.Key.Image,
                    Price = x.Key.Price,
                    Count = x.Count(),
                    Sum = x.Sum(s => s.Skin.Price),
                    Chance = (float)x.Count() / totalWins
                });

            return Json(res);
        }

        public class TestOpenCaseViewModel
        {
            public class TestOpenCaseTotalsViewModel
            {
                public decimal TotalCasePrice { get; set; }

                public decimal TotalSkinPrice { get; set; }
            }

            public TestOpenCaseTotalsViewModel Totals { get; set; } = new TestOpenCaseTotalsViewModel();

            public List<object> Result { get; set; } = new List<object>();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [Route("testopencase")]
        [HttpGet]
        public async Task<IActionResult> TestOpenCase(Int64 caseId, Int64 count)
        {
            var context = (ApplicationDbContext)HttpContext.RequestServices.GetService(typeof(ApplicationDbContext));

            var user = await _userManager.GetUserAsync(HttpContext.User);
            var casea = context.Cases.FirstOrDefault(x => x.Id == caseId);

            List<CaseOpenResult> openResults = new List<CaseOpenResult>();
            CaseLogic caseLogic = new CaseLogic(context, casea, user);

            for (int i = 0; i < count; i++)
            {
                openResults.Add(caseLogic.TestOpen());
            }

            var res = new TestOpenCaseViewModel();

            res.Result.AddRange(openResults
                .GroupBy(x => ((WinnerViewModel)x.Winner).Skin)
                .Select(g => new {
                    Skin = new { g.Key.MarketHashName, g.Key.Id, g.Key.Price },
                    Count = g.Count(),
                    Chance = (float)g.Count() / count
                })
                .OrderBy(x => x.Skin.Id)
                .ToList());

            res.Totals.TotalCasePrice = casea.Price * count;
            res.Totals.TotalSkinPrice = openResults.Sum(x => ((WinnerViewModel)x.Winner).Price);

            return Json(res);
        }
    }
}