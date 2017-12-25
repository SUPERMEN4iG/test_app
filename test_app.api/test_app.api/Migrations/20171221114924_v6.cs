using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace test_app.api.Migrations
{
    public partial class v6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MarketHashName",
                table: "Skins",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Skins_Id_MarketHashName",
                table: "Skins",
                columns: new[] { "Id", "MarketHashName" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Skins_Id_MarketHashName",
                table: "Skins");

            migrationBuilder.AlterColumn<string>(
                name: "MarketHashName",
                table: "Skins",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
