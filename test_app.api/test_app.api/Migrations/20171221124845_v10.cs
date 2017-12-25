using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace test_app.api.Migrations
{
    public partial class v10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CaseSellLogs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreate = table.Column<DateTime>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    SkinId = table.Column<long>(nullable: true),
                    Source = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    WinnerId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseSellLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaseSellLogs_Skins_SkinId",
                        column: x => x.SkinId,
                        principalTable: "Skins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CaseSellLogs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CaseSellLogs_Winners_WinnerId",
                        column: x => x.WinnerId,
                        principalTable: "Winners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StackCaseSkins",
                columns: table => new
                {
                    StackCaseId = table.Column<long>(nullable: false),
                    SkinId = table.Column<long>(nullable: false),
                    DateCreate = table.Column<DateTime>(nullable: false),
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StackCaseSkins", x => new { x.StackCaseId, x.SkinId });
                    table.UniqueConstraint("AK_StackCaseSkins_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StackCaseSkins_Skins_SkinId",
                        column: x => x.SkinId,
                        principalTable: "Skins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StackCaseSkins_StackCases_StackCaseId",
                        column: x => x.StackCaseId,
                        principalTable: "StackCases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CaseSellLogs_SkinId",
                table: "CaseSellLogs",
                column: "SkinId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseSellLogs_UserId",
                table: "CaseSellLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseSellLogs_WinnerId",
                table: "CaseSellLogs",
                column: "WinnerId");

            migrationBuilder.CreateIndex(
                name: "IX_StackCaseSkins_SkinId",
                table: "StackCaseSkins",
                column: "SkinId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CaseSellLogs");

            migrationBuilder.DropTable(
                name: "StackCaseSkins");
        }
    }
}
