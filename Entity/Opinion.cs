using Newtonsoft.Json;

namespace Feedback.Entity
{
    public enum OpinionCategory
    {
        All,
        UI,
        UX,
        Enhancement,
        Bug,
        Feature,
        General,
        Feedback    // Bu değeri eklemeyi unutmuş olabilirsiniz
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
        // İlişkiler
        public string UserId { get; set; } // İlişkili kullanıcının ID'si
       
        public ApplicationUser User { get; set; } // İlişkili kullanıcı

        public ICollection<Comment> Comments { get; set; }
        public ICollection<Vote> Votes { get; set; }
        public int VoteCount { get; set; }
    }

}
