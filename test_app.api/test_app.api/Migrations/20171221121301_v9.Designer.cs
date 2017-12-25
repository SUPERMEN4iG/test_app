﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using test_app.api.Data;

namespace test_app.api.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20171221121301_v9")]
    partial class v9
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("test_app.api.Data.Application", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("DailyBonusCaseId");

                    b.Property<DateTime>("DateCreate");

                    b.Property<bool>("IsInitialized");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Applications");
                });

            modelBuilder.Entity("test_app.api.Data.Bot", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreate");

                    b.Property<bool>("IsAdminsOnly");

                    b.Property<bool>("IsAwaiting");

                    b.Property<bool>("IsHidden");

                    b.Property<string>("Login");

                    b.Property<string>("Server");

                    b.Property<string>("SteamId");

                    b.Property<DateTime>("SyncTime");

                    b.Property<string>("TradeOffer");

                    b.HasKey("Id");

                    b.ToTable("Bots");
                });

            modelBuilder.Entity("test_app.api.Data.BotTradeoffer", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("BotId");

                    b.Property<DateTime>("DateCreate");

                    b.Property<DateTime>("DateExpiration");

                    b.Property<DateTime>("DateInsertion");

                    b.Property<DateTime>("DateUpdate");

                    b.Property<long>("ItemsToGive");

                    b.Property<string>("SteamIdOther");

                    b.Property<int>("TradeOfferState");

                    b.Property<long>("TradeofferId");

                    b.HasKey("Id");

                    b.HasIndex("BotId");

                    b.ToTable("BotTradeoffers");
                });

            modelBuilder.Entity("test_app.api.Data.Case", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("CategoryId");

                    b.Property<DateTime>("DateCreate");

                    b.Property<string>("Image");

                    b.Property<int>("Index");

                    b.Property<bool>("IsAvalible");

                    b.Property<decimal>("Price");

                    b.Property<string>("StaticName");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Cases");
                });

            modelBuilder.Entity("test_app.api.Data.CaseCategory", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreate");

                    b.Property<string>("StaticName");

                    b.HasKey("Id");

                    b.ToTable("CaseCategories");
                });

            modelBuilder.Entity("test_app.api.Data.CaseDiscount", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("CaseId");

                    b.Property<DateTime>("DateCreate");

                    b.Property<decimal>("Percent");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("CaseId");

                    b.HasIndex("UserId");

                    b.ToTable("CaseDiscounts");
                });

            modelBuilder.Entity("test_app.api.Data.CaseFaultLog", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("CaseId");

                    b.Property<DateTime>("DateCreate");

                    b.Property<string>("Text");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("CaseId");

                    b.HasIndex("UserId");

                    b.ToTable("CaseFaultLogs");
                });

            modelBuilder.Entity("test_app.api.Data.CasesDrop", b =>
                {
                    b.Property<long>("CaseId");

                    b.Property<long>("SkinId");

                    b.Property<DateTime>("DateCreate");

                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.HasKey("CaseId", "SkinId");

                    b.HasAlternateKey("Id");

                    b.HasIndex("SkinId");

                    b.ToTable("CasesDrop");
                });

            modelBuilder.Entity("test_app.api.Data.PurshaseBotQueue", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreate");

                    b.Property<DateTime>("DateLastRequest");

                    b.Property<long?>("LastBotId");

                    b.Property<bool>("Locked");

                    b.Property<long?>("SkinId");

                    b.HasKey("Id");

                    b.HasIndex("LastBotId");

                    b.HasIndex("SkinId");

                    b.ToTable("PurshaseBotQueues");
                });

            modelBuilder.Entity("test_app.api.Data.Skin", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreate");

                    b.Property<string>("Image");

                    b.Property<string>("MarketHashName");

                    b.Property<decimal>("Price");

                    b.Property<long?>("StackCaseId");

                    b.HasKey("Id");

                    b.HasIndex("StackCaseId");

                    b.HasIndex("Id", "MarketHashName")
                        .IsUnique()
                        .HasFilter("[MarketHashName] IS NOT NULL");

                    b.ToTable("Skins");
                });

            modelBuilder.Entity("test_app.api.Data.StackCase", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreate");

                    b.Property<string>("Image");

                    b.Property<decimal>("Price");

                    b.Property<string>("RarityString")
                        .HasColumnName("Rarity");

                    b.HasKey("Id");

                    b.ToTable("StackCases");
                });

            modelBuilder.Entity("test_app.api.Data.Winner", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("CaseId");

                    b.Property<DateTime>("DateCreate");

                    b.Property<bool>("IsSent");

                    b.Property<bool>("IsSold");

                    b.Property<long?>("SkinId");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("CaseId");

                    b.HasIndex("SkinId");

                    b.HasIndex("UserId");

                    b.ToTable("Winners");
                });

            modelBuilder.Entity("test_app.api.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<string>("SteamAvatar");

                    b.Property<string>("SteamId");

                    b.Property<int>("SteamProfileState");

                    b.Property<string>("SteamUsername");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("test_app.api.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("test_app.api.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("test_app.api.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("test_app.api.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("test_app.api.Data.BotTradeoffer", b =>
                {
                    b.HasOne("test_app.api.Data.Bot", "Bot")
                        .WithMany()
                        .HasForeignKey("BotId");
                });

            modelBuilder.Entity("test_app.api.Data.Case", b =>
                {
                    b.HasOne("test_app.api.Data.CaseCategory", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId");
                });

            modelBuilder.Entity("test_app.api.Data.CaseDiscount", b =>
                {
                    b.HasOne("test_app.api.Data.Case", "Case")
                        .WithMany()
                        .HasForeignKey("CaseId");

                    b.HasOne("test_app.api.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("test_app.api.Data.CaseFaultLog", b =>
                {
                    b.HasOne("test_app.api.Data.Case", "Case")
                        .WithMany()
                        .HasForeignKey("CaseId");

                    b.HasOne("test_app.api.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("test_app.api.Data.CasesDrop", b =>
                {
                    b.HasOne("test_app.api.Data.Case", "Case")
                        .WithMany("CaseSkins")
                        .HasForeignKey("CaseId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("test_app.api.Data.Skin", "Skin")
                        .WithMany("CaseSkins")
                        .HasForeignKey("SkinId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("test_app.api.Data.PurshaseBotQueue", b =>
                {
                    b.HasOne("test_app.api.Data.Bot", "LastBot")
                        .WithMany()
                        .HasForeignKey("LastBotId");

                    b.HasOne("test_app.api.Data.Skin", "Skin")
                        .WithMany()
                        .HasForeignKey("SkinId");
                });

            modelBuilder.Entity("test_app.api.Data.Skin", b =>
                {
                    b.HasOne("test_app.api.Data.StackCase")
                        .WithMany("Skins")
                        .HasForeignKey("StackCaseId");
                });

            modelBuilder.Entity("test_app.api.Data.Winner", b =>
                {
                    b.HasOne("test_app.api.Data.Case", "Case")
                        .WithMany()
                        .HasForeignKey("CaseId");

                    b.HasOne("test_app.api.Data.Skin", "Skin")
                        .WithMany()
                        .HasForeignKey("SkinId");

                    b.HasOne("test_app.api.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });
#pragma warning restore 612, 618
        }
    }
}
