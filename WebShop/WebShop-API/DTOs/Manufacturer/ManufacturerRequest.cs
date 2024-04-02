namespace WebShop_API.DTOs.Manufacturer
{
    public class ManufacturerRequest
    {
        [Required(ErrorMessage = "Manufacturer name is required")]
        public string ManufacturerName { get; set; } = string.Empty;
    }
}
