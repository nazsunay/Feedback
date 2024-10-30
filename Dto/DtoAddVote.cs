namespace Feedback.Dto
{
    public class DtoAddVote
    {
        public int OpinionId { get; set; }  // Oy verilecek geri bildirimin ID'si
        public bool IsUpvote { get; set; }   // Oy türü: true ise olumlu, false ise olumsuz
    }

}
