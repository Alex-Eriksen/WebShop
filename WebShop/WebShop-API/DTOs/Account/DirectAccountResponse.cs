using WebShop_API.DTOs.Customer;

namespace WebShop_API.DTOs.Account
{
    public class DirectAccountResponse
    {
        public int AccountID { get; set; } = 0;

        public string Username { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public StaticCustomerResponse Customer { get; set; } = null!;
    }
}
