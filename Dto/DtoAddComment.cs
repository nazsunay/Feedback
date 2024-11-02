public class DtoAddComment
{
    public int Id { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public int UserId { get; set; }
    public int OpinionId { get; set; }
    public int? ParentCommentId { get; set; } // Nullable int
    public List<DtoAddComment> Replies { get; set; }
}
