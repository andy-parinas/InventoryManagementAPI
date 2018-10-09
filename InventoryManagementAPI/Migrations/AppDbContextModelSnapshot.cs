﻿// <auto-generated />
using InventoryManagementAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace InventoryManagementAPI.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("InventoryManagementAPI.Models.Inventory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsArchived");

                    b.Property<int?>("LocationId");

                    b.Property<int?>("ProductId");

                    b.Property<double>("Quantity");

                    b.Property<string>("Sku")
                        .IsRequired();

                    b.Property<int?>("StatusId");

                    b.Property<int>("ThresholdCritical");

                    b.Property<int>("ThresholdWarning");

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.HasIndex("ProductId");

                    b.HasIndex("StatusId");

                    b.ToTable("Inventories");
                });

            modelBuilder.Entity("InventoryManagementAPI.Models.InventoryStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Status")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("InventoryStatuses");
                });

            modelBuilder.Entity("InventoryManagementAPI.Models.InventoryTransaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Details");

                    b.Property<int?>("InventoryId");

                    b.Property<bool>("IsArchived");

                    b.Property<double>("Quantity");

                    b.Property<DateTime>("TimeStamp");

                    b.Property<int?>("TransactionTypeId");

                    b.HasKey("Id");

                    b.HasIndex("InventoryId");

                    b.HasIndex("TransactionTypeId");

                    b.ToTable("InventoryTransactions");
                });

            modelBuilder.Entity("InventoryManagementAPI.Models.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address")
                        .IsRequired();

                    b.Property<int?>("LocationTypeId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Phone");

                    b.HasKey("Id");

                    b.HasIndex("LocationTypeId");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("InventoryManagementAPI.Models.LocationType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("LocationTypes");
                });

            modelBuilder.Entity("InventoryManagementAPI.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Cost");

                    b.Property<string>("Descriptions");

                    b.Property<bool>("IsArchived");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<double>("Price");

                    b.Property<int?>("ProductCategoryId");

                    b.Property<string>("Upc")
                        .IsRequired();

                    b.Property<DateTime>("UpdatedAt");

                    b.HasKey("Id");

                    b.HasIndex("ProductCategoryId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("InventoryManagementAPI.Models.ProductCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("ProductCategories");
                });

            modelBuilder.Entity("InventoryManagementAPI.Models.TransactionType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Action")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("TransactionTypes");
                });

            modelBuilder.Entity("InventoryManagementAPI.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired();

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired();

                    b.Property<string>("Phone");

                    b.Property<string>("Username")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("InventoryManagementAPI.Models.Inventory", b =>
                {
                    b.HasOne("InventoryManagementAPI.Models.Location", "Location")
                        .WithMany("Inventories")
                        .HasForeignKey("LocationId");

                    b.HasOne("InventoryManagementAPI.Models.Product", "Product")
                        .WithMany("Inventories")
                        .HasForeignKey("ProductId");

                    b.HasOne("InventoryManagementAPI.Models.InventoryStatus", "Status")
                        .WithMany("Inventories")
                        .HasForeignKey("StatusId");
                });

            modelBuilder.Entity("InventoryManagementAPI.Models.InventoryTransaction", b =>
                {
                    b.HasOne("InventoryManagementAPI.Models.Inventory", "Inventory")
                        .WithMany("Transactions")
                        .HasForeignKey("InventoryId");

                    b.HasOne("InventoryManagementAPI.Models.TransactionType", "TransactionType")
                        .WithMany("Transactions")
                        .HasForeignKey("TransactionTypeId");
                });

            modelBuilder.Entity("InventoryManagementAPI.Models.Location", b =>
                {
                    b.HasOne("InventoryManagementAPI.Models.LocationType", "LocationType")
                        .WithMany("Locations")
                        .HasForeignKey("LocationTypeId");
                });

            modelBuilder.Entity("InventoryManagementAPI.Models.Product", b =>
                {
                    b.HasOne("InventoryManagementAPI.Models.ProductCategory", "ProductCategory")
                        .WithMany("Products")
                        .HasForeignKey("ProductCategoryId");
                });
#pragma warning restore 612, 618
        }
    }
}
