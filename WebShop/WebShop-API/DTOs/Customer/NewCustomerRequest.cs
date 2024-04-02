namespace WebShop_API.DTOs.Customer
{
    public class NewCustomerRequest
    {
        [Required( ErrorMessage = "* is required" )]
        public AccountRequest Account { get; set; } = null!;

        [Required( ErrorMessage = "* is required" )]
        public CustomerRequest Customer { get; set; } = null!;
    }
}
