using Feedback.Entity;
using Microsoft.EntityFrameworkCore;

namespace Feedback.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Opinion> Opinions { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Vote> Votes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Enumları string olarak saklama
            modelBuilder.Entity<Ticket>()
                .Property(t => t.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Opinion>()
                .Property(f => f.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Opinion>()
                .Property(f => f.Category) // Kategori enum'u olarak kullanılacak
                .HasConversion<string>();

            // User - Feedback ilişki tanımı
            modelBuilder.Entity<Opinion>()
                .HasOne(f => f.User)
                .WithMany(u => u.Opinions)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // User - Comment ilişki tanımı
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // User - Vote ilişki tanımı
            modelBuilder.Entity<Vote>()
                .HasOne(v => v.User)
                .WithMany(u => u.Votes)
                .HasForeignKey(v => v.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Ticket - Feedback ilişki tanımı
            modelBuilder.Entity<Opinion>()
                .HasOne(f => f.Ticket)
                .WithMany(t => t.Opinions)
                .HasForeignKey(f => f.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

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

            // Benzersiz alan kısıtlamaları
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();
        }
    }
}
