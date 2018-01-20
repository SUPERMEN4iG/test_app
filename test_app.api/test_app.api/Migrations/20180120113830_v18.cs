using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace test_app.api.Migrations
{
    public partial class v18 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "CaseCategories",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                table: "CaseCategories");
        }
    }
}
