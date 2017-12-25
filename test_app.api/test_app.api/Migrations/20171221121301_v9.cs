using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace test_app.api.Migrations
{
    public partial class v9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurshaseBotQueues_Skins_MarketHashName",
                table: "PurshaseBotQueues");

            migrationBuilder.RenameColumn(
                name: "MarketHashName",
                table: "PurshaseBotQueues",
                newName: "SkinId");

            migrationBuilder.RenameIndex(
                name: "IX_PurshaseBotQueues_MarketHashName",
                table: "PurshaseBotQueues",
                newName: "IX_PurshaseBotQueues_SkinId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurshaseBotQueues_Skins_SkinId",
                table: "PurshaseBotQueues",
                column: "SkinId",
                principalTable: "Skins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurshaseBotQueues_Skins_SkinId",
                table: "PurshaseBotQueues");

            migrationBuilder.RenameColumn(
                name: "SkinId",
                table: "PurshaseBotQueues",
                newName: "MarketHashName");

            migrationBuilder.RenameIndex(
                name: "IX_PurshaseBotQueues_SkinId",
                table: "PurshaseBotQueues",
                newName: "IX_PurshaseBotQueues_MarketHashName");

            migrationBuilder.AddForeignKey(
                name: "FK_PurshaseBotQueues_Skins_MarketHashName",
                table: "PurshaseBotQueues",
                column: "MarketHashName",
                principalTable: "Skins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
