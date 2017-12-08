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
                name: "AuthVerificationModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AuthModelType = table.Column<int>(nullable: false),
                    CodeSendedDateTime = table.Column<DateTime>(nullable: false),
                    NewPassword = table.Column<string>(nullable: true),
                    Reason = table.Column<int>(nullable: false),
                    TargetEmail = table.Column<string>(nullable: true),
                    TargetPhone = table.Column<string>(nullable: true),
                    TwoWayAuthKey = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthVerificationModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BankCards",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Balance = table.Column<decimal>(nullable: false),
                    CVV = table.Column<string>(nullable: true),
                    CardNumber = table.Column<string>(nullable: true),
                    CardType = table.Column<int>(nullable: false),
                    ExpirationDate = table.Column<DateTime>(nullable: false),
                    IsBlocked = table.Column<bool>(nullable: false),
                    OwnerFullName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankCards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientInfos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AuthModelType = table.Column<int>(nullable: false),
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
                        name: "FK_Users_ClientInfos_ClientInfoId",
                        column: x => x.ClientInfoId,
                        principalTable: "ClientInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserCards",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CVV = table.Column<string>(nullable: true),
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
                    table.PrimaryKey("PK_UserCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserCards_ClientInfos_ClientInfoId",
                        column: x => x.ClientInfoId,
                        principalTable: "ClientInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserCards_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Goals",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChargeDate = table.Column<DateTime>(nullable: false),
                    ClientInfoId = table.Column<int>(nullable: true),
                    CreditCardId = table.Column<int>(nullable: true),
                    CurrentMoney = table.Column<decimal>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsPaused = table.Column<bool>(nullable: false),
                    MoneyPerMonth = table.Column<decimal>(nullable: false),
                    MoneyTarget = table.Column<decimal>(nullable: false),
                    TargetEnd = table.Column<DateTime>(nullable: false),
                    TargetStart = table.Column<DateTime>(nullable: false),
                    TargetType = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Goals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Goals_ClientInfos_ClientInfoId",
                        column: x => x.ClientInfoId,
                        principalTable: "ClientInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Goals_UserCards_CreditCardId",
                        column: x => x.CreditCardId,
                        principalTable: "UserCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CardId = table.Column<int>(nullable: false),
                    ClientInfoId = table.Column<int>(nullable: true),
                    CreditCardId = table.Column<int>(nullable: true),
                    GoalId = table.Column<int>(nullable: false),
                    IsPausedOrError = table.Column<bool>(nullable: false),
                    Money = table.Column<decimal>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    TransactionDateTime = table.Column<DateTime>(nullable: false),
                    TransactionIsRetried = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_ClientInfos_ClientInfoId",
                        column: x => x.ClientInfoId,
                        principalTable: "ClientInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_UserCards_CreditCardId",
                        column: x => x.CreditCardId,
                        principalTable: "UserCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_Goals_GoalId",
                        column: x => x.GoalId,
                        principalTable: "Goals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Goals_ClientInfoId",
                table: "Goals",
                column: "ClientInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Goals_CreditCardId",
                table: "Goals",
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
                name: "IX_UserCards_ClientInfoId",
                table: "UserCards",
                column: "ClientInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCards_OwnerId",
                table: "UserCards",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ClientInfoId",
                table: "Users",
                column: "ClientInfoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthVerificationModel");

            migrationBuilder.DropTable(
                name: "BankCards");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Goals");

            migrationBuilder.DropTable(
                name: "UserCards");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ClientInfos");
        }
    }
}
