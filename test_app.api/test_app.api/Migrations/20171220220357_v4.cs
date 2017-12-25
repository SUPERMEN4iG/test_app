using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace test_app.api.Migrations
{
    public partial class v4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PurshaseBotQueues",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreate = table.Column<DateTime>(nullable: false),
                    DateLastRequest = table.Column<DateTime>(nullable: false),
                    LastBotId = table.Column<long>(nullable: true),
                    Locked = table.Column<bool>(nullable: false),
                    MarketHashName = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurshaseBotQueues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurshaseBotQueues_Bots_LastBotId",
                        column: x => x.LastBotId,
                        principalTable: "Bots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurshaseBotQueues_Skins_MarketHashName",
                        column: x => x.MarketHashName,
                        principalTable: "Skins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurshaseBotQueues_LastBotId",
                table: "PurshaseBotQueues",
                column: "LastBotId");

            migrationBuilder.CreateIndex(
                name: "IX_PurshaseBotQueues_MarketHashName",
                table: "PurshaseBotQueues",
                column: "MarketHashName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurshaseBotQueues");
        }
    }
}
