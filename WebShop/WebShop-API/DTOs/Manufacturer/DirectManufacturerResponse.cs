namespace WebShop_API.DTOs.Manufacturer
{
    public class DirectManufacturerResponse
    {
        public int ManufacturerID { get; set; } = 0;

        public string ManufacturerName { get; set; } = string.Empty;

        public List<StaticProductResponse> Products { get; set; } = new List<StaticProductResponse>();
    }
}
