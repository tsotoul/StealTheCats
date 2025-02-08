using Microsoft.EntityFrameworkCore;
using StealTheCats.Models;

namespace StealTheCats
{
    public class CatsDbContext : DbContext
    {
        public DbSet<Cat> Cats { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<CatTag> CatTags { get; set; }

        public CatsDbContext(DbContextOptions<CatsDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cat>()
                .HasMany(c => c.CatTags)
                .WithOne(ct => ct.Cat)
                .HasForeignKey(ct => ct.CatId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Tag>()
                .HasMany(t => t.CatTags)
                .WithOne(ct => ct.Tag)
                .HasForeignKey(ct => ct.TagId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
