using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using test_app.shared.Data;
using System.Diagnostics;
using test_app.shared.ViewModels;

namespace test_app.shared.Repositories
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        UserProfileViewModel GetUserById(string id);
        long GetUsersCount();
    }

    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
            Debug.WriteLine("ctr()");
        }

        public UserProfileViewModel GetUserById(string id)
        {
            return _dbSet
                .Where(x => x.Id == id).Select(x => new UserProfileViewModel()
                {
                    Id = x.Id,
                    SteamAvatar = x.SteamAvatar,
                    Email = x.Email,
                    UserName = x.UserName,
                    TradeofferUrl = x.TradeofferUrl,
                    WonItems = new List<UserWinnerViewModel>()
                }).FirstOrDefault();
        }

        public long GetUsersCount()
        {
            return this._dbSet.LongCount();
        }
    }
}
