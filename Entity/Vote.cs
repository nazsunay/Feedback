namespace Feedback.Entity
{
    public class Vote
    {
        public int Id { get; set; }                  // Primary key

        // İlişkiler
        public string UserId { get; set; }                  // İlişkili kullanıcının ID'si
       
        public int OpinionId { get; set; }              // İlişkili geri bildirimin ID'si
        public Opinion Opinions { get; set; }           // İlişkili geri bildirim
        public ApplicationUser User { get; set; } // İlişkili kullanıcı

    }
}
