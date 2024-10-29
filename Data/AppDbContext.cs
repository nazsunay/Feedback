using Feedback.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Feedback.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

      
        public DbSet<Opinion> Opinions { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Vote> Votes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Enumları string olarak saklama
            modelBuilder.Entity<Opinion>()
                .Property(f => f.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Opinion>()
                .Property(f => f.Category) // Kategori enum'u olarak kullanılacak
                .HasConversion<string>();

            


            // Feedback - Comment ilişki tanımı
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Opinions)
                .WithMany(f => f.Comments)
                .HasForeignKey(c => c.OpinionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Feedback - Vote ilişki tanımı
            modelBuilder.Entity<Vote>()
                .HasOne(v => v.Opinions)
                .WithMany(f => f.Votes)
                .HasForeignKey(v => v.OpinionId)
                .OnDelete(DeleteBehavior.Cascade);

            
        }
    }
}
