namespace Feedback.Entity
{
    public class User
    {
        public int Id { get; set; }                  // Primary key
        public string Username { get; set; }             // Kullanıcı adı, benzersiz
        public string Email { get; set; }                // E-posta adresi, benzersiz
        public string PasswordHash { get; set; }         // Şifre hash'i
        public DateTime CreatedAt { get; set; }          // Oluşturulma tarihi

        // İlişkiler
        public ICollection<Opinion> Opinions { get; set; }    // Kullanıcının geri bildirimleri
        public ICollection<Comment> Comments { get; set; }      // Kullanıcının yorumları
        public ICollection<Vote> Votes { get; set; }            // Kullanıcının oyları
    }
}
