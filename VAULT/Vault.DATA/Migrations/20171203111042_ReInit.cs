using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Vault.DATA.Migrations
{
    public partial class ReInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailAuthModels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CodeSendedDateTime = table.Column<DateTime>(nullable: false),
                    EmailKey = table.Column<string>(nullable: true),
                    NewPassword = table.Column<string>(nullable: true),
                    Reason = table.Column<int>(nullable: false),
                    TargetEmail = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailAuthModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClientInfoId = table.Column<int>(nullable: true),
                    IsRegistrationFinished = table.Column<bool>(nullable: false),
                    Password = table.Column<string>(nullable: true),
                    Role = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_ClientInfo_ClientInfoId",
                        column: x => x.ClientInfoId,
                        principalTable: "ClientInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CVV = table.Column<string>(nullable: true),
                    CardBalance = table.Column<decimal>(nullable: false),
                    CardNumber = table.Column<string>(nullable: true),
                    CardType = table.Column<int>(nullable: false),
                    ClientInfoId = table.Column<int>(nullable: true),
                    CustomCardName = table.Column<string>(nullable: true),
                    ExpirationDate = table.Column<DateTime>(nullable: false),
                    IsPaused = table.Column<bool>(nullable: false),
                    OwnerFullName = table.Column<string>(nullable: true),
                    OwnerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cards_ClientInfo_ClientInfoId",
                        column: x => x.ClientInfoId,
                        principalTable: "ClientInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cards_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Targets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClientInfoId = table.Column<int>(nullable: true),
                    CreditCardId = table.Column<int>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsPaused = table.Column<bool>(nullable: false),
                    MoneyCurrent = table.Column<decimal>(nullable: false),
                    MoneyPerMonth = table.Column<decimal>(nullable: false),
                    MoneyTarget = table.Column<decimal>(nullable: false),
                    TargetEnd = table.Column<DateTime>(nullable: false),
                    TargetStart = table.Column<DateTime>(nullable: false),
                    TargetType = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Targets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Targets_ClientInfo_ClientInfoId",
                        column: x => x.ClientInfoId,
                        principalTable: "ClientInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Targets_Cards_CreditCardId",
                        column: x => x.CreditCardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CardId = table.Column<int>(nullable: true),
                    ClientInfoId = table.Column<int>(nullable: true),
                    CreditCardId = table.Column<int>(nullable: true),
                    GoalId = table.Column<int>(nullable: true),
                    IsPausedError = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_ClientInfo_ClientInfoId",
                        column: x => x.ClientInfoId,
                        principalTable: "ClientInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_Cards_CreditCardId",
                        column: x => x.CreditCardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_Targets_GoalId",
                        column: x => x.GoalId,
                        principalTable: "Targets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cards_ClientInfoId",
                table: "Cards",
                column: "ClientInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_OwnerId",
                table: "Cards",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Targets_ClientInfoId",
                table: "Targets",
                column: "ClientInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Targets_CreditCardId",
                table: "Targets",
                column: "CreditCardId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ClientInfoId",
                table: "Transactions",
                column: "ClientInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CreditCardId",
                table: "Transactions",
                column: "CreditCardId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_GoalId",
                table: "Transactions",
                column: "GoalId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ClientInfoId",
                table: "Users",
                column: "ClientInfoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailAuthModels");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Targets");

            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ClientInfo");
        }
    }
}
