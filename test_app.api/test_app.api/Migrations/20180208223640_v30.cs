using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace test_app.api.Migrations
{
    public partial class v30 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BotsPurcasesFullHistory",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BotId = table.Column<long>(nullable: true),
                    BoughtAt = table.Column<DateTime>(nullable: false),
                    DateCreate = table.Column<DateTime>(nullable: false),
                    ListedAt = table.Column<DateTime>(nullable: false),
                    MarketHashName = table.Column<string>(nullable: true),
                    Platform = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BotsPurcasesFullHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BotsPurcasesFullHistory_Bots_BotId",
                        column: x => x.BotId,
                        principalTable: "Bots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BotsPurcasesFullHistory_BotId",
                table: "BotsPurcasesFullHistory",
                column: "BotId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BotsPurcasesFullHistory");
        }
    }
}
