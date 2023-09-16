﻿// <auto-generated />
using Infrastructure.WriteData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Infrastructure.Migrations
{
    [DbContext(typeof(EfWriteDbContext))]
    [Migration("20220507132631_YearOfStudyFieldToGroupsTable")]
    partial class YearOfStudyFieldToGroupsTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.13")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Domain.Common.ListItemEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("ListItems");
                });

            modelBuilder.Entity("Domain.Entities.ChangesListEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("EducationalOrgId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsOddWeek")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("EducationalOrgId");

                    b.ToTable("ChangesLists");
                });

            modelBuilder.Entity("Domain.Entities.EducationalOrgEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("EducationalOrgs");
                });

            modelBuilder.Entity("Domain.Entities.GroupEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("EducationalOrgId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("GroupNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("YearOfStudy")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EducationalOrgId");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("Domain.Entities.LessonCallEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("DayOfWeek")
                        .HasColumnType("int");

                    b.Property<Guid>("EducationalOrgId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<TimeSpan>("EndTime")
                        .HasColumnType("time");

                    b.Property<int>("Position")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("StartTime")
                        .HasColumnType("time");

                    b.HasKey("Id");

                    b.HasIndex("EducationalOrgId");

                    b.ToTable("LessonCalls");
                });

            modelBuilder.Entity("Domain.Entities.ScheduleListEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("DayOfWeek")
                        .HasColumnType("int");

                    b.Property<Guid>("GroupId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("ScheduleLists");
                });

            modelBuilder.Entity("Domain.Entities.ChangesListItemEntity", b =>
                {
                    b.HasBaseType("Domain.Common.ListItemEntity");

                    b.Property<Guid>("ChangesListId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("GroupId")
                        .HasColumnType("uniqueidentifier");

                    b.HasIndex("ChangesListId");

                    b.HasIndex("GroupId");

                    b.ToTable("ChangesListItems");
                });

            modelBuilder.Entity("Domain.Entities.ScheduleListItemEntity", b =>
                {
                    b.HasBaseType("Domain.Common.ListItemEntity");

                    b.Property<bool?>("IsOddWeek")
                        .HasColumnType("bit");

                    b.Property<Guid>("ScheduleListId")
                        .HasColumnType("uniqueidentifier");

                    b.HasIndex("ScheduleListId");

                    b.ToTable("ScheduleListItems");
                });

            modelBuilder.Entity("Domain.Entities.DatedLessonCallEntity", b =>
                {
                    b.HasBaseType("Domain.Entities.LessonCallEntity");

                    b.Property<DateTimeOffset>("Date")
                        .HasColumnType("datetimeoffset");

                    b.ToTable("DatedLessonCalls");
                });

            modelBuilder.Entity("Domain.Common.ListItemEntity", b =>
                {
                    b.OwnsOne("Domain.ValueObjects.ItemInfo", "ItemInfo", b1 =>
                        {
                            b1.Property<Guid>("ListItemEntityId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Auditorium")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<int>("Position")
                                .HasColumnType("int");

                            b1.Property<string>("SubjectName")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("TeacherInitials")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("ListItemEntityId");

                            b1.ToTable("ListItems");

                            b1.WithOwner()
                                .HasForeignKey("ListItemEntityId");
                        });

                    b.Navigation("ItemInfo");
                });

            modelBuilder.Entity("Domain.Entities.ChangesListEntity", b =>
                {
                    b.HasOne("Domain.Entities.EducationalOrgEntity", "EducationalOrg")
                        .WithMany("ChangesLists")
                        .HasForeignKey("EducationalOrgId");

                    b.Navigation("EducationalOrg");
                });

            modelBuilder.Entity("Domain.Entities.GroupEntity", b =>
                {
                    b.HasOne("Domain.Entities.EducationalOrgEntity", "EducationalOrg")
                        .WithMany("Groups")
                        .HasForeignKey("EducationalOrgId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EducationalOrg");
                });

            modelBuilder.Entity("Domain.Entities.LessonCallEntity", b =>
                {
                    b.HasOne("Domain.Entities.EducationalOrgEntity", "EducationalOrg")
                        .WithMany("LessonCalls")
                        .HasForeignKey("EducationalOrgId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EducationalOrg");
                });

            modelBuilder.Entity("Domain.Entities.ScheduleListEntity", b =>
                {
                    b.HasOne("Domain.Entities.GroupEntity", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");
                });

            modelBuilder.Entity("Domain.Entities.ChangesListItemEntity", b =>
                {
                    b.HasOne("Domain.Entities.ChangesListEntity", "ChangesList")
                        .WithMany("ListItems")
                        .HasForeignKey("ChangesListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.GroupEntity", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Common.ListItemEntity", null)
                        .WithOne()
                        .HasForeignKey("Domain.Entities.ChangesListItemEntity", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("ChangesList");

                    b.Navigation("Group");
                });

            modelBuilder.Entity("Domain.Entities.ScheduleListItemEntity", b =>
                {
                    b.HasOne("Domain.Common.ListItemEntity", null)
                        .WithOne()
                        .HasForeignKey("Domain.Entities.ScheduleListItemEntity", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.ScheduleListEntity", "ScheduleList")
                        .WithMany("ListItems")
                        .HasForeignKey("ScheduleListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ScheduleList");
                });

            modelBuilder.Entity("Domain.Entities.DatedLessonCallEntity", b =>
                {
                    b.HasOne("Domain.Entities.LessonCallEntity", null)
                        .WithOne()
                        .HasForeignKey("Domain.Entities.DatedLessonCallEntity", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Entities.ChangesListEntity", b =>
                {
                    b.Navigation("ListItems");
                });

            modelBuilder.Entity("Domain.Entities.EducationalOrgEntity", b =>
                {
                    b.Navigation("ChangesLists");

                    b.Navigation("Groups");

                    b.Navigation("LessonCalls");
                });

            modelBuilder.Entity("Domain.Entities.ScheduleListEntity", b =>
                {
                    b.Navigation("ListItems");
                });
#pragma warning restore 612, 618
        }
    }
}
