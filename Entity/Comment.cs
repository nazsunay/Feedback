namespace Feedback.Entity
{
    public class Comment
    {
        public int Id { get; set; }               // Primary key
        public string Content { get; set; }              // Yorum içeriği
        public DateTime CreatedAt { get; set; }          // Oluşturulma tarihi

        // İlişkiler
        public int OpinionId { get; set; }              // İlişkili geri bildirimin ID'si
        public Opinion Opinions { get; set; }           // İlişkili geri bildirim
        public int UserId { get; set; }                  // İlişkili kullanıcının ID'si
        public User User { get; set; }                   // İlişkili kullanıcı
    }

}
