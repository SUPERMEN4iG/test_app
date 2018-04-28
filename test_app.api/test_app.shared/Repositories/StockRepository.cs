using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using test_app.shared.Data;

namespace test_app.shared.Repositories
{
    public interface IStockRepository : IRepository<Stock>
    {
    }

    public class StockRepository : Repository<Stock>, IStockRepository
    {
        public StockRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
