using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace test_app.api.Migrations
{
    public partial class v23 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SteamAnalystUrl",
                table: "Skins",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SteamUrl",
                table: "Skins",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SteamAnalystUrl",
                table: "Skins");

            migrationBuilder.DropColumn(
                name: "SteamUrl",
                table: "Skins");
        }
    }
}
