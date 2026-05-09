using Microsoft.EntityFrameworkCore;
using GameShared;

namespace GameServer
{
    public class GameDbContext : DbContext
    {
        public DbSet<Game> Games { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer(
    @"Server=(localdb)\MSSQLLocalDB;Database=GameCatalog;Integrated Security=true;" +
    @"AttachDbFilename=C:\Users\User\Desktop\GameCatalog\GameServer\games.mdf;");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>(entity =>
            {
                entity.HasKey(g => g.Id);
                entity.Property(g => g.Name).IsRequired().HasMaxLength(200);
                entity.Property(g => g.Description).HasMaxLength(1000);
                entity.Property(g => g.Price).HasColumnType("decimal(10,2)");
            });
        }
    }
}