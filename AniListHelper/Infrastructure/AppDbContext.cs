using AniListHelper.Models;
using Microsoft.EntityFrameworkCore;

namespace AniListHelper.Infrastructure {
    public class AppDbContext : DbContext {
        public DbSet<MediaEntryModel> MediaEntries { get; set; }
        public AppDbContext() {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
            //Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlite($"Filename={Constants.Database.DatabasePath}");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
