﻿using System;
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
        private readonly _context _context;

        public OpinionsController(_context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DtoAddOpinion>>> GetOpinions()
        {
            var opinions = await _context.Opinions
                .Include(o => o.User) // Kullanıcıyı dahil et
                .Select(o => new DtoAddOpinion
                {
                    Id = o.Id,
                    Title = o.Title,
                    Description = o.Description,
                    Status = o.Status,
                    Category = o.Category, // Enum olarak döndürülecek
                    CreatedAt = o.CreatedAt,
                    UserId = o.UserId, // Kullanıcı ID'sini al
                })
                .ToListAsync();

            return Ok(opinions);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<OpinionDto>> GetOpinion(int id)
        {
            var opinion = await _context.Opinions
                .Include(o => o.Comments)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (opinion == null)
            {
                return NotFound();
            }

            var dto = new OpinionDto
            {
                Id = opinion.Id,
                Title = opinion.Title,
                Description = opinion.Description,
                Status = opinion.Status,
                Category = opinion.Category,
                CreatedAt = opinion.CreatedAt,
                UserId = opinion.UserId, // UserId'yi burada al

                Comments = opinion.Comments.Select(c => new DtoAddComment
                {
                    Id = c.Id,
                    Content = c.Content,
                    UserId = c.UserId,
                    // Diğer gerekli özellikler
                }).ToList()
            };

            return dto;
        }





        // POST: api/Opinions
        [HttpPost]
        public async Task<ActionResult<DtoAddOpinion>> CreateOpinion(DtoAddOpinion dtoOpinion)
        {
            
            var opinion = new Opinion
            {
                Title = dtoOpinion.Title,
                Description = dtoOpinion.Description,
                Status = dtoOpinion.Status,
                Category = dtoOpinion.Category,
                CreatedAt = DateTime.UtcNow, 
                UserId = dtoOpinion.UserId,
                
            };

            
            _context.Opinions.Add(opinion);
            await _context.SaveChangesAsync(); 

            
            dtoOpinion.Id = opinion.Id;

            
            return CreatedAtAction(nameof(GetOpinion), new { id = opinion.Id }, dtoOpinion);
        }
        // PUT: api/Opinions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOpinion(int id, DtoAddOpinion dtoOpinion)
        {
           
            var opinion = await _context.Opinions.FindAsync(id);

            if (opinion == null)
            {
                return BadRequest(); 
            }

            
            opinion.Title = dtoOpinion.Title;
            opinion.Description = dtoOpinion.Description;
            opinion.Status = dtoOpinion.Status;
            opinion.Category = dtoOpinion.Category;
            opinion.UserId = dtoOpinion.UserId;
            

            
            try
            {
                await _context.SaveChangesAsync(); // 
            }
            catch (DbUpdateConcurrencyException)
            {
                
                if (!_context.Opinions.Any(o => o.Id == id))
                {
                    return BadRequest(); 
                }
                else
                {
                    throw; 
                }
            }

            return Ok(); 
        }
        // DELETE: api/Opinions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOpinion(int id)
        {
            // Silinecek kaydı buluyoruz
            var opinion = await _context.Opinions.FindAsync(id);

            if (opinion == null)
            {
                return BadRequest(); // Kayıt bulunamazsa 404 döner
            }

            // Kayıt siliniyor
            _context.Opinions.Remove(opinion);

            try
            {
                await _context.SaveChangesAsync(); // Değişiklikleri kaydet
            }
            catch (Exception ex)
            {
                // Hata durumunda detaylı bilgi loglanabilir
                return StatusCode(500, $"Kayıt silinirken bir hata oluştu: {ex.Message}");
            }

            return Ok(); // Başarılı silme sonrası 204 No Content
        }

        

    }
}
