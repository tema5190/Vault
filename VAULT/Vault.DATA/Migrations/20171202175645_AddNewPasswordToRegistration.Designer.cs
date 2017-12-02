﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using Vault.DATA;
using Vault.DATA.Enums;

namespace Vault.DATA.Migrations
{
    [DbContext(typeof(VaultContext))]
    [Migration("20171202175645_AddNewPasswordToRegistration")]
    partial class AddNewPasswordToRegistration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Vault.DATA.CreditCard", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CardNumber");

                    b.Property<int>("CardType");

                    b.Property<int?>("ClientInfoId");

                    b.Property<string>("Name");

                    b.Property<int?>("OwnerId");

                    b.Property<DateTime>("RefillDate");

                    b.HasKey("Id");

                    b.HasIndex("ClientInfoId");

                    b.HasIndex("OwnerId");

                    b.ToTable("Cards");
                });

            modelBuilder.Entity("Vault.DATA.Models.ClientInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.HasKey("Id");

                    b.ToTable("ClientInfo");
                });

            modelBuilder.Entity("Vault.DATA.Models.Goal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ClientInfoId");

                    b.Property<string>("Description");

                    b.Property<decimal>("MoneyCurrent");

                    b.Property<decimal>("MoneyTarget");

                    b.Property<DateTime>("TargetEnd");

                    b.Property<DateTime>("TargetStart");

                    b.Property<int>("TargetType");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("ClientInfoId");

                    b.ToTable("Targets");
                });

            modelBuilder.Entity("Vault.DATA.Models.RefillTransaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CardId");

                    b.Property<int?>("ClientInfoId");

                    b.Property<int?>("CreditCardId");

                    b.Property<string>("Description");

                    b.Property<int?>("TargetId");

                    b.HasKey("Id");

                    b.HasIndex("ClientInfoId");

                    b.HasIndex("CreditCardId");

                    b.HasIndex("TargetId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("Vault.DATA.Models.Users.Registration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CodeSendedDateTime");

                    b.Property<string>("EmailKey");

                    b.Property<string>("NewPassword");

                    b.Property<string>("TargetEmail");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.ToTable("Registrations");
                });

            modelBuilder.Entity("Vault.DATA.Models.VaultUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ClientInfoId");

                    b.Property<bool>("IsRegistrationFinished");

                    b.Property<string>("Password");

                    b.Property<int>("Role");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.HasIndex("ClientInfoId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Vault.DATA.CreditCard", b =>
                {
                    b.HasOne("Vault.DATA.Models.ClientInfo")
                        .WithMany("Cards")
                        .HasForeignKey("ClientInfoId");

                    b.HasOne("Vault.DATA.Models.VaultUser", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId");
                });

            modelBuilder.Entity("Vault.DATA.Models.Goal", b =>
                {
                    b.HasOne("Vault.DATA.Models.ClientInfo")
                        .WithMany("Goals")
                        .HasForeignKey("ClientInfoId");
                });

            modelBuilder.Entity("Vault.DATA.Models.RefillTransaction", b =>
                {
                    b.HasOne("Vault.DATA.Models.ClientInfo")
                        .WithMany("Transactions")
                        .HasForeignKey("ClientInfoId");

                    b.HasOne("Vault.DATA.CreditCard", "CreditCard")
                        .WithMany("Transactions")
                        .HasForeignKey("CreditCardId");

                    b.HasOne("Vault.DATA.Models.Goal", "Target")
                        .WithMany("Transactions")
                        .HasForeignKey("TargetId");
                });

            modelBuilder.Entity("Vault.DATA.Models.VaultUser", b =>
                {
                    b.HasOne("Vault.DATA.Models.ClientInfo", "ClientInfo")
                        .WithMany()
                        .HasForeignKey("ClientInfoId");
                });
#pragma warning restore 612, 618
        }
    }
}
