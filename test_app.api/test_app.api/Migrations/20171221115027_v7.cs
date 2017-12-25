using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace test_app.api.Migrations
{
    public partial class v7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Skins_Id_MarketHashName",
                table: "Skins");

            migrationBuilder.CreateIndex(
                name: "IX_Skins_Id_MarketHashName",
                table: "Skins",
                columns: new[] { "Id", "MarketHashName" },
                unique: true,
                filter: "[MarketHashName] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Skins_Id_MarketHashName",
                table: "Skins");

            migrationBuilder.CreateIndex(
                name: "IX_Skins_Id_MarketHashName",
                table: "Skins",
                columns: new[] { "Id", "MarketHashName" });
        }
    }
}
