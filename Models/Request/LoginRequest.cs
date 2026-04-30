using System.ComponentModel.DataAnnotations;

namespace IMDBSample.Models.Request
{
    public class LoginRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}