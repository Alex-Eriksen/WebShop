namespace WebShop_API.DTOs.Account
{
    public class AccountRequest
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(32, MinimumLength = 6, ErrorMessage = "Username must be between 6 and 32 characters")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        //[StringLength(255, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 255  characters")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [StringLength(100, ErrorMessage = "Name must not contain more than 100 characters")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = string.Empty;

        [StringLength(32, ErrorMessage = "* must not contains more than 32 characters.")]
        public string Role { get; set; } = "Customer";
    }
}
