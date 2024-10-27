using Feedback.Data;
using Feedback.Dto;
using Feedback.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Feedback.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DtoAddUser>>> GetUsers()
        {
            var users = await _context.Users
                .Select(user => new DtoAddUser
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    CreatedAt = user.CreatedAt,
                    PasswordHash = user.PasswordHash
                })
                .ToListAsync();

            return Ok(users);
        }

        // GET: api/User/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<DtoAddUser>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var userDto = new DtoAddUser
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                PasswordHash = user.PasswordHash,
            };

            return Ok(userDto);
        }

        // POST: api/User
        [HttpPost]
        public async Task<ActionResult<DtoAddUser>> CreateUser([FromBody] DtoAddUser newUserDto)
        {
            var newUser = new User
            {
                Username = newUserDto.Username,
                Email = newUserDto.Email,
                CreatedAt = DateTime.UtcNow,
                PasswordHash = newUserDto.PasswordHash // Varsayılan şifre hash'i veya şifre ayarlama işlemi yapılmalı.
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            newUserDto.Id = newUser.Id;
            newUserDto.CreatedAt = newUser.CreatedAt;

            return CreatedAtAction(nameof(GetUser), new { id = newUser.Id }, newUserDto);
        }

        // PUT: api/User/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] DtoAddUser updatedUserDto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Güncellenmiş bilgileri ayarla
            user.Username = updatedUserDto.Username;
            user.Email = updatedUserDto.Email;
            user.PasswordHash = updatedUserDto.PasswordHash;

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/User/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }


}
