using Feedback.Data;
using Feedback.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Feedback.Controllers
{
   
        
    [Route("api/[controller]")]
        [ApiController]
        public class CommentController : ControllerBase
        {
            private readonly AppDbContext _context;

            public CommentController(AppDbContext context)
            {
                _context = context;
            }

            // GET: api/comment
            [HttpGet]
            public async Task<ActionResult<IEnumerable<Comment>>> GetComments()
            {
                return await _context.Comments.Include(c => c.User).ToListAsync();
            }

            // GET: api/comment/{id}
            [HttpGet("{id}")]
            public async Task<ActionResult<Comment>> GetComment(int id)
            {
                var comment = await _context.Comments.Include(c => c.User)
                                                     .FirstOrDefaultAsync(c => c.Id == id);

                if (comment == null)
                {
                    return NotFound();
                }

                return comment;
            }

            // POST: api/comment
            [HttpPost]
            public async Task<ActionResult<Comment>> PostComment(Comment comment)
            {
                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetComment), new { id = comment.Id }, comment);
            }

            // PUT: api/comment/{id}
            [HttpPut("{id}")]
            public async Task<IActionResult> PutComment(int id, Comment comment)
            {
                if (id != comment.Id)
                {
                    return BadRequest();
                }

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

