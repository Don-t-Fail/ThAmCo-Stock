using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThAmCo.Stock.Data
{
    public class StockDbContext : DbContext
    {
        public DbSet<Price> Prices { get; set; }
        public DbSet<ProductStock> ProductStocks { get; set; }

        private IHostingEnvironment HostEnv { get; }

        public StockDbContext(DbContextOptions<StockDbContext> options, IHostingEnvironment env) : base(options)
        {
            HostEnv = env;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            base.OnConfiguring(builder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Price>()
                .HasKey(p => p.Id);

            builder.Entity<ProductStock>()
                .HasKey(ps => ps.Id);

            if (HostEnv != null && HostEnv.IsDevelopment())
            {
                builder.Entity<ProductStock>()
                    .HasData(
                        new ProductStock { Id = 1, ProductId = 1, PriceId = 2, Stock = 4 },
                        new ProductStock { Id = 2, ProductId = 2, PriceId = 3, Stock = 42 },
                        new ProductStock { Id = 3, ProductId = 3, PriceId = 4, Stock = 32 },
                        new ProductStock { Id = 4, ProductId = 4, PriceId = 5, Stock = 13},
                        new ProductStock { Id = 5, ProductId = 5, PriceId = 6, Stock = 0},
                        new ProductStock { Id = 6, ProductId = 6, PriceId = 7, Stock = 1},
                        new ProductStock { Id = 7, ProductId = 7, PriceId = 8, Stock = 49}
                    );

                builder.Entity<Price>()
                    .HasData(
                        new Price { Id = 1, ProductStockId = 1, ProductPrice = 9.99 },
                        new Price { Id = 2, ProductStockId = 1, ProductPrice = 8.99 },
                        new Price { Id = 3, ProductStockId = 2, ProductPrice = 17.99 },
                        new Price { Id = 4, ProductStockId = 3, ProductPrice = 23.00},
                        new Price { Id = 5, ProductStockId = 4, ProductPrice = 2.99},
                        new Price { Id = 6, ProductStockId = 5, ProductPrice = 49.63},
                        new Price { Id = 7, ProductStockId = 6, ProductPrice = 4.56},
                        new Price { Id = 8, ProductStockId = 7, ProductPrice = 10.01}
                    );
            }
        }
    }
}
