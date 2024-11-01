using Feedback.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Feedback.Data
{
    public class _context : IdentityDbContext<ApplicationUser>
    {
        public _context(DbContextOptions options) : base(options)
        {
        }
        public DbSet<FeedbackUser> FeedbackUsers { get; set; } 
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

            // Comment - Comment (Kendi kendine referans)
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.ParentComment)
                .WithMany(c => c.Replies)
                .HasForeignKey(c => c.ParentCommentId)
                .OnDelete(DeleteBehavior.Cascade);

            // User - Vote ilişki tanımı
            modelBuilder.Entity<Vote>()
                .HasOne(v => v.User)
                .WithMany(u => u.Votes)
                .HasForeignKey(v => v.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Feedback - Comment ilişki tanımı
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Opinions)
                .WithMany(f => f.Comments)
                .HasForeignKey(c => c.OpinionId)
                .OnDelete(DeleteBehavior.Restrict); // Cascade'ı kaldırdık

            // Feedback - Vote ilişki tanımı
            modelBuilder.Entity<Vote>()
                .HasOne(v => v.Opinions)
                .WithMany(f => f.Votes)
                .HasForeignKey(v => v.OpinionId)
                .OnDelete(DeleteBehavior.Restrict); // Cascade'ı kaldırdık

            // Kullanıcı - Opinion ilişkisi
            modelBuilder.Entity<Opinion>()
                .HasOne(o => o.User)
                .WithMany(u => u.Opinions)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Kullanıcı - Comment ilişkisi
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Kullanıcı - FeedbackUser ilişkisi
            modelBuilder.Entity<FeedbackUser>()
                .HasOne(fu => fu.User)
                .WithMany(u => u.FeedbackUsers)
                .HasForeignKey(fu => fu.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // FeedbackUser - Opinion ilişkisi
            modelBuilder.Entity<FeedbackUser>()
                .HasOne(fu => fu.Opinion)
                .WithMany()
                .HasForeignKey(fu => fu.OpinionId)
                .OnDelete(DeleteBehavior.Restrict); // Cascade'ı kaldırdık
        }


    }
}
