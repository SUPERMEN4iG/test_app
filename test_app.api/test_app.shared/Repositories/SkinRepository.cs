using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using test_app.shared.Data;
using test_app.shared.ViewModels;

namespace test_app.shared.Repositories
{
    public interface ISkinRepository : IRepository<Skin>
    {
        IList<SkinsViewModel> GetSkins(Func<Skin, SkinsViewModel> outputSkins);
    }

    public class SkinRepository : Repository<Skin>, ISkinRepository
    {
        public SkinRepository(ApplicationDbContext context) : base(context) { }

        public IList<SkinsViewModel> GetSkins(Func<Skin, SkinsViewModel> outputSkins)
        {
            return _dbSet
                .Select(outputSkins)
                .ToList();
        }
    }
}
