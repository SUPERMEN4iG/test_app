using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace test_app.api.Migrations
{
    public partial class v3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bots",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreate = table.Column<DateTime>(nullable: false),
                    IsAdminsOnly = table.Column<bool>(nullable: false),
                    IsAwaiting = table.Column<bool>(nullable: false),
                    IsHidden = table.Column<bool>(nullable: false),
                    Login = table.Column<string>(nullable: true),
                    Server = table.Column<string>(nullable: true),
                    SteamId = table.Column<string>(nullable: true),
                    SyncTime = table.Column<DateTime>(nullable: false),
                    TradeOffer = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CaseDiscounts",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CaseId = table.Column<long>(nullable: true),
                    DateCreate = table.Column<DateTime>(nullable: false),
                    Percent = table.Column<decimal>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseDiscounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaseDiscounts_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CaseDiscounts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CaseFaultLogs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CaseId = table.Column<long>(nullable: true),
                    DateCreate = table.Column<DateTime>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseFaultLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaseFaultLogs_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CaseFaultLogs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BotTradeoffers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BotId = table.Column<long>(nullable: true),
                    DateCreate = table.Column<DateTime>(nullable: false),
                    DateExpiration = table.Column<DateTime>(nullable: false),
                    DateInsertion = table.Column<DateTime>(nullable: false),
                    DateUpdate = table.Column<DateTime>(nullable: false),
                    ItemsToGive = table.Column<long>(nullable: false),
                    SteamIdOther = table.Column<string>(nullable: true),
                    TradeOfferState = table.Column<int>(nullable: false),
                    TradeofferId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BotTradeoffers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BotTradeoffers_Bots_BotId",
                        column: x => x.BotId,
                        principalTable: "Bots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BotTradeoffers_BotId",
                table: "BotTradeoffers",
                column: "BotId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseDiscounts_CaseId",
                table: "CaseDiscounts",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseDiscounts_UserId",
                table: "CaseDiscounts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseFaultLogs_CaseId",
                table: "CaseFaultLogs",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseFaultLogs_UserId",
                table: "CaseFaultLogs",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BotTradeoffers");

            migrationBuilder.DropTable(
                name: "CaseDiscounts");

            migrationBuilder.DropTable(
                name: "CaseFaultLogs");

            migrationBuilder.DropTable(
                name: "Bots");
        }
    }
}
