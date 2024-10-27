namespace Feedback.Entity
{


    public class Ticket
    {
        public int Id { get; set; }                // Primary key
        public string Title { get; set; }                // Bilet başlığı
        public string Description { get; set; }          // Bilet açıklaması
        public TicketStatus Status { get; set; }         // Bilet durumu (enum: Open, Closed, InProgress)
        public DateTime CreatedAt { get; set; }          // Oluşturulma tarihi
        public DateTime? UpdatedAt { get; set; }         // Güncellenme tarihi (opsiyonel)

        // İlişkiler
        public ICollection<Opinion> Opinions { get; set; }    // Biletin geri bildirimleri
    }
}
