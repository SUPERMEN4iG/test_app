using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace test_app.shared.Migrations
{
    public partial class chance_fix_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CaseFaultLogs_Cases_CaseId",
                table: "CaseFaultLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Winners_Cases_CaseId",
                table: "Winners");

            migrationBuilder.DropForeignKey(
                name: "FK_Winners_Skins_SkinId",
                table: "Winners");

            migrationBuilder.AlterColumn<long>(
                name: "SkinId",
                table: "Winners",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CaseId",
                table: "Winners",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Chance",
                table: "CasesDrops",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(9, 8)");

            migrationBuilder.AlterColumn<long>(
                name: "CaseId",
                table: "CaseFaultLogs",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseFaultLogs_Cases_CaseId",
                table: "CaseFaultLogs",
                column: "CaseId",
                principalTable: "Cases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Winners_Cases_CaseId",
                table: "Winners",
                column: "CaseId",
                principalTable: "Cases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Winners_Skins_SkinId",
                table: "Winners",
                column: "SkinId",
                principalTable: "Skins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CaseFaultLogs_Cases_CaseId",
                table: "CaseFaultLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Winners_Cases_CaseId",
                table: "Winners");

            migrationBuilder.DropForeignKey(
                name: "FK_Winners_Skins_SkinId",
                table: "Winners");

            migrationBuilder.AlterColumn<long>(
                name: "SkinId",
                table: "Winners",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "CaseId",
                table: "Winners",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<decimal>(
                name: "Chance",
                table: "CasesDrops",
                type: "decimal(9, 8)",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<long>(
                name: "CaseId",
                table: "CaseFaultLogs",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddForeignKey(
                name: "FK_CaseFaultLogs_Cases_CaseId",
                table: "CaseFaultLogs",
                column: "CaseId",
                principalTable: "Cases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Winners_Cases_CaseId",
                table: "Winners",
                column: "CaseId",
                principalTable: "Cases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Winners_Skins_SkinId",
                table: "Winners",
                column: "SkinId",
                principalTable: "Skins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
