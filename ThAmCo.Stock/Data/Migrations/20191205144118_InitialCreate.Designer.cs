﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ThAmCo.Stock.Data;

namespace ThAmCo.Stock.Data.Migrations
{
    [DbContext(typeof(StockDbContext))]
    [Migration("20191205144118_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.0-rtm-35687")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ThAmCo.Stock.Data.Price", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date");

                    b.Property<double>("ProductPrice");

                    b.Property<int>("ProductStockId");

                    b.HasKey("Id");

                    b.ToTable("Prices");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Date = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ProductPrice = 9.9900000000000002,
                            ProductStockId = 1
                        },
                        new
                        {
                            Id = 2,
                            Date = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ProductPrice = 8.9900000000000002,
                            ProductStockId = 1
                        },
                        new
                        {
                            Id = 3,
                            Date = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ProductPrice = 17.989999999999998,
                            ProductStockId = 2
                        });
                });

            modelBuilder.Entity("ThAmCo.Stock.Data.ProductStock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("PriceId");

                    b.Property<int>("ProductId");

                    b.Property<int>("Stock");

                    b.HasKey("Id");

                    b.ToTable("ProductStocks");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            PriceId = 2,
                            ProductId = 1,
                            Stock = 4
                        },
                        new
                        {
                            Id = 2,
                            PriceId = 3,
                            ProductId = 2,
                            Stock = 42
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
