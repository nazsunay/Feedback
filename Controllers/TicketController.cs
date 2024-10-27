using Feedback.Data;
using Feedback.Dto;
using Feedback.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Feedback.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TicketController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public ActionResult<IEnumerable<Ticket>> GetTickets()
        {
            return _context.Tickets.ToList();
        }

        // GET: api/ticket/{id}
        [HttpGet("{id}")]
        public ActionResult<Ticket> GetTicket(int id)
        {
            var ticket = _context.Tickets.Find(id);
            if (ticket == null)
            {
                return NotFound();
            }
            return ticket;
        }

        // POST: api/ticket
        [HttpPost]
        public ActionResult<Ticket> CreateTicket(DtoAddTicket dtoAddTicket)
        {
            // DTO'dan yeni Ticket nesnesi oluşturun
            var ticket = new Ticket
            {
                // DTO'daki alanları Ticket nesnesine atayın
                Title = dtoAddTicket.Title,
                Description = dtoAddTicket.Description,
                CreatedAt = DateTime.UtcNow
            };

            _context.Tickets.Add(ticket);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetTicket), new { id = ticket.Id }, ticket);
        }

        // PUT: api/ticket/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateTicket(int id, DtoAddTicket dtoAddTicket)
        {
            var ticket = _context.Tickets.Find(id);
            if (ticket == null)
            {
                return NotFound();
            }

            ticket.Title = dtoAddTicket.Title;
            ticket.Description = dtoAddTicket.Description;
            ticket.UpdatedAt = DateTime.UtcNow;

            _context.Entry(ticket).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketExists(id))
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

        // DELETE: api/ticket/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteTicket(int id)
        {
            var ticket = _context.Tickets.Find(id);
            if (ticket == null)
            {
                return NotFound();
            }

            _context.Tickets.Remove(ticket);
            _context.SaveChanges();

            return NoContent();
        }

        private bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.Id == id);
        }
    }
}
