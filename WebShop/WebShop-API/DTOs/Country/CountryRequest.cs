namespace WebShop_API.DTOs.Country
{
    public class CountryRequest
    {
        [Required(ErrorMessage = "Country name is required")]
        public string CountryName { get; set; } = string.Empty;
    }
}
