using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RestfulApp.Data.Models;

namespace RestfulApp.Data;
public class DataContext : IdentityDbContext
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options) { }

    public DbSet<ItemDto> Items { get; set; }
    public DbSet<RefreshTokenDto> RefreshTokens { get; set; }
}