using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Vault.DATA.Migrations
{
    public partial class RemoveUserFromAuthModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailAuthModels_Users_UserId",
                table: "EmailAuthModels");

            migrationBuilder.DropIndex(
                name: "IX_EmailAuthModels_UserId",
                table: "EmailAuthModels");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "EmailAuthModels");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "EmailAuthModels",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_EmailAuthModels_UserId",
                table: "EmailAuthModels",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailAuthModels_Users_UserId",
                table: "EmailAuthModels",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
