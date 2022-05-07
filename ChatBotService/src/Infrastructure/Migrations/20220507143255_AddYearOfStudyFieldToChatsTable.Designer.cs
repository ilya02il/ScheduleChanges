﻿// <auto-generated />
using System;
using Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20220507143255_AddYearOfStudyFieldToChatsTable")]
    partial class AddYearOfStudyFieldToChatsTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.13")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Domain.Entities.ChatBotEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("PlatformHash")
                        .HasColumnType("bigint");

                    b.Property<long>("PlatformSpecificChatId")
                        .HasColumnType("bigint");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Chats");
                });

            modelBuilder.Entity("Domain.Entities.ChatBotEntity", b =>
                {
                    b.OwnsOne("Domain.ValueObjects.UserInfo", "UserInfo", b1 =>
                        {
                            b1.Property<Guid>("ChatBotEntityId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Username")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("ChatBotEntityId");

                            b1.ToTable("Chats");

                            b1.WithOwner()
                                .HasForeignKey("ChatBotEntityId");

                            b1.OwnsOne("Domain.ValueObjects.EducationalInfo", "EducationalInfo", b2 =>
                                {
                                    b2.Property<Guid>("UserInfoChatBotEntityId")
                                        .HasColumnType("uniqueidentifier");

                                    b2.Property<string>("EducOrgName")
                                        .HasColumnType("nvarchar(max)");

                                    b2.Property<string>("GroupNumber")
                                        .HasColumnType("nvarchar(max)");

                                    b2.Property<int>("YearOfStudy")
                                        .HasColumnType("int");

                                    b2.HasKey("UserInfoChatBotEntityId");

                                    b2.ToTable("Chats");

                                    b2.WithOwner()
                                        .HasForeignKey("UserInfoChatBotEntityId");
                                });

                            b1.Navigation("EducationalInfo");
                        });

                    b.Navigation("UserInfo");
                });
#pragma warning restore 612, 618
        }
    }
}
