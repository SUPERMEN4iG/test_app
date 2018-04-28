using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using test_app.shared.Data;
using test_app.shared.Extensions;
using test_app.shared.ViewModels;

namespace test_app.shared.Repositories
{
    public interface ICaseRepository : IRepository<Case>
    {
        IList<CaseCategoryViewModel> GetCases(Func<CasesDrop, SkinsViewModel> outputSkins);
        IEnumerable<CaseDrop> GetCaseDrop(long caseId);
        Case GetCaseSkins(Int64 id);

        CaseOpenResult OpenCase(ApplicationUser user, Case casea);
        CaseOpenResult OpenCaseTest(IEnumerable<CasesDrop> caseDrop);
    }

    public class CaseRepository : Repository<Case>, ICaseRepository
    {
        public CaseRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IList<CaseCategoryViewModel> GetCases(Func<CasesDrop, SkinsViewModel> outputSkins)
        {
            return _dbSet.Include(x => x.Category)
                    .Include(x => x.CaseSkins)
                    .ThenInclude(x => x.Skin)
                    .Select(x => new
                    {
                        x.Id,
                        x.IsAvalible,
                        Category = new { x.Category.Id, x.Category.Index, x.Category.StaticName, x.Category.FullName },
                        x.StaticName,
                        x.FullName,
                        x.Image,
                        x.Price,
                        x.PreviousPrice,
                        x.Index,
                        CaseSkins = x.CaseSkins
                    })
                    .ToList()
                    .Where(x => x.IsAvalible == true)
                    .GroupBy(x => x.Category,
                        (key, group) => new CaseCategoryViewModel()
                        {
                            Category = new CaseCategoryViewModel.CategoryViewModel() { Index = key.Index, Id = key.Id, StaticName = key.StaticName, FullName = key.FullName },
                            Cases = group.Select(c => new CasesViewModel()
                            {
                                Id = c.Id,
                                StaticName = c.StaticName,
                                FullName = c.FullName,
                                Image = c.Image,
                                Price = c.Price,
                                PreviousPrice = c.PreviousPrice,
                                Index = c.Index,
                                CategoryName = key.StaticName,
                                Skins = c.CaseSkins
                                            .Select(outputSkins)
                                            .ToList(),
                            }).OrderBy(x => x.Index).ToList()
                        }).OrderBy(x => x.Category.Index).ToList();
        }

        public IEnumerable<CaseDrop> GetCaseDrop(long caseId)
        {
            return ((ApplicationDbContext)_dbContext)
                    .CasesDrops
                    .Include(x => x.Skin)
                    .Where(x => x.CaseId == caseId)
                    .Select(x => new CaseDrop
                    {
                        Id = x.Id,
                        Chance = x.Chance,
                        Skin = x.Skin
                    })
                    .AsEnumerable();
        }

        public CaseOpenResult OpenCaseTest(IEnumerable<CasesDrop> caseDrop)
        {
            CasesDrop selected = caseDrop.RandomElementByWeight(x => x.Chance);

            return CaseOpenResult.GenerateSuccessTest(new WinnerViewModel()
            {
                MarketHashName = selected.Skin.MarketHashName,
                Price = (double)selected.Skin.Price,
                Image = selected.Skin.Image,
                Skin = selected.Skin
            }, selected.Chance.ToString());
        }

        public Case GetCaseSkins(Int64 id)
        {
            return _dbSet
                .Include(x => x.CaseSkins)
                .ThenInclude(x => x.Skin)
                .FirstOrDefault(x => x.Id == id);
        }

        public CaseOpenResult OpenCase(ApplicationUser user, Case casea)
        {
            using (var transaction = ((ApplicationDbContext)_dbContext).Database.BeginTransaction())
            {
                try
                {
                    IEnumerable<CaseDrop> caseDrop = null;
                    caseDrop = this.GetCaseDrop(casea.Id);

                    // TODO: Добавить CaseException
                    if (caseDrop.LongCount() == 0)
                    {
                        throw new Exception("case not have drop items");
                    }

                    if (user.Balance < casea.Price)
                    {
                        return CaseOpenResult.GenerateError(String.Format("Not enough money"), ResponseType.NotEnoughMoney);
                    }

                    CaseDrop selected = caseDrop.RandomElementByWeight(s => (float)s.Chance);

                    Winner winner = new Winner()
                    {
                        Case = casea,
                        Skin = selected.Skin,
                        User = user,
                        State = Winner.WinnerState.None
                    };

                    ((ApplicationDbContext)_dbContext).Add(winner);
                    user.Balance -= casea.Price;

                    ((ApplicationDbContext)_dbContext).SaveChanges();
                    transaction.Commit();

                    return CaseOpenResult.GenerateSuccess(new WinnerViewModel
                    {
                        MarketHashName = winner.Skin.MarketHashName,
                        Price = (double)winner.Skin.Price * 0.8,
                        Image = winner.Skin.Image,
                        Id = winner.Id,
                        DateCreate = winner.Skin.DateCreate,
                        Rarity = winner.Skin.Rarity,
                        Skin = null
                    }, selected.Chance.ToString());
                }
                catch (Exception e)
                {
                    transaction.Rollback();

                    ((ApplicationDbContext)_dbContext).CaseFaultLogs.Add(new CaseFaultLog() { Case = casea, Text = e.Message.ToString(), User = user });
                    ((ApplicationDbContext)_dbContext).SaveChanges();

                    return CaseOpenResult.GenerateError(e.Message.ToString(), ResponseType.ServerError);
                }
            }
        }
    }
}
