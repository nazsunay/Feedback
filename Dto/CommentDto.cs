namespace Feedback.Dto
{
    public class CommentDto
    {

        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public int OpinionId { get; set; }
        public int? ParentCommentId { get; set; } // Nullable for root comments
        public List<CommentDto> Replies { get; set; } = new List<CommentDto>(); // Alt yorumlar


    }

}
