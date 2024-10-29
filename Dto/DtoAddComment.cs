namespace Feedback.Dto
{
    public class CommentDto
    {
       
            public int Id { get; set; }
            public string Content { get; set; }
            public DateTime CreatedAt { get; set; }
            
            public int OpinionId { get; set; } // Bu satırı ekleyin
        

    }

}
