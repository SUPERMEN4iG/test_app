using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace test_app.api.Migrations
{
    public partial class UpdateStock2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "TradeofferId",
                table: "BotTradeoffers",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<long>(
                name: "TradeId",
                table: "BotTradeoffers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TradeId",
                table: "BotTradeoffers");

            migrationBuilder.AlterColumn<long>(
                name: "TradeofferId",
                table: "BotTradeoffers",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);
        }
    }
}
