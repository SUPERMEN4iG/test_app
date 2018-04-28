using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore;
using test_app.shared.Data;
using test_app.shared.ViewModels;

namespace test_app.shared.Repositories
{
    public interface ICaseDropRepository : IRepository<CasesDrop>
    {
    }

    public class CaseDropRepository : Repository<CasesDrop>, ICaseDropRepository
    {
        public CaseDropRepository(ApplicationDbContext context) : base(context) { }
    }
}
