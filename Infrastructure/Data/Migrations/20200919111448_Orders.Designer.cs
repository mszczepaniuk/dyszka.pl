﻿// <auto-generated />
using System;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20200919111448_Orders")]
    partial class Orders
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ApplicationCore.Models.ApplicationUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfileImage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TelephoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ApplicationUsers");
                });

            modelBuilder.Entity("ApplicationCore.Models.AuditLog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AffectedEntityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CreatedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.ToTable("AuditLogs");
                });

            modelBuilder.Entity("ApplicationCore.Models.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CreatedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsPositive")
                        .HasColumnType("bit");

                    b.Property<Guid?>("OfferId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("UpdatedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("OfferId");

                    b.HasIndex("UpdatedById");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("ApplicationCore.Models.Message", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CreatedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("ReceiverId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("ReceiverId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("ApplicationCore.Models.Offer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CreatedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsBlocked")
                        .HasColumnType("bit");

                    b.Property<bool>("IsHidden")
                        .HasColumnType("bit");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,4)");

                    b.Property<string>("ShortDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Tags")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("UpdatedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("UpdatedById");

                    b.ToTable("Offers");
                });

            modelBuilder.Entity("ApplicationCore.Models.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CreatedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Done")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("DoneTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("OfferId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("OfferId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("ApplicationCore.Models.AuditLog", b =>
                {
                    b.HasOne("ApplicationCore.Models.ApplicationUser", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");
                });

            modelBuilder.Entity("ApplicationCore.Models.Comment", b =>
                {
                    b.HasOne("ApplicationCore.Models.ApplicationUser", "CreatedBy")
                        .WithMany("Comments")
                        .HasForeignKey("CreatedById");

                    b.HasOne("ApplicationCore.Models.Offer", "Offer")
                        .WithMany("Comments")
                        .HasForeignKey("OfferId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ApplicationCore.Models.ApplicationUser", "UpdatedBy")
                        .WithMany()
                        .HasForeignKey("UpdatedById");
                });

            modelBuilder.Entity("ApplicationCore.Models.Message", b =>
                {
                    b.HasOne("ApplicationCore.Models.ApplicationUser", "CreatedBy")
                        .WithMany("SendMessages")
                        .HasForeignKey("CreatedById");

                    b.HasOne("ApplicationCore.Models.ApplicationUser", "Receiver")
                        .WithMany("ReceivedMessages")
                        .HasForeignKey("ReceiverId");
                });

            modelBuilder.Entity("ApplicationCore.Models.Offer", b =>
                {
                    b.HasOne("ApplicationCore.Models.ApplicationUser", "CreatedBy")
                        .WithMany("Offers")
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("ApplicationCore.Models.ApplicationUser", "UpdatedBy")
                        .WithMany()
                        .HasForeignKey("UpdatedById");
                });

            modelBuilder.Entity("ApplicationCore.Models.Order", b =>
                {
                    b.HasOne("ApplicationCore.Models.ApplicationUser", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.HasOne("ApplicationCore.Models.Offer", "Offer")
                        .WithMany()
                        .HasForeignKey("OfferId");
                });
#pragma warning restore 612, 618
        }
    }
}
