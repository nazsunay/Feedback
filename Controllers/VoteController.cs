using Feedback.Data;
using Feedback.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Feedback.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoteController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VoteController(AppDbContext context)
        {
            _context = context;
        }

        // Oy ekleme işlemi
        [HttpPost("{opinionId}/votes")]
        public async Task<IActionResult> AddVote(int opinionId, Vote vote)
        {
            // İlgili opinion'u bul
            var opinion = await _context.Opinions
                              .Include(o => o.Votes) // Votes koleksiyonunu dahil et
                              .FirstOrDefaultAsync(o => o.Id == opinionId);

            if (opinion == null)
            {
                return NotFound("Opinion bulunamadı.");
            }

            // Oyu ekle
            opinion.Votes.Add(vote);

            // Değişiklikleri kaydet
            await _context.SaveChangesAsync();

            return Ok("Oy başarıyla eklendi.");
        }


        // Oy silme işlemi
        [HttpDelete("{opinionId}/votes/{voteId}")]
        public async Task<IActionResult> RemoveVote(int opinionId, int voteId)
        {
            // İlgili opinion'u bul
            var opinion = await _context.Opinions
                              .Include(o => o.Votes) // Votes koleksiyonunu dahil et
                              .FirstOrDefaultAsync(o => o.Id == opinionId);

            if (opinion == null)
            {
                return NotFound("Opinion bulunamadı.");
            }

            // Oyu bul
            var vote = opinion.Votes.FirstOrDefault(v => v.Id == voteId);

            if (vote == null)
            {
                return NotFound("Oy bulunamadı.");
            }

            // Oyu kaldır
            opinion.Votes.Remove(vote);

            // Değişiklikleri kaydet
            await _context.SaveChangesAsync();

            return NoContent(); // Başarılı silme
        }

    }
}
