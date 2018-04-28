using System;
using System.Collections.Generic;
using System.Text;
using test_app.shared.Data;
using test_app.shared.ViewModels;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace test_app.shared.Repositories
{
    public interface IWinnerRepository : IRepository<Winner>
    {
        IList<LastWinnersViewModel> GetLastWinners();
        IList<UserWinnerViewModel> GetWinners(string userId);
        Winner GetWinnerLast(Int64 id);
        long GetWinnersCount();
        long GetWinnersCount(Int64 caseId);
        List<SuperSkinViewModel> GetDropRate(Int64 caseId);
    }

    public class WinnerRepository : Repository<Winner>, IWinnerRepository
    {
        public WinnerRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IList<LastWinnersViewModel> GetLastWinners()
        {
            return _dbSet
                .Include(x => x.User)
                .Include(x => x.Skin)
                .Include(x => x.Case)
                .OrderByDescending(x => x.DateCreate)
                .Select(x => new LastWinnersViewModel()
                {
                    user_name = x.User.SteamUsername,
                    winner_id = x.Id,
                    skin_name = x.Skin.MarketHashName,
                    skin_rarity = x.Skin.Rarity,
                    skin_image = x.Skin.Image,
                    case_name = x.Case.FullName,
                    case_static_name = x.Case.StaticName
                })
                .Take(8)
                .ToList();
        }

        public IList<UserWinnerViewModel> GetWinners(string userId)
        {
            return _dbSet
                .Where(x => x.User.Id == userId)
                .Include(x => x.Skin)
                .Select(x => new UserWinnerViewModel()
                {
                    Id = x.Id,
                    DateCreate = x.DateCreate,
                    MarketHashName = x.Skin.MarketHashName,
                    Image = x.Skin.Image,
                    State = x.State,
                    Price = x.Skin.Price * 0.8M,
                }).OrderByDescending(x => x.DateCreate).ToList();
        }

        public Winner GetWinnerLast(Int64 id)
        {
            return _dbSet
                .Include(x => x.Skin)
                .Include(x => x.User)
                .LastOrDefault(x => x.Id == id);
        }

        public long GetWinnersCount()
        {
            return _dbSet.LongCount();
        }

        public long GetWinnersCount(Int64 caseId)
        {
            return _dbSet.Where(x => x.Case.Id == caseId).LongCount();
        }

        public List<SuperSkinViewModel> GetDropRate(Int64 caseId)
        {
            var totalWins = this.GetWinnersCount(caseId);

            return _dbSet
                .Where(x => x.Case.Id == caseId)
                .GroupBy(x => x.Skin)
                .Select(x => new SuperSkinViewModel()
                {
                    MarketHashName = x.Key.MarketHashName,
                    Id = x.Key.Id,
                    Image = x.Key.Image,
                    Price = x.Key.Price,
                    Count = x.Count(),
                    Sum = x.Sum(s => s.Skin.Price),
                    Chance = (float)x.Count() / totalWins
                }).ToList();
        }
    }
}
