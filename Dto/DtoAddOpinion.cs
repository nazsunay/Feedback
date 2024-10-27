using Feedback.Entity;

namespace Feedback.Dto
{
    public class DtoAddOpinion
    {
        public int Id { get; set; }              // Primary key
        public string Title { get; set; }        // Geri bildirim başlığı
        public string Description { get; set; }  // Geri bildirim açıklaması
        public OpinionStatus Status { get; set; } // Geri bildirim durumu
        public OpinionCategory Category { get; set; } // Geri bildirim kategorisi
        public DateTime CreatedAt { get; set; }  // Oluşturulma tarihi
        public int UserId { get; set; }          // İlişkili kullanıcının ID'si
        public int TicketId { get; set; }

    }
}
