using Microsoft.EntityFrameworkCore;
using Comnet.Data.DBModels;
using Comnet.Data.SPModels.Car;

namespace Comnet.Data.Context
{
    public class ComnetDbContext : DbContext
    {
        public ComnetDbContext(DbContextOptions<ComnetDbContext> options) : base(options)
        {
        }
        public DbSet<Car> Car { get; set; }
        public DbSet<Images> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //DB Models
            modelBuilder.Entity<Car>().ToTable("CM_Cars");
            modelBuilder.Entity<Images>().ToTable("CM_CarImages");

            //SP Models
            modelBuilder.Entity<CarList>().HasNoKey().ToView(null);
        }
    }
}
