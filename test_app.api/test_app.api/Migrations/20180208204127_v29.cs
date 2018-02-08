using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace test_app.api.Migrations
{
    public partial class v29 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PurchasebotPurchases",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BotId = table.Column<long>(nullable: true),
                    BotQueueId = table.Column<long>(nullable: true),
                    DateCreate = table.Column<DateTime>(nullable: false),
                    MarketHashName = table.Column<string>(nullable: true),
                    Platform = table.Column<string>(nullable: true),
                    PriceUSD = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchasebotPurchases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchasebotPurchases_Bots_BotId",
                        column: x => x.BotId,
                        principalTable: "Bots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchasebotPurchases_PurshaseBotQueues_BotQueueId",
                        column: x => x.BotQueueId,
                        principalTable: "PurshaseBotQueues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchasebotPurchases_BotId",
                table: "PurchasebotPurchases",
                column: "BotId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasebotPurchases_BotQueueId",
                table: "PurchasebotPurchases",
                column: "BotQueueId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchasebotPurchases");
        }
    }
}
