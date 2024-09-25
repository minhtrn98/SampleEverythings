﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UploadLargeFile.Api;

#nullable disable

namespace UploadLargeFile.Api.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("UploadLargeFile.Api.Book", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Authors")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CountsOfReview")
                        .HasColumnType("int");

                    b.Property<string>("ISBN")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PagesNumber")
                        .HasColumnType("int");

                    b.Property<int>("PublishDay")
                        .HasColumnType("int");

                    b.Property<int>("PublishMonth")
                        .HasColumnType("int");

                    b.Property<int>("PublishYear")
                        .HasColumnType("int");

                    b.Property<string>("Publisher")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Rating")
                        .HasColumnType("float");

                    b.Property<Guid>("RatingDistributionId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RatingDistributionId");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("UploadLargeFile.Api.RatingDistribution", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("RatingDist1")
                        .HasColumnType("int");

                    b.Property<int>("RatingDist2")
                        .HasColumnType("int");

                    b.Property<int>("RatingDist3")
                        .HasColumnType("int");

                    b.Property<int>("RatingDist4")
                        .HasColumnType("int");

                    b.Property<int>("RatingDist5")
                        .HasColumnType("int");

                    b.Property<int>("RatingDistTotal")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("RatingDistributions");
                });

            modelBuilder.Entity("UploadLargeFile.Api.Book", b =>
                {
                    b.HasOne("UploadLargeFile.Api.RatingDistribution", "RatingDistribution")
                        .WithMany()
                        .HasForeignKey("RatingDistributionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RatingDistribution");
                });
#pragma warning restore 612, 618
        }
    }
}
