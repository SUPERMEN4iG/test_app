using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test_app.api.Data
{
    public static class ApplicationDbContextExtensions
    {
        /// <summary>
        /// Инициалзиация приложения
        /// </summary>
        /// <param name="context"></param>
        public static void EnsureSeedData(this ApplicationDbContext context)
        {
            if (context.AllMigrationsApplied() && context.AllMigrations() >= 1)
            {
                if (context.Applications.Any((x) => x.Name == "TEST_APP" && x.IsInitialized == true)) return;

                var app = new Application()
                {
                    Name = "TEST_APP",
                    DailyBonusCaseId = 2,
                    IsInitialized = true
                };

                context.AddRange(app);

                var categories = new[]
                {
                    new CaseCategory() { StaticName = "our-cases" },
                };

                var skins = new[]
                {
                    new Skin()
                    {
                        MarketHashName = "T-shirt (Red)",
                        Image = "https://steamcommunity-a.akamaihd.net/economy/image/8HAGSsiO9OXk0bu4o76O6xabNUY8RRLf00e56zWT3IZUH8Flab9goIFna_837oFuZVQtrmh23qr2o44kS6-MLaIGhQ",
                        Price = new decimal(0.04)
                    },

                    new Skin()
                    {
                        MarketHashName = "Combat Pants (White)",
                        Image = "https://steamcommunity-a.akamaihd.net/economy/image/8HAGSsiO9OXk0bu4o76O6xabNUY8RRLf00e56zWT3IZUH8Flab9goIFna_837oFuZVQtrmh13qr2rY4kS6_jKn_piQ",
                        Price = new decimal(0.05)
                    },
                };

                var cases = new[]
                {
                    new Case()
                    {
                        StaticName = "Test case 1",
                        Image = "https://s3.amazonaws.com/pubgboxes/public/assets/images/twitch_crate_basel.png",
                        Price = new decimal(0.01),
                        Index = 1,
                        IsAvalible = true,
                        Category = categories[0]
                    },
                };

                var stackCases = new[]
                {
                    new StackCase()
                    {
                        Name = "Test stack 1",
                        Skins = skins,
                    },
                };

                context.AddRange(stackCases);

                context.AddRange(
                    new CasesDrop()
                    {
                        Case = cases[0],
                        Skin = skins[0],
                        Chance = new decimal(0.02),
                    },
                    new CasesDrop()
                    {
                        Case = cases[0],
                        Skin = skins[1],
                        Chance = new decimal(0.03),
                    });

                context.SaveChanges();
            }
        }
}
}
