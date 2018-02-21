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

            builder.Entity<Skin>()
                .HasIndex(p => new { p.Id, p.MarketHashName }).IsUnique();

            builder.Entity<CasesDrop>()
                .HasKey(t => new { t.CaseId, t.SkinId });

            builder.Entity<CasesDrop>()
                .HasOne(cs => cs.Skin)
                .WithMany(c => c.CaseSkins)
                .HasForeignKey(cs => cs.SkinId);

            builder.Entity<CasesDrop>()
                .HasOne(cs => cs.Skin)
                .WithMany(c => c.CaseSkins)
                .HasForeignKey(cs => cs.SkinId);

            builder.Entity<CasesDrop>()
                .Property(x => x.Chance).HasColumnType("decimal(9, 8)");

            builder.Entity<StackCaseSkin>()
                .HasKey(t => new { t.StackCaseId, t.SkinId });

            builder.Entity<StackCaseSkin>()
                .HasOne(cs => cs.Skin)
                .WithMany(c => c.StackCaseSkins)
                .HasForeignKey(cs => cs.SkinId);

            builder.Entity<StackCaseSkin>()
                .HasOne(cs => cs.Skin)
                .WithMany(c => c.StackCaseSkins)
                .HasForeignKey(cs => cs.SkinId);

            //builder.Entity<PurshaseBotQueue>()
            //    .HasOne(x => x.Skin)
            //    .WithOne().HasForeignKey(typeof(Skin), "MarketHashName");

            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
