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
                    new CaseCategory() { StaticName = "rare-cases", FullName = "Rare Cases", Index = 1 },
                    new CaseCategory() { StaticName = "standarts-cases", FullName = "Standarts Cases", Index = 2 },
                    new CaseCategory() { StaticName = "regular-cases", FullName = "Regular Cases", Index = 3 },
                    new CaseCategory() { StaticName = "exclusive-cases", FullName = "Exclusive Cases", Index = 4 }
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
                        FullName = "Rare Case 1",
                        StaticName = "rare-case-1",
                        Image = "/assets/images/cases/regular_case_gold.png",
                        Price = new decimal(1.00),
                        Index = 1,
                        IsAvalible = true,
                        Category = categories[0]
                    },
                    new Case()
                    {
                        FullName = "Rare Case 2",
                        StaticName = "rare-case-2",
                        Image = "/assets/images/cases/regular_case_gold.png",
                        Price = new decimal(1.00),
                        Index = 2,
                        IsAvalible = true,
                        Category = categories[0]
                    },
                    new Case()
                    {
                        FullName = "Rare Case 3",
                        StaticName = "rare-case-3",
                        Image = "/assets/images/cases/regular_case_gold.png",
                        Price = new decimal(3.00),
                        Index = 3,
                        IsAvalible = true,
                        Category = categories[0]
                    },
                    new Case()
                    {
                        FullName = "Rare Case 4",
                        StaticName = "rare-case-4",
                        Image = "/assets/images/cases/regular_case_gold.png",
                        Price = new decimal(4.00),
                        Index = 4,
                        IsAvalible = true,
                        Category = categories[0]
                    },

                    new Case()
                    {
                        FullName = "Standarts Case 1",
                        StaticName = "standarts-case-1",
                        Image = "/assets/images/cases/stand_case_01.png",
                        Price = new decimal(1.00),
                        Index = 1,
                        IsAvalible = true,
                        Category = categories[1]
                    },
                    new Case()
                    {
                        FullName = "Standarts Case 2",
                        StaticName = "standarts-case-2",
                        Image = "/assets/images/cases/stand_case_01.png",
                        Price = new decimal(2.00),
                        Index = 2,
                        IsAvalible = true,
                        Category = categories[1]
                    },
                    new Case()
                    {
                        FullName = "Standarts Case 3",
                        StaticName = "standarts-case-3",
                        Image = "/assets/images/cases/stand_case_01.png",
                        Price = new decimal(3.00),
                        Index = 3,
                        IsAvalible = true,
                        Category = categories[1]
                    },
                    new Case()
                    {
                        FullName = "Standarts Case 4",
                        StaticName = "standarts-case-4",
                        Image = "/assets/images/cases/stand_case_01.png",
                        Price = new decimal(4.00),
                        Index = 4,
                        IsAvalible = true,
                        Category = categories[1]
                    },
                    new Case()
                    {
                        FullName = "Standarts Case 5",
                        StaticName = "standarts-case-5",
                        Image = "/assets/images/cases/stand_case_01.png",
                        Price = new decimal(5.00),
                        Index = 5,
                        IsAvalible = true,
                        Category = categories[1]
                    },
                    new Case()
                    {
                        FullName = "Standarts Case 6",
                        StaticName = "standarts-case-6",
                        Image = "/assets/images/cases/stand_case_01.png",
                        Price = new decimal(6.00),
                        Index = 6,
                        IsAvalible = true,
                        Category = categories[1]
                    },
                    new Case()
                    {
                        FullName = "Standarts Case 7",
                        StaticName = "standarts-case-7",
                        Image = "/assets/images/cases/stand_case_01.png",
                        Price = new decimal(7.00),
                        Index = 7,
                        IsAvalible = true,
                        Category = categories[1]
                    },
                    new Case()
                    {
                        FullName = "Standarts Case 8",
                        StaticName = "standarts-case-8",
                        Image = "/assets/images/cases/stand_case_01.png",
                        Price = new decimal(8.00),
                        Index = 8,
                        IsAvalible = true,
                        Category = categories[1]
                    },

                    new Case()
                    {
                        FullName = "Regular Case 1",
                        StaticName = "regular-case-1",
                        Image = "/assets/images/cases/regular_case_gold.png",
                        Price = new decimal(1.00),
                        Index = 1,
                        IsAvalible = true,
                        Category = categories[2]
                    },
                    new Case()
                    {
                        FullName = "Regular Case 2",
                        StaticName = "regular-case-2",
                        Image = "/assets/images/cases/regular_case_blue.png",
                        Price = new decimal(2.00),
                        Index = 2,
                        IsAvalible = true,
                        Category = categories[2]
                    },
                    new Case()
                    {
                        FullName = "Regular Case 3",
                        StaticName = "regular-case-3",
                        Image = "/assets/images/cases/regular_case_purple.png",
                        Price = new decimal(3.00),
                        Index = 3,
                        IsAvalible = true,
                        Category = categories[2]
                    },
                    new Case()
                    {
                        FullName = "Regular Case 4",
                        StaticName = "regular-case-4",
                        Image = "/assets/images/cases/regular_case_red.png",
                        Price = new decimal(4.00),
                        Index = 4,
                        IsAvalible = true,
                        Category = categories[2]
                    },

                    new Case()
                    {
                        FullName = "Exclusive Case 1",
                        StaticName = "exclusive-case-1",
                        Image = "/assets/images/cases/regular_case_gold.png",
                        Price = new decimal(1.00),
                        Index = 1,
                        IsAvalible = true,
                        Category = categories[3]
                    },
                    new Case()
                    {
                        FullName = "Exclusive Case 2",
                        StaticName = "exclusive-case-2",
                        Image = "/assets/images/cases/regular_case_gold.png",
                        Price = new decimal(2.00),
                        Index = 2,
                        IsAvalible = true,
                        Category = categories[3]
                    },
                    new Case()
                    {
                        FullName = "Exclusive Case 3",
                        StaticName = "exclusive-case-3",
                        Image = "/assets/images/cases/regular_case_gold.png",
                        Price = new decimal(3.00),
                        Index = 3,
                        IsAvalible = true,
                        Category = categories[3]
                    },
                    new Case()
                    {
                        FullName = "Exclusive Case 4",
                        StaticName = "exclusive-case-4",
                        Image = "/assets/images/cases/regular_case_gold.png",
                        Price = new decimal(4.00),
                        Index = 4,
                        IsAvalible = true,
                        Category = categories[3]
                    },
                    new Case()
                    {
                        FullName = "Exclusive Case 5",
                        StaticName = "exclusive-case-5",
                        Image = "/assets/images/cases/regular_case_gold.png",
                        Price = new decimal(5.00),
                        Index = 5,
                        IsAvalible = true,
                        Category = categories[3]
                    },
                    new Case()
                    {
                        FullName = "Exclusive Case 6",
                        StaticName = "exclusive-case-6",
                        Image = "/assets/images/cases/regular_case_gold.png",
                        Price = new decimal(6.00),
                        Index = 6,
                        IsAvalible = true,
                        Category = categories[3]
                    },
                    new Case()
                    {
                        FullName = "Exclusive Case 7",
                        StaticName = "exclusive-case-7",
                        Image = "/assets/images/cases/regular_case_gold.png",
                        Price = new decimal(7.00),
                        Index = 7,
                        IsAvalible = true,
                        Category = categories[3]
                    },
                    new Case()
                    {
                        FullName = "Exclusive Case 8",
                        StaticName = "exclusive-case-8",
                        Image = "/assets/images/cases/regular_case_gold.png",
                        Price = new decimal(8.00),
                        Index = 8,
                        IsAvalible = true,
                        Category = categories[3]
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

                var casesDrop = new List<CasesDrop>();
                var index = 0;
                cases.ToList().ForEach((c) =>
                {
                    casesDrop.AddRange(new List<CasesDrop>() {
                        new CasesDrop()
                        {
                            Case = cases[index],
                            Skin = skins[0],
                            Chance = new decimal(0.5),
                        },
                        new CasesDrop()
                        {
                            Case = cases[index],
                            Skin = skins[1],
                            Chance = new decimal(0.5),
                        }
                    });
                    index++;
                });

                context.AddRange(casesDrop);

                context.SaveChanges();
            }
        }
}
}
