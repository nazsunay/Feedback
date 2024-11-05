public class DtoAddComment
{
    public int Id { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public string UserId { get; set; }
    public int OpinionId { get; set; }


    // Alt yorumları ekleyelim
}