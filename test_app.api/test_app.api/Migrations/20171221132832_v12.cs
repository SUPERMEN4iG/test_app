using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace test_app.api.Migrations
{
    public partial class v12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CasesDrop_Cases_CaseId",
                table: "CasesDrop");

            migrationBuilder.DropForeignKey(
                name: "FK_CasesDrop_Skins_SkinId",
                table: "CasesDrop");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_CasesDrop_Id",
                table: "CasesDrop");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CasesDrop",
                table: "CasesDrop");

            migrationBuilder.RenameTable(
                name: "CasesDrop",
                newName: "CasesDrops");

            migrationBuilder.RenameIndex(
                name: "IX_CasesDrop_SkinId",
                table: "CasesDrops",
                newName: "IX_CasesDrops_SkinId");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_CasesDrops_Id",
                table: "CasesDrops",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CasesDrops",
                table: "CasesDrops",
                columns: new[] { "CaseId", "SkinId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CasesDrops_Cases_CaseId",
                table: "CasesDrops",
                column: "CaseId",
                principalTable: "Cases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CasesDrops_Skins_SkinId",
                table: "CasesDrops",
                column: "SkinId",
                principalTable: "Skins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CasesDrops_Cases_CaseId",
                table: "CasesDrops");

            migrationBuilder.DropForeignKey(
                name: "FK_CasesDrops_Skins_SkinId",
                table: "CasesDrops");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_CasesDrops_Id",
                table: "CasesDrops");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CasesDrops",
                table: "CasesDrops");

            migrationBuilder.RenameTable(
                name: "CasesDrops",
                newName: "CasesDrop");

            migrationBuilder.RenameIndex(
                name: "IX_CasesDrops_SkinId",
                table: "CasesDrop",
                newName: "IX_CasesDrop_SkinId");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_CasesDrop_Id",
                table: "CasesDrop",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CasesDrop",
                table: "CasesDrop",
                columns: new[] { "CaseId", "SkinId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CasesDrop_Cases_CaseId",
                table: "CasesDrop",
                column: "CaseId",
                principalTable: "Cases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CasesDrop_Skins_SkinId",
                table: "CasesDrop",
                column: "SkinId",
                principalTable: "Skins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
