using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using test_app.shared.Data;
using test_app.shared.ViewModels;

namespace test_app.shared.Repositories
{
    public interface IBotRepository : IRepository<Bot>
    {
        bool Any(long botId);
        List<PurshaseBotQueue> GetQueue(int count);
        List<PurshaseBotQueue> GetQueue(List<InsertNewPurchaseViewModel> list);
    }

    public class BotRepository : Repository<Bot>, IBotRepository
    {
        public BotRepository(ApplicationDbContext context) : base(context) { }

        public bool Any(long botId)
        {
            return _dbSet.Any(x => x.Id == botId);
        }

        public List<PurshaseBotQueue> GetQueue(int count)
        {
            return ((ApplicationDbContext)_dbContext)
                .PurshaseBotQueues
                .OrderBy(x => x.TriesCount)
                .Take(count)
                .ToList();
        }

        public List<PurshaseBotQueue> GetQueue(List<InsertNewPurchaseViewModel> list)
        {
            return ((ApplicationDbContext)_dbContext)
                .PurshaseBotQueues
                .Where(q => list.Any(x => x.queue_id == q.Id))
                .ToList();
        }
    }
}
