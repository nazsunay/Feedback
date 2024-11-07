using Feedback.Data;
using Feedback.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Feedback.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        
        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(Register model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Avatar = model.Avatar,
                Nickname = model.Nickname
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return Ok("Kullanıcı kaydedildi.");
                //persistent kalıcı olarak kaydedilme işlemi
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(Login model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return Ok("Giriş yapıldı.");

            }

            return Unauthorized("Bu kullanıcı bulunamadı.");
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new
            {
                succes = true,
                msg = "Çıkış Yapıldı"
            });
        }

        //[Authorize]//isteği göndermek için kullanıcının login olması gerkli 
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound("Kullanıcı girişi yapılmamış.");
            }

            var userInfo = new
            {
                user.Id, 
                user.FirstName,
                user.LastName,
                user.Email,
                user.Avatar,
                user.Nickname
            };

            return Ok(userInfo);
        }

        // Kullanıcı ID ile bilgilerini al
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound("Kullanıcı bulunamadı.");
            }

            var userInfo = new
            {
                user.FirstName,
                user.LastName,
                user.Email,
                user.Avatar,
                user.Nickname
            };

            return Ok(userInfo);
        }
        // Tüm kullanıcıları listele
        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();

            var userInfos = users.Select(user => new
            {
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                user.Avatar,
                user.Nickname
            }).ToList();

            return Ok(userInfos);
        }


    }
}
