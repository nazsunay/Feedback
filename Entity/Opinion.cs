﻿namespace Feedback.Entity
{
    public enum OpinionCategory
    {
        Feature,
        Bug,
        Improvement,
        Other
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
        public int TicketId { get; set; }        // İlişkili biletin ID'si
        public Ticket Ticket { get; set; }       // İlişkili bilet
        public ICollection<Comment> Comments { get; set; } // Geri bildirime yapılan yorumlar
        public ICollection<Vote> Votes { get; set; } // Geri bildirimdeki oylar
    }
}