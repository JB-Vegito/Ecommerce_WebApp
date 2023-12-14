﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ecommerce_web.Data;

#nullable disable

namespace ecommerce_web.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230327115836_Orders")]
    partial class Orders
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ecommerce_web.Models.Cart", b =>
                {
                    b.Property<int>("Cart_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Cart_Id"));

                    b.Property<int>("Item_Quantity")
                        .HasColumnType("int");

                    b.Property<int>("Product_Id")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Cart_Id");

                    b.HasIndex("Product_Id");

                    b.HasIndex("Username");

                    b.ToTable("Carts");
                });

            modelBuilder.Entity("ecommerce_web.Models.Category", b =>
                {
                    b.Property<string>("CategoryId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("CategoryId");

                    b.HasAlternateKey("CategoryName")
                        .HasName("AlternateKey_CategoryName");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("ecommerce_web.Models.Inventory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CategoryId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Price")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Inventories");
                });

            modelBuilder.Entity("ecommerce_web.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Item_Quantity")
                        .HasColumnType("int");

                    b.Property<int>("Product_Id")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Product_Id");

                    b.HasIndex("Username");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("ecommerce_web.Models.Users", b =>
                {
                    b.Property<string>("username")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("username");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ecommerce_web.Models.Cart", b =>
                {
                    b.HasOne("ecommerce_web.Models.Inventory", "Inventory")
                        .WithMany()
                        .HasForeignKey("Product_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ecommerce_web.Models.Users", "Users")
                        .WithMany()
                        .HasForeignKey("Username")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Inventory");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("ecommerce_web.Models.Inventory", b =>
                {
                    b.HasOne("ecommerce_web.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("ecommerce_web.Models.Order", b =>
                {
                    b.HasOne("ecommerce_web.Models.Inventory", "Inventory")
                        .WithMany()
                        .HasForeignKey("Product_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ecommerce_web.Models.Users", "Users")
                        .WithMany()
                        .HasForeignKey("Username")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Inventory");

                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
