namespace WebShop_API.DTOs.Customer
{
    public class CustomerRequest 
    {
        [Required(ErrorMessage = "* is required")]
        public int AccountID { get; set; }

        [Required(ErrorMessage = "* is required")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "* is required")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "* is required")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "* must be 8 characters")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "* is required")]
        public int CountryID { get; set; }

        [Required(ErrorMessage = "* is required")]
        public int ZipCode { get; set; }

        [Required(ErrorMessage = "* is required")]
        public string Gender { get; set; } = string.Empty;
    }
}
