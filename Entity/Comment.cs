
using Feedback.Entity;

public class Comment
{
    public int Id { get; set; } // Primary key
    public string Content { get; set; } // Yorum içeriği
    public DateTime CreatedAt { get; set; } // Oluşturulma tarihi
    public string UserId { get; set; } // İlişkili kullanıcının ID'si
    public int OpinionId { get; set; } // İlişkili Opinion'un ID'si
    public int? ParentCommentId { get; set; } // Eğer varsa, üst yorumun ID'si
    public Comment ParentComment { get; set; }

    // Navigasyon özellikleri
    public virtual ApplicationUser User { get; set; } // İlişkili kullanıcı
    public virtual Opinion Opinion { get; set; } // İlişkili Opinion
    public virtual ICollection<Comment> Replies { get; set; } = new List<Comment>(); // Yanıtlar
}
