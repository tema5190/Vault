using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Vault.DATA.Migrations
{
    public partial class AddPauses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPausedError",
                table: "Transactions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPaused",
                table: "Targets",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CVV",
                table: "Cards",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CardBalance",
                table: "Cards",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsPaused",
                table: "Cards",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPausedError",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "IsPaused",
                table: "Targets");

            migrationBuilder.DropColumn(
                name: "CVV",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "CardBalance",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "IsPaused",
                table: "Cards");
        }
    }
}
