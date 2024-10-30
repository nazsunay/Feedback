using Feedback.Data;
using Feedback.Dto;
using Feedback.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static Feedback.Dto.DtoAddVote;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Feedback.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoteController : ControllerBase
    {
        private readonly _context _context;

        public VoteController(_context context)
        {
            _context = context;
        }
        // GET: api/Votes
        [HttpGet]
        public async Task<IActionResult> GetAllVotes()
        {
            var votes = await _context.Votes
                
                .Include(v => v.Opinions) // Opinion bilgilerini dahil et
                .ToListAsync();

            if (votes == null || !votes.Any())
            {
                return NotFound("Hiç oy bulunamadı.");
            }

            // Listeyi DTO (Data Transfer Object) ile döndürmek isterseniz:
            var voteList = votes.Select(v => new
            {
                VoteId = v.Id,
                
                OpinionId = v.OpinionId,
                OpinionTitle = v.Opinions.Title
            });

            return Ok(voteList);
        }


        [HttpPost]
        public async Task<IActionResult> VoteForOpinion([FromBody] DtoAddVote voteDto)
        {
            if (voteDto == null)
            {
                return BadRequest("Vote data is required.");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated.");
            }

            // İlgili geri bildirimi bul
            var opinion = await _context.Opinions.Include(o => o.Votes).FirstOrDefaultAsync(o => o.Id == voteDto.OpinionId);
            if (opinion == null)
            {
                return NotFound("Opinion not found.");
            }

            // Kullanıcının daha önce oy verip vermediğini kontrol et
            var existingVote = await _context.Votes.FirstOrDefaultAsync(v => v.UserId == userId && v.OpinionId == voteDto.OpinionId);

            if (existingVote == null)
            {
                // Yeni oy ekle
                var newVote = new Vote
                {
                    UserId = userId,
                    OpinionId = voteDto.OpinionId
                };

                _context.Votes.Add(newVote);
                opinion.VoteCount++;  // Oy sayısını artır
            }
            else
            {
                // Kullanıcı mevcut oyu kaldırmak istiyorsa
                _context.Votes.Remove(existingVote);
                opinion.VoteCount--;  // Oy sayısını azalt
            }

            // Değişiklikleri kaydet
            await _context.SaveChangesAsync();
            return Ok(opinion.VoteCount); // Güncel oy sayısını döndür
        }

    }
}
