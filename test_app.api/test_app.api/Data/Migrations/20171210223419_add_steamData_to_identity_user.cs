using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace test_app.api.Data.Migrations
{
    public partial class add_steamData_to_identity_user : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DisplayName",
                table: "AspNetUsers",
                newName: "SteamUsername");

            migrationBuilder.AddColumn<string>(
                name: "SteamAvatar",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SteamId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SteamProfileState",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SteamAvatar",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SteamId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SteamProfileState",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "SteamUsername",
                table: "AspNetUsers",
                newName: "DisplayName");
        }
    }
}
