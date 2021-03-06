﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using POSApplicationAPI.Data;
using System;

namespace POSApplicationAPI.Migrations
{
    [DbContext(typeof(PosAppDbContext))]
    [Migration("20181116011643_AddProductToOrderitem")]
    partial class AddProductToOrderitem
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("POSApplicationAPI.Models.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("BirthDay");

                    b.Property<string>("ContactNumber");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("LoyaltyCodes");

                    b.Property<double>("LoyaltyPoints");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("POSApplicationAPI.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CustomerId");

                    b.Property<string>("InvoiceNumber");

                    b.Property<DateTime>("OrderDate");

                    b.Property<double>("Total");

                    b.Property<double>("TotalExGst");

                    b.Property<double>("TotalGst");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("POSApplicationAPI.Models.OrderItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<double>("Discounts");

                    b.Property<double>("Gst");

                    b.Property<int?>("OrderId");

                    b.Property<int>("ProductId");

                    b.Property<double>("Quantity");

                    b.Property<double>("TotalExGst");

                    b.Property<double>("TotalIncGst");

                    b.Property<double>("UnitPrice");

                    b.Property<string>("Upc");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("POSApplicationAPI.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Descriptions");

                    b.Property<bool>("IsGstApplicable");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<double>("Price");

                    b.Property<string>("Upc")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("POSApplicationAPI.Models.Order", b =>
                {
                    b.HasOne("POSApplicationAPI.Models.Customer", "Customer")
                        .WithMany("Orders")
                        .HasForeignKey("CustomerId");
                });

            modelBuilder.Entity("POSApplicationAPI.Models.OrderItem", b =>
                {
                    b.HasOne("POSApplicationAPI.Models.Order", "Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId");

                    b.HasOne("POSApplicationAPI.Models.Product", "Product")
                        .WithMany("OrderItems")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
