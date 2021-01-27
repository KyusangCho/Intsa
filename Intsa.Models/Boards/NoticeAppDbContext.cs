using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace Intsa.Models.Boards
{
    /// <summary>
    /// [5] DbContext Class
    /// </summary>
    public class NoticeAppDbContext : DbContext
    {
        // SqlServer, InMemory, ConfigurationManager
        public NoticeAppDbContext()
        {
            // Empty
        }

        public NoticeAppDbContext(DbContextOptions<NoticeAppDbContext> options) : base(options)
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
            modelBuilder.Entity<BoardNotices>().Property(m => m.Created).HasDefaultValueSql("GetDate()"); 
        }

        public DbSet<BoardNotices> BoardNotices { get; set; }
    }
}
