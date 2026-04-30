using System.ComponentModel.DataAnnotations;

namespace IMDBSample.Models.Request
{
    public class SignupRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}