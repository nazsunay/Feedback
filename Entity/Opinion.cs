namespace Feedback.Entity
{
    public enum OpinionCategory
    {
        All,
        UI,
        UX,
        Enhancement,
        Bug,
        Feature
    }

    public class Opinion
    {
        public int Id { get; set; }              // Primary key
        public string Title { get; set; }        // Geri bildirim başlığı
        public string Description { get; set; }  // Geri bildirim açıklaması
        public OpinionStatus Status { get; set; } // Geri bildirim durumu
        public OpinionCategory Category { get; set; } // Geri bildirim kategorisi
        public DateTime CreatedAt { get; set; }  // Oluşturulma tarihi

        // İlişkiler
        public int UserId { get; set; }          // İlişkili kullanıcının ID'si
        public User User { get; set; }           // İlişkili kullanıcı

        public ICollection<Comment> Comments { get; set; } // Yorumlar
        public ICollection<Vote> Votes { get; set; }        // Oylar

        public int VoteCount { get; set; } // Oy sayısı
    }

}
