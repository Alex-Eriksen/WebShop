namespace WebShop_API.DTOs.Authentication
{
    public class AuthenticationRequest
    {
        [Required( ErrorMessage = "* must not be empty!" )]
        public string Username_Email { get; set; } = string.Empty;

        [Required( ErrorMessage = "* must not be empty!" )]
        public string Password { get; set; } = string.Empty;
    }
}
