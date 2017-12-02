using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Vault.DATA.Migrations
{
    public partial class RefactorCard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Targets_TargetId",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "TargetId",
                table: "Transactions",
                newName: "GoalId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_TargetId",
                table: "Transactions",
                newName: "IX_Transactions_GoalId");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Cards",
                newName: "OwnerFullName");

            migrationBuilder.AddColumn<int>(
                name: "CreditCardId",
                table: "Targets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomCardName",
                table: "Cards",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Targets_CreditCardId",
                table: "Targets",
                column: "CreditCardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Targets_Cards_CreditCardId",
                table: "Targets",
                column: "CreditCardId",
                principalTable: "Cards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Targets_GoalId",
                table: "Transactions",
                column: "GoalId",
                principalTable: "Targets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Targets_Cards_CreditCardId",
                table: "Targets");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Targets_GoalId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Targets_CreditCardId",
                table: "Targets");

            migrationBuilder.DropColumn(
                name: "CreditCardId",
                table: "Targets");

            migrationBuilder.DropColumn(
                name: "CustomCardName",
                table: "Cards");

            migrationBuilder.RenameColumn(
                name: "GoalId",
                table: "Transactions",
                newName: "TargetId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_GoalId",
                table: "Transactions",
                newName: "IX_Transactions_TargetId");

            migrationBuilder.RenameColumn(
                name: "OwnerFullName",
                table: "Cards",
                newName: "Name");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Targets_TargetId",
                table: "Transactions",
                column: "TargetId",
                principalTable: "Targets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
