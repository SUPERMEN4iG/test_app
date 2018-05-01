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
    public interface ICaseCategoryRepository : IRepository<CaseCategory>
    {

    }

    public class CaseCategoryRepository : Repository<CaseCategory>, ICaseCategoryRepository
    {
        public CaseCategoryRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
