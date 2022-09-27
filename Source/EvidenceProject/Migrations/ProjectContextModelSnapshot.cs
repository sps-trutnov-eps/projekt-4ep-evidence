﻿// <auto-generated />
using System;
using EvidenceProject.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EvidenceProject.Migrations
{
    [DbContext(typeof(ProjectContext))]
    partial class ProjectContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("EvidenceProject.Data.DataModels.DialCode", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"), 1L, 1);

                    b.Property<int?>("ProjectAchivements")
                        .HasColumnType("int");

                    b.Property<string>("description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("dialInfoid")
                        .HasColumnType("int");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.HasIndex("ProjectAchivements");

                    b.HasIndex("dialInfoid");

                    b.ToTable("dialCodes");
                });

            modelBuilder.Entity("EvidenceProject.Data.DataModels.DialInfo", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"), 1L, 1);

                    b.Property<string>("desc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("dialInfos");
                });

            modelBuilder.Entity("EvidenceProject.Data.DataModels.Project", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"), 1L, 1);

                    b.Property<int>("ProjectState")
                        .HasColumnType("int");

                    b.Property<int>("ProjectTechnology")
                        .HasColumnType("int");

                    b.Property<int>("ProjectType")
                        .HasColumnType("int");

                    b.Property<string>("github")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("slack")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.HasIndex("ProjectState");

                    b.HasIndex("ProjectTechnology");

                    b.HasIndex("ProjectType");

                    b.ToTable("projects");
                });

            modelBuilder.Entity("EvidenceProject.Data.DataModels.User", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"), 1L, 1);

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Projectid")
                        .HasColumnType("int");

                    b.Property<string>("contactDetails")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("fullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("studyField")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.HasIndex("Projectid");

                    b.ToTable("User");

                    b.HasDiscriminator<string>("Discriminator").HasValue("User");
                });

            modelBuilder.Entity("EvidenceProject.Data.DataModels.AuthUser", b =>
                {
                    b.HasBaseType("EvidenceProject.Data.DataModels.User");

                    b.Property<bool?>("globalAdmin")
                        .HasColumnType("bit");

                    b.Property<string>("password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("username")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasIndex("username")
                        .IsUnique()
                        .HasFilter("[username] IS NOT NULL");

                    b.HasDiscriminator().HasValue("AuthUser");
                });

            modelBuilder.Entity("EvidenceProject.Data.DataModels.DialCode", b =>
                {
                    b.HasOne("EvidenceProject.Data.DataModels.Project", null)
                        .WithMany("projectAchievements")
                        .HasForeignKey("ProjectAchivements");

                    b.HasOne("EvidenceProject.Data.DataModels.DialInfo", "dialInfo")
                        .WithMany("dialCodes")
                        .HasForeignKey("dialInfoid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("dialInfo");
                });

            modelBuilder.Entity("EvidenceProject.Data.DataModels.Project", b =>
                {
                    b.HasOne("EvidenceProject.Data.DataModels.DialCode", "projectState")
                        .WithMany()
                        .HasForeignKey("ProjectState")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EvidenceProject.Data.DataModels.DialCode", "projectTechnology")
                        .WithMany()
                        .HasForeignKey("ProjectTechnology")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EvidenceProject.Data.DataModels.DialCode", "projectType")
                        .WithMany()
                        .HasForeignKey("ProjectType")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("projectState");

                    b.Navigation("projectTechnology");

                    b.Navigation("projectType");
                });

            modelBuilder.Entity("EvidenceProject.Data.DataModels.User", b =>
                {
                    b.HasOne("EvidenceProject.Data.DataModels.Project", null)
                        .WithMany("assignees")
                        .HasForeignKey("Projectid");
                });

            modelBuilder.Entity("EvidenceProject.Data.DataModels.DialInfo", b =>
                {
                    b.Navigation("dialCodes");
                });

            modelBuilder.Entity("EvidenceProject.Data.DataModels.Project", b =>
                {
                    b.Navigation("assignees");

                    b.Navigation("projectAchievements");
                });
#pragma warning restore 612, 618
        }
    }
}
