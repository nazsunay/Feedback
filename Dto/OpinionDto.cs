using Feedback.Entity;

namespace Feedback.Dto
{
    public class OpinionDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public OpinionStatus Status { get; set; }
        public OpinionCategory Category { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserId { get; set; } // UserId alanını ekleyin
        public List<DtoAddComment> Comments { get; set; }
    }

}
