using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace test_app.api.Migrations
{
    public partial class v27 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurshaseBotQueues_Skins_SkinId",
                table: "PurshaseBotQueues");

            migrationBuilder.DropIndex(
                name: "IX_PurshaseBotQueues_SkinId",
                table: "PurshaseBotQueues");

            migrationBuilder.DropColumn(
                name: "SkinId",
                table: "PurshaseBotQueues");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateLastRequest",
                table: "PurshaseBotQueues",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddColumn<string>(
                name: "MarketHashName",
                table: "PurshaseBotQueues",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MarketHashName",
                table: "PurshaseBotQueues");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateLastRequest",
                table: "PurshaseBotQueues",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SkinId",
                table: "PurshaseBotQueues",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurshaseBotQueues_SkinId",
                table: "PurshaseBotQueues",
                column: "SkinId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurshaseBotQueues_Skins_SkinId",
                table: "PurshaseBotQueues",
                column: "SkinId",
                principalTable: "Skins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
