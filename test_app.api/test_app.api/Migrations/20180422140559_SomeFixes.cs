using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace test_app.api.Migrations
{
    public partial class SomeFixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Winners");

            migrationBuilder.AddColumn<decimal>(
                name: "MaxPriceUsd",
                table: "PurshaseBotQueues",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "TransactionId",
                table: "G2APayments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxPriceUsd",
                table: "PurshaseBotQueues");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "G2APayments");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Winners",
                nullable: true);
        }
    }
}
