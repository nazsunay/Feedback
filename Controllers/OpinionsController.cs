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
                return BadRequest();
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
        // POST: api/Opinions
        [HttpPost]
        public async Task<ActionResult<DtoAddOpinion>> CreateOpinion(DtoAddOpinion dtoOpinion)
        {
            // DTO'dan Opinion nesnesini oluşturma
            var opinion = new Opinion
            {
                Title = dtoOpinion.Title,
                Description = dtoOpinion.Description,
                Status = dtoOpinion.Status,
                Category = dtoOpinion.Category,
                CreatedAt = DateTime.UtcNow, // Oluşturulma tarihini otomatik ayarlıyoruz
                UserId = dtoOpinion.UserId,
                TicketId = dtoOpinion.TicketId
            };

            // Opinion nesnesini veritabanına ekliyoruz
            _context.Opinions.Add(opinion);
            await _context.SaveChangesAsync(); // Değişiklikleri kaydediyoruz

            // Yeni eklenen Opinion'un ID'sini DTO'ya geri aktarıyoruz
            dtoOpinion.Id = opinion.Id;

            // Başarılı ekleme için 201 Created yanıtı dönüyoruz
            return CreatedAtAction(nameof(GetOpinion), new { id = opinion.Id }, dtoOpinion);
        }
        // PUT: api/Opinions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOpinion(int id, DtoAddOpinion dtoOpinion)
        {
            // Güncellenmek istenen kayıt olup olmadığını kontrol ediyoruz
            var opinion = await _context.Opinions.FindAsync(id);

            if (opinion == null)
            {
                return BadRequest(); // Kayıt bulunamazsa 404 dön
            }

            // DTO'daki verileri mevcut Opinion nesnesine aktarma
            opinion.Title = dtoOpinion.Title;
            opinion.Description = dtoOpinion.Description;
            opinion.Status = dtoOpinion.Status;
            opinion.Category = dtoOpinion.Category;
            opinion.UserId = dtoOpinion.UserId;
            opinion.TicketId = dtoOpinion.TicketId;

            // Değişiklikleri kaydetme
            try
            {
                await _context.SaveChangesAsync(); // Güncelleme işlemi
            }
            catch (DbUpdateConcurrencyException)
            {
                // Güncelleme sırasında hata olursa kayıt halen mevcut mu kontrol ediyoruz
                if (!_context.Opinions.Any(o => o.Id == id))
                {
                    return BadRequest(); // Kayıt bulunmazsa 404 dön
                }
                else
                {
                    throw; // Başka bir hata olursa fırlat
                }
            }

            return Ok(); // Başarılı güncelleme sonrası 204 No Content
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
