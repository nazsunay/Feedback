using Feedback.Data;
using Feedback.Dto;
using Feedback.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Feedback.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly _context _context;

        public CommentController(_context context)
        {
            _context = context;
        }

        // GET: api/comment
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetComments()
        {
            var comments = await _context.Comments
                
                .Select(c => new CommentDto
                {
                    Id = c.Id,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt,
                    
                    OpinionId = c.OpinionId
                })
                .ToListAsync();

            return comments;
        }

        // GET: api/comment/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CommentDto>> GetComment(int id)
        {
            var comment = await _context.Comments
                
                .Select(c => new CommentDto
                {
                    Id = c.Id,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt,
                    
                    OpinionId= c.OpinionId
                })
                .FirstOrDefaultAsync(c => c.Id == id);

            if (comment == null)
            {
                return NotFound();
            }

            return comment;
        }

        // POST: api/comment
        [HttpPost]
        public async Task<ActionResult<CommentDto>> PostComment(CommentDto commentDto)
        {
            // Geçerli bir OpinionId kontrolü
            var opinionExists = await _context.Opinions.AnyAsync(o => o.Id == commentDto.OpinionId);
            if (!opinionExists)
            {
                return BadRequest("Geçersiz görüş ID.");
            }

            var comment = new Comment
            {
                Content = commentDto.Content,
                CreatedAt = DateTime.UtcNow,
                
                OpinionId = commentDto.OpinionId // Burada OpinionId'yi ayarlıyoruz
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            commentDto.Id = comment.Id; // Set the Id of the newly created comment

            return CreatedAtAction(nameof(GetComment), new { id = comment.Id }, commentDto);
        }


        // PUT: api/comment/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComment(int id, CommentDto commentDto)
        {
            if (id != commentDto.Id)
            {
                return BadRequest();
            }

            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            comment.Content = commentDto.Content;
            
            // Update CreatedAt or keep it unchanged based on your requirements

            _context.Entry(comment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/comment/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }
    }
}
