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
                .Property(f => f.Category)
                .HasConversion<string>();

            // Feedback - Comment ilişki tanımı
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Opinions)
                .WithMany(f => f.Comments)
                .HasForeignKey(c => c.OpinionId)
                .OnDelete(DeleteBehavior.Restrict); // Cascade silmeyi kaldırdık

            // Feedback - Vote ilişki tanımı
            modelBuilder.Entity<Vote>()
                .HasOne(v => v.Opinions)
                .WithMany(f => f.Votes)
                .HasForeignKey(v => v.OpinionId)
                .OnDelete(DeleteBehavior.Restrict); // Aynı şekilde

            // Kullanıcı - Opinion ilişkisi
            modelBuilder.Entity<Opinion>()
                .HasOne(o => o.User)
                .WithMany(u => u.Opinions)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Burada hala cascade bırakabilirsiniz

            // Kullanıcı - Comment ilişkisi
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Burada da cascade bırakabilirsiniz

            // Kullanıcı - Vote ilişkisi
            modelBuilder.Entity<Vote>()
                .HasOne(v => v.User)
                .WithMany(u => u.Votes)
                .HasForeignKey(v => v.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Burada da cascade bırakabilirsiniz
        }

    }
}
