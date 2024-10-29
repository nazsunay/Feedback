using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Feedback.Entity
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Avatar { get; set; }
        public string Nickname { get; set; }

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        // İlişkiler
        public ICollection<Opinion> Opinions { get; set; } = new List<Opinion>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Vote> Votes { get; set; } = new List<Vote>();
        public ICollection<FeedbackUser> FeedbackUsers { get; set; } = new List<FeedbackUser>(); // Yeni koleksiyon
    }

    public class Register
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Avatar { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class Login
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class FeedbackUser
    {
        public int Id { get; set; } // Primary key
        public string UserId { get; set; } // İlişkili kullanıcının ID'si
        public int OpinionId { get; set; } // İlişkili geri bildirimin ID'si (int olarak değiştirdik)
        public ApplicationUser User { get; set; } // İlişkili kullanıcı
        public Opinion Opinion { get; set; } // İlişkili geri bildirim
    }


}
