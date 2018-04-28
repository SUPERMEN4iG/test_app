using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using test_app.shared;
using test_app.shared.Repositories;

namespace test_app.api_admin.Controllers
{
    [Produces("application/json")]
    [Route("api/Statistic")]
    public class StatisticController : Controller
    {
        private readonly ICaseRepository _caseRepository;
        private readonly ICaseDropRepository _caseDropRepository;
        private readonly IWinnerRepository _winnerRepository;
        private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;
        private IMemoryCache _cache;

        public StatisticController(
            IUnitOfWork<ApplicationDbContext> unitOfWork,
            ICaseRepository caseRepository,
            ICaseDropRepository caseDropRepository,
            IWinnerRepository winnerRepository,
            IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
            _caseRepository = caseRepository;
            _caseDropRepository = caseDropRepository;
            _winnerRepository = winnerRepository;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [Route("getstatistic")]
        [HttpGet]
        public IActionResult GetStatistic()
        {
            return Json(true);
            //var data = new AdminStatisticViewModel();

            //// TODO: need to refactor
            //Func<List<AnonStat>> generateTemplate = () => {
            //    return new List<AnonStat> {
            //        new AnonStat() {
            //            State = AnonStat.StatState.None,
            //            Values = new List<AnonStatValue> {
            //                new AnonStatValue() {
            //                    DayRange = 1,
            //                    Count = 0,
            //                    Sum = 0
            //                },
            //                new AnonStatValue() {
            //                    DayRange = 7,
            //                    Count = 0,
            //                    Sum = 0
            //                },
            //                new AnonStatValue() {
            //                    DayRange = 30,
            //                    Count = 0,
            //                    Sum = 0
            //                }
            //            }
            //        },
            //        new AnonStat() {
            //            State = AnonStat.StatState.Sold,
            //            Values = new List<AnonStatValue> {
            //                new AnonStatValue() {
            //                    DayRange = 1,
            //                    Count = 0,
            //                    Sum = 0
            //                },
            //                new AnonStatValue() {
            //                    DayRange = 7,
            //                    Count = 0,
            //                    Sum = 0
            //                },
            //                new AnonStatValue() {
            //                    DayRange = 30,
            //                    Count = 0,
            //                    Sum = 0
            //                },
            //            }
            //        },
            //        new AnonStat() {
            //            State = AnonStat.StatState.Traded,
            //            Values = new List<AnonStatValue> {
            //                new AnonStatValue() {
            //                    DayRange = 1,
            //                    Count = 0,
            //                    Sum = 0
            //                },
            //                new AnonStatValue() {
            //                    DayRange = 7,
            //                    Count = 0,
            //                    Sum = 0
            //                },
            //                new AnonStatValue() {
            //                    DayRange = 30,
            //                    Count = 0,
            //                    Sum = 0
            //                },
            //            }
            //        },
            //        new AnonStat() {
            //            State = AnonStat.StatState.Total,
            //            Values = new List<AnonStatValue> {
            //                new AnonStatValue() {
            //                    DayRange = 1,
            //                    Count = 0,
            //                    Sum = 0
            //                },
            //                new AnonStatValue() {
            //                    DayRange = 7,
            //                    Count = 0,
            //                    Sum = 0
            //                },
            //                new AnonStatValue() {
            //                    DayRange = 30,
            //                    Count = 0,
            //                    Sum = 0
            //                },
            //            }
            //        },
            //    };
            //};

            //var res = _context.Winners.Include(x => x.Skin)
            //    .Where(x => x.State != Winner.WinnerState.None && DbUtility.DateDiff("day", x.DateCreate, DateTime.Today) <= 30)
            //    .GroupBy(x => x.Case)
            //    .Select(x => new {
            //        Values = x
            //            .GroupBy(s => s.State).Select(d => new AnonStat()
            //            {
            //                State = (AnonStat.StatState)d.Key,
            //                Values = d.Select(inv => new {
            //                    DayRange = DbUtility.DateDiff("day", inv.DateCreate, DateTime.Today) >= 0 && DbUtility.DateDiff("day", inv.DateCreate, DateTime.Today) <= 1 ? 1 :
            //                               DbUtility.DateDiff("day", inv.DateCreate, DateTime.Today) > 1 && DbUtility.DateDiff("day", inv.DateCreate, DateTime.Today) <= 7 ? 7 :
            //                               DbUtility.DateDiff("day", inv.DateCreate, DateTime.Today) > 7 && DbUtility.DateDiff("day", inv.DateCreate, DateTime.Today) <= 30 ? 30 : 0,
            //                    Price = inv.Skin.Price,
            //                    State = inv.State,
            //                    Case = inv.Case,
            //                    Skin = inv.Skin
            //                })
            //                .GroupBy(inv => inv.DayRange)
            //                .Select(g => new AnonStatValue()
            //                {
            //                    DayRange = g.Key,
            //                    Count = g.Count(),
            //                    Sum = g.Sum(ss =>
            //                                 (ss.State == Winner.WinnerState.None) ? 0 :
            //                                 (ss.State == Winner.WinnerState.Sold) ? ss.Case.Price - ss.Price * 0.8M :
            //                                (ss.State == Winner.WinnerState.Traded) ? ss.Case.Price - ss.Price : 0)
            //                }).ToList()
            //            }).ToList(),
            //        Case = x.Key,
            //    })
            //    .ToList();

            //// тут жёсткий замут с union
            //foreach (var c in res.ToList())
            //{
            //    var temp1 = c.Values.ToList();
            //    c.Values.RemoveAll(x => x != null);
            //    c.Values.AddRange(
            //        temp1
            //        .Union(generateTemplate())
            //        .Distinct(new AnonStatCompare())
            //        .ToList().OrderBy(x => x.State));

            //    foreach (var s in c.Values)
            //    {
            //        var temp2 = s.Values.ToList();
            //        s.Values.RemoveAll(x => x != null);
            //        var temp3 = temp2.Union(generateTemplate().Where(d => d.State == s.State)
            //                        .SelectMany(ff => ff.Values))
            //                        .Distinct(new AnonStatValueCompare())
            //                        .OrderBy(x => x.DayRange)
            //                        .ToList();

            //        if (s.State == AnonStat.StatState.Total)
            //        {
            //            temp3.ForEach(x =>
            //            {
            //                var solded = c.Values
            //                    .FirstOrDefault(d => d.State == AnonStat.StatState.Sold)
            //                    .Values
            //                    .FirstOrDefault(d => d.DayRange == x.DayRange);

            //                var traded = c.Values
            //                    .FirstOrDefault(d => d.State == AnonStat.StatState.Traded)
            //                    .Values
            //                    .FirstOrDefault(d => d.DayRange == x.DayRange);

            //                x.Sum = solded.Sum + traded.Sum;
            //                x.Count = solded.Count + traded.Count;
            //            });
            //        }

            //        temp3.ForEach(x =>
            //        {
            //            switch (x.DayRange)
            //            {
            //                case 7:
            //                    if (s.State != AnonStat.StatState.Total)
            //                    {
            //                        var f = temp3.FirstOrDefault(ss => ss.DayRange == 1);
            //                        x.Count += f.Count;
            //                        x.Sum += f.Sum;
            //                    }

            //                    break;

            //                case 30:
            //                    if (s.State != AnonStat.StatState.Total)
            //                    {
            //                        var f = temp3.FirstOrDefault(ss => ss.DayRange == 7);
            //                        x.Count += f.Count;
            //                        x.Sum += f.Sum;
            //                    }
            //                    break;

            //                default:
            //                    break;
            //            }

            //        });

            //        s.Values.AddRange(temp3);
            //    }
            //}

            //foreach (var c in res)
            //{
            //    foreach (var s in c.Values)
            //    {
            //        if (s.State == AnonStat.StatState.None)
            //        {
            //            s.Values.ForEach(x =>
            //            {
            //                var solded = c.Values
            //                    .FirstOrDefault(d => d.State == AnonStat.StatState.Sold)
            //                    .Values
            //                    .FirstOrDefault(d => d.DayRange == x.DayRange);

            //                var traded = c.Values
            //                    .FirstOrDefault(d => d.State == AnonStat.StatState.Traded)
            //                    .Values
            //                    .FirstOrDefault(d => d.DayRange == x.DayRange);

            //                var count = solded.Count + traded.Count;
            //                var sum = count * c.Case.Price;

            //                x.Sum = sum;
            //                x.Count = count;
            //            });
            //        }
            //    }
            //}

            //return Json(res);
        }
    }
}