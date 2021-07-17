using Microsoft.EntityFrameworkCore;
using ParkyAPI.Models;

namespace ParkyAPI.Data
{
    public class ParkyDbContext : DbContext
    {
        public ParkyDbContext(DbContextOptions<ParkyDbContext> options) : base(options)
        {
        }

        public DbSet<NationalPark> NationalParks { get; set; }
    }
}