using Microsoft.EntityFrameworkCore;

namespace Product.Api.Data;
using ProductEntity = Product.Api.Entities.Product;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<ProductEntity> Products => Set<ProductEntity>();
}
