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
    public class WinnerViewModel
    {
        public string MarketHashName { get; set; }

        public decimal Price { get; set; }

        public string Image { get; set; }

        public Skin Skin { get; set; }

        public DateTime DateCreate { get; set; }

        public Int64 Id { get; set; }
    }

    public class CaseLogic
    {
        private readonly ApplicationDbContext _context;
        private Case _case;
        private ApplicationUser _user;

        private IList<CasesDrop> _caseDrop { get; set; }

        public CaseLogic(ApplicationDbContext context, Case _case, ApplicationUser _user, List<CasesDrop> caseDrop = null)
        {
            this._context = context;
            this._case = _case;
            this._user = _user;

            if (caseDrop == null)
            {
                this._caseDrop = _context.CasesDrops.Where(x => x.CaseId == _case.Id).Include(x => x.Skin).ToList();
            }
            else
            {
                this._caseDrop = caseDrop;
            }
        }

        public decimal TestOpen2 (){

            var skinModel = _caseDrop.RandomElementByWeight(x => (float)x.Chance);
            return skinModel.Skin.Price;
        } 

        public CaseOpenResult TestOpen()
        {
            CasesDrop selected = _caseDrop.RandomElementByWeight(x => (float)x.Chance);

            Winner winner = new Winner()
            {
                Case = _case,
                Skin = selected.Skin,
                User = _user,
                State = Winner.WinnerState.None
            };

            return CaseOpenResult.GenerateSuccessTest(new WinnerViewModel() {
                MarketHashName = winner.Skin.MarketHashName,
                Price = selected.Skin.Price,
                Image = winner.Skin.Image,
                Skin = winner.Skin
            }, selected.Chance.ToString());
        }

        public CaseOpenResult Open()
        {

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // проверка баланса
                    if (_user.Balance < _case.Price)
                    {
                        return CaseOpenResult.GenerateError(String.Format("Not enough money"), ResponseType.NotEnoughMoney);
                    }

                    CasesDrop selected = this._caseDrop.RandomElementByWeight(x => (float)x.Chance);

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

                    return CaseOpenResult.GenerateSuccess(new WinnerViewModel {
                        MarketHashName = winner.Skin.MarketHashName,
                        Price = winner.Skin.Price * 0.8M,
                        Image = winner.Skin.Image,
                        Id = winner.Id,
                        DateCreate = winner.Skin.DateCreate,
                        Skin = null }, selected.Chance.ToString());
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
