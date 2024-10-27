using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Feedback.Data;
using Feedback.Entity;
using static Feedback.Dto.DtoAddOpinion;
using Feedback.Dto;

namespace Feedback.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OpinionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OpinionsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Opinions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Opinion>>> GetOpinions()
        {
            return await _context.Opinions.ToListAsync();
        }

        // GET: api/Opinions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DtoAddOpinion>> GetOpinion(int id)
        {
            var opinion = await _context.Opinions
                .Include(o => o.User)
                .Include(o => o.Ticket)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (opinion == null)
            {
                return NotFound();
            }

            var dto = new DtoAddOpinion
            {
                Id = opinion.Id,
                Title = opinion.Title,
                Description = opinion.Description,
                Status = opinion.Status,
                Category = opinion.Category,
                CreatedAt = opinion.CreatedAt,
                UserId = opinion.UserId,
                TicketId = opinion.TicketId
            };

            return dto;
        }


        // PUT: api/Opinions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOpinion(int id, Opinion opinion)
        {
            if (id != opinion.Id)
            {
                return BadRequest();
            }

            _context.Entry(opinion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OpinionExists(id))
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

        // POST: api/Opinions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Opinion>> PostOpinion(Opinion opinion)
        {
            _context.Opinions.Add(opinion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOpinion", new { id = opinion.Id }, opinion);
        }

        // DELETE: api/Opinions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOpinion(int id)
        {
            var opinion = await _context.Opinions.FindAsync(id);
            if (opinion == null)
            {
                return NotFound();
            }

            _context.Opinions.Remove(opinion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OpinionExists(int id)
        {
            return _context.Opinions.Any(e => e.Id == id);
        }
    }
}
