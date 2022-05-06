using CryptoP2P.Backend.Data.Configuration;
using CryptoP2P.Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace CryptoP2P.Backend.Data;

public class AppDbContext : DbContext
{
  public DbSet<User> Users { get; set; }
  public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
  {
  }

  protected override void OnConfiguring(DbContextOptionsBuilder options)
    => options.UseSqlite($"Data Source = Data.db");

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfiguration(new UserConfiguration());
  }
}