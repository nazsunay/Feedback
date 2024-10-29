﻿using Feedback.Data;
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

        [HttpPost]
        public async Task VoteForOpinion(string userId, int opinionId)
        {
            // Kullanıcının bu geri bildirimle ilgili bir kaydı olup olmadığını kontrol et
            var existingFeedbackUser = await _context.FeedbackUsers
                .FirstOrDefaultAsync(fu => fu.UserId == userId && fu.OpinionId == opinionId);

            var opinion = await _context.Opinions.FindAsync(opinionId);

            if (opinion == null)
            {
                throw new InvalidOperationException("Opinion not found.");
            }

            if (existingFeedbackUser == null)
            {
                // Oy yoksa yeni oy ver ve geri bildirim sayısını artır
                var newVote = new Vote
                {
                    UserId = userId,
                    OpinionId = opinionId
                };

                _context.Votes.Add(newVote);
                opinion.VoteCount++;

                var feedbackUser = new FeedbackUser
                {
                    UserId = userId,
                    OpinionId = opinionId
                };
                _context.FeedbackUsers.Add(feedbackUser);
            }
            else
            {
                // Oy varsa oy sayısını düşür ve geri bildirim kaydını sil
                var existingVote = await _context.Votes
                    .FirstOrDefaultAsync(v => v.UserId == userId && v.OpinionId == opinionId);

                if (existingVote != null)
                {
                    _context.Votes.Remove(existingVote);
                    opinion.VoteCount--;
                }

                _context.FeedbackUsers.Remove(existingFeedbackUser);
            }

            await _context.SaveChangesAsync();
        }


    }
}
