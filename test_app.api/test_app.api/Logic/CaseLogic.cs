using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using test_app.api.Data;
using test_app.api.Extensions;
using test_app.api.Models;

namespace test_app.api.Logic
{
    public class CaseLogic
    {
        private readonly ApplicationDbContext _context;
        private Case _case;
        private ApplicationUser _user;

        public CaseLogic(ApplicationDbContext context, Case _case, ApplicationUser _user)
        {
            this._context = context;
            this._case = _case;
            this._user = _user;
        }

        public CaseOpenResult Open()
        {
            // for cache
            var skinsChances = _context.CasesDrops.Where(x => x.CaseId == _case.Id).Include(x => x.Skin).ToList();

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // проверка баланса
                    if (_user.Balance < _case.Price)
                    {
                        return CaseOpenResult.GenerateError(String.Format("Not enough money"), ResponseType.NotEnoughMoney);
                    }

                    CasesDrop selected = skinsChances.RandomElementByWeight(x => (float)x.Chance);

                    Winner winner = new Winner()
                    {
                        Case = _case,
                        Skin = selected.Skin,
                        User = _user,
                        State = Winner.WinnerState.None
                    };

                    _context.Add(winner);

                    _user.Balance -= _case.Price;

                    _context.SaveChanges();

                    // Получаем текущую цену
                    var stockItems = _context.Stock.Include(x => x.Skin).Where(x => x.Skin.Id == winner.Skin.Id);
                    var cnt = stockItems.Count();
                    var stockItem = stockItems.Count() > 0 ? stockItems.FirstOrDefault() : null;
                    var price = (stockItem == null) ? winner.Skin.Price : stockItem.Price;

                    //var random = new Random().NextDouble();
                    //CasesDrop selected = null;

                    //foreach (var skinsChance in skinsChances)
                    //{
                    //    if (random < (double)skinsChance.Chance)
                    //    {
                    //        selected = skinsChance;
                    //    }
                    //    random = random - (double)skinsChance.Chance;
                    //}

                    transaction.Commit();

                    return CaseOpenResult.GenerateSuccess(new { winner.Skin.MarketHashName, price, winner.Skin.Image, winner.Id, winner.Skin.DateCreate, winner.State }, selected.Chance.ToString());
                }
                catch (Exception e)
                {
                    transaction.Rollback();

                    _context.CaseFaultLogs.Add(new CaseFaultLog() { Case = _case, Text = e.Message.ToString(), User = _user });
                    _context.SaveChanges();

                    return CaseOpenResult.GenerateError(String.Format("Transaction error: {0}", e.Message), ResponseType.ServerError);
                }
            }
        }
    }
}
