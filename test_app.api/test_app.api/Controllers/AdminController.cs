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

namespace test_app.api.Controllers
{
    [Produces("application/json")]
    [Route("api/Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
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

        public class AnonStatValue
        {
            public int DayRange { get; set; }

            public int Count { get; set; }

            public decimal? Sum { get; set; }
        }

        public class AnonStat
        {
            public enum StatState
            {
                None = 0,
                Sold = 1,
                Traded = 2,
                Total = 3,
                TotalToWithDraw = 4
            }

            public StatState State { get; set; }

            public List<AnonStatValue> Values { get; set; }
        }

        class TestCompare : IEqualityComparer<AnonStat>
        {
            public bool Equals(AnonStat x, AnonStat y)
            {
                return x.State == y.State;
            }
            public int GetHashCode(AnonStat codeh)
            {
                return codeh.State.GetHashCode();
            }
        }

        class TestCompare2 : IEqualityComparer<AnonStatValue>
        {
            public bool Equals(AnonStatValue x, AnonStatValue y)
            {
                return x.DayRange == y.DayRange;
            }
            public int GetHashCode(AnonStatValue codeh)
            {
                return codeh.DayRange.GetHashCode();
            }
        }

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [Route("getstatistic")]
        [HttpGet]
        public async Task<IActionResult> GetStatistic()
        {
            var data = new AdminStatisticViewModel();

            var now = DateTime.Today;
            var periods = new[] { 5, 10, 20, 30 };

            //var res = _context.Winners
            //    .GroupBy(x => x.Case)
            //    .Select(x => new {
            //        Values = x
            //            .GroupBy(d => new { Dayily = d.DateCreate.Day, Nedelya = d.DateCreate.AddDays(-7), Monthly = d.DateCreate.AddDays(-30) })
            //            .Select(f => f.GroupBy(g => g.State).Select(c => new { State = c.Key, Count = c.Count(), Sum = c.Sum(s => s.State == Winner.WinnerState.None ? s.Case.Price : s.Price.GetValueOrDefault()) })),
            //        Key = x.Key,
            //    })
            //    .ToList();

            /*
            var res = _context.Winners
                .Select(inv => new
                {
                    DayRange = DbUtility.DateDiff("day", inv.DateCreate, DateTime.Today) <= 1 ? 1 :
                               DbUtility.DateDiff("day", inv.DateCreate, DateTime.Today) > 1 && DbUtility.DateDiff("day", inv.DateCreate, DateTime.Today) <= 7 ? 7 :
                               DbUtility.DateDiff("day", inv.DateCreate, DateTime.Today) > 7 && DbUtility.DateDiff("day", inv.DateCreate, DateTime.Today) <= 30 ? 30 : 0,
                    Price = inv.Price,
                }).GroupBy(inv => inv.DayRange)
                .Select(g => new
                {
                    DayRange = g.Key,
                    Sum = g.Sum(inv => inv.Price)
                }).ToList();
                */

            var template = new List<AnonStat> {
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
                c.Values.AddRange(temp1.Union(template).Distinct(new TestCompare()).ToList().OrderBy(x => x.State));

                foreach (var s in c.Values)
                {
                    var temp2 = s.Values.ToList();
                    s.Values.RemoveAll(x => x != null);
                    s.Values.AddRange(temp2.Union(template.Where(d => d.State == s.State).SelectMany(ff => ff.Values)).Distinct(new TestCompare2()).ToList().OrderBy(x => x.DayRange));

                    if (s.State == AnonStat.StatState.Total)
                    {
                        s.Values.ForEach(x => 
                        {
                            var sum = c.Values.FirstOrDefault(d => d.State == AnonStat.StatState.Sold).Values.FirstOrDefault(d => d.DayRange == x.DayRange).Sum;
                            var sum2 = c.Values.FirstOrDefault(d => d.State == AnonStat.StatState.Traded).Values.FirstOrDefault(d => d.DayRange == x.DayRange).Sum;
                            x.Sum = sum + sum2;
                        });
                    }
                }
            }

            return Json(res);
        }

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
                    Chance = (float)x.Count() / totalWins
                });

            return Json(res);
        }
    }
}