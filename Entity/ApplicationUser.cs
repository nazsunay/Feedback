using Microsoft.AspNetCore.Identity;

namespace Feedback.Entity
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName {  get; set; }
        public string LastName { get; set; }
        public string Avatar {  get; set; }
        public string Nickname {  get; set; }
        public string FullName => $"{FirstName}";
    }
    public class Register
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Avatar { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class Login
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
