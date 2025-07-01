using BusinessCardApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessCardApi.DataDb
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<BusinessCard> BusinessCards { get; set; }

    }
}
