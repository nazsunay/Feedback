namespace Feedback.Dto
{
    public class DtoAddUser
    {
        public int Id { get; set; }                  // Primary key
        public string Username { get; set; }             // Kullanıcı adı, benzersiz
        public string Email { get; set; }                // E-posta adresi, benzersiz
        public string PasswordHash { get; set; }         // Şifre hash'i
        public DateTime CreatedAt { get; set; }          // Oluşturulma tarihi

    }
}
