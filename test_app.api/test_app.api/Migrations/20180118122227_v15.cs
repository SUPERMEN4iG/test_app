using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace test_app.api.Migrations
{
    public partial class v15 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rarity",
                table: "StackCases");

            migrationBuilder.AddColumn<string>(
                name: "Rarity",
                table: "Skins",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rarity",
                table: "Skins");

            migrationBuilder.AddColumn<string>(
                name: "Rarity",
                table: "StackCases",
                nullable: true);
        }
    }
}
