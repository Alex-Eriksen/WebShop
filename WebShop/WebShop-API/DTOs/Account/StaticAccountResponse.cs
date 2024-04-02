namespace WebShop_API.DTOs.Account
{
    public class StaticAccountResponse
    {
        public int AccountID { get; set; } = 0;

        public string Username { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;
    }
}
