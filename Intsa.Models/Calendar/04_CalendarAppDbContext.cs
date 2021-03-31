using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace Intsa.Models.Calendar
{
    /// <summary>
    /// [5] DbContext Class
    /// </summary>
    public class CalendarAppDbContext : DbContext
    {
        // SqlServer, InMemory, ConfigurationManager
        public CalendarAppDbContext()
        {
            // Empty
        }

        public CalendarAppDbContext(DbContextOptions<CalendarAppDbContext> options) : base(options)
        {
            // 공식같은 코드 

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = ConfigurationManager.ConnectionStrings[
                    "ConnectionString"].ConnectionString;
                optionsBuilder.UseSqlServer(connectionString); 
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CalendarCenter>().Property(m => m.StartTime).HasDefaultValueSql("GetDate()"); 
            modelBuilder.Entity<CalendarCenter>().Property(m => m.EndTime).HasDefaultValueSql("GetDate()"); 
        }

        public DbSet<CalendarCenter> CalendarCenter { get; set; }
    }
}
