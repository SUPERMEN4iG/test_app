using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using test_app.shared.Data;

namespace test_app.shared.Repositories
{
    public interface IG2ARepository : IRepository<G2APayment>
    {
        void AddIpnLog(string request, string response);
        G2APayment GetPayment(long id);
    }

    public class G2ARepository : Repository<G2APayment>, IG2ARepository
    {
        public G2ARepository(ApplicationDbContext context) : base(context) { }

        public void AddIpnLog(string request, string response)
        {
            ((ApplicationDbContext)_dbContext).Add(new G2AIPNLog() { Request = request, Response = response });
        }

        public G2APayment GetPayment(long id)
        {
            return _dbSet
                .Include(x => x.User)
                .FirstOrDefault(x => x.Id == id);
        }
    }
}
