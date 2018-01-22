using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace test_app.api.Migrations
{
    public partial class v22 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSent",
                table: "Winners");

            migrationBuilder.DropColumn(
                name: "IsSold",
                table: "Winners");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Winners",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Winners",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "StockId",
                table: "Winners",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Stock",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BotId = table.Column<long>(nullable: true),
                    DateCreate = table.Column<DateTime>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    SkinId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stock", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stock_Bots_BotId",
                        column: x => x.BotId,
                        principalTable: "Bots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Stock_Skins_SkinId",
                        column: x => x.SkinId,
                        principalTable: "Skins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Winners_StockId",
                table: "Winners",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_Stock_BotId",
                table: "Stock",
                column: "BotId");

            migrationBuilder.CreateIndex(
                name: "IX_Stock_SkinId",
                table: "Stock",
                column: "SkinId");

            migrationBuilder.AddForeignKey(
                name: "FK_Winners_Stock_StockId",
                table: "Winners",
                column: "StockId",
                principalTable: "Stock",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Winners_Stock_StockId",
                table: "Winners");

            migrationBuilder.DropTable(
                name: "Stock");

            migrationBuilder.DropIndex(
                name: "IX_Winners_StockId",
                table: "Winners");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Winners");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Winners");

            migrationBuilder.DropColumn(
                name: "StockId",
                table: "Winners");

            migrationBuilder.AddColumn<bool>(
                name: "IsSent",
                table: "Winners",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSold",
                table: "Winners",
                nullable: false,
                defaultValue: false);
        }
    }
}
