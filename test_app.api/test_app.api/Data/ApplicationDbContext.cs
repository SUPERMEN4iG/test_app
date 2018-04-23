using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Expressions;
using test_app.api.Helper;
using test_app.api.Models;

namespace test_app.api.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Application> Applications { get; set; }

        public DbSet<Case> Cases { get; set; }

        public DbSet<Skin> Skins { get; set; }

        public DbSet<CaseCategory> CaseCategories { get; set; }

        public DbSet<StackCase> StackCases { get; set; }

        public DbSet<StackCaseSkin> StackCaseSkins { get; set; }

        public DbSet<Winner> Winners { get; set; }

        public DbSet<Bot> Bots { get; set; }

        public DbSet<CaseFaultLog> CaseFaultLogs { get; set; }

        public DbSet<CaseSellLog> CaseSellLogs { get; set; }

        public DbSet<CaseDiscount> CaseDiscounts { get; set; }

        public DbSet<BotTradeoffer> BotTradeoffers { get; set; }

        public DbSet<PurshaseBotQueue> PurshaseBotQueues { get; set; }

        public DbSet<PurchasebotPurchases> PurchasebotPurchases { get; set; }

        public DbSet<BotsPurcasesFullHistory> BotsPurcasesFullHistory { get; set; }

        public DbSet<CasesDrop> CasesDrops { get; set; }

        public DbSet<Stock> Stock { get; set; }

        public DbSet<G2APayment> G2APayments { get; set; }

        public DbSet<G2AIPNLog> G2AIPNLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDbFunction(typeof(DbUtility)
                .GetMethod(nameof(DbUtility.DateDiff)))
                .HasTranslation(args => {
                    var newArgs = args.ToArray();
                    newArgs[0] = new SqlFragmentExpression((string)((ConstantExpression)newArgs[0]).Value);
                    return new SqlFunctionExpression(
                        "DATEDIFF",
                        typeof(int),
                        newArgs);
                });

            // TODO: Укажем маппинг явно, надо сделать через Assembly..
            builder.AddConfiguration(new Bot.BotConfiguration());
            builder.AddConfiguration(new BotsPurcasesFullHistory.BotsPurcasesFullHistoryConfiguration());
            builder.AddConfiguration(new BotTradeoffer.BotTradeofferConfiguration());
            builder.AddConfiguration(new Case.CaseConfiguration());
            builder.AddConfiguration(new CaseCategory.CaseCategoryConfiguration());
            builder.AddConfiguration(new CaseDiscount.CaseDiscountConfiguration());
            builder.AddConfiguration(new CaseFaultLog.CaseFaultLogConfiguration());
            builder.AddConfiguration(new CasesDrop.CasesDropConfiguration());
            builder.AddConfiguration(new CaseSellLog.CaseSellLogConfiguration());
            builder.AddConfiguration(new G2AIPNLog.G2AIPNLogConfiguration());
            builder.AddConfiguration(new G2APayment.G2APaymentConfiguration());
            builder.AddConfiguration(new PurchasebotPurchases.PurchasebotPurchasesConfiguration());
            builder.AddConfiguration(new PurshaseBotQueue.PurshaseBotQueueConfiguration());
            builder.AddConfiguration(new Skin.SkinConfiguration());
            builder.AddConfiguration(new StackCase.StackCaseConfiguration());
            builder.AddConfiguration(new StackCaseSkin.StackCaseSkinConfiguration());
            builder.AddConfiguration(new Stock.StockConfiguration());
            builder.AddConfiguration(new Winner.WinnerConfiguration());

            //builder.Entity<PurshaseBotQueue>()
            //    .HasOne(x => x.Skin)
            //    .WithOne().HasForeignKey(typeof(Skin), "MarketHashName");

            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
