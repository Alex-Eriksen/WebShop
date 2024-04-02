namespace WebShop_API.DTOs.ProductType
{
    public class DirectProductTypeResponse
    {
        public int ProductTypeID { get; set; }

        public string ProductTypeName { get; set; } = string.Empty;

        public List<StaticProductResponse> Products { get; set; } = new List<StaticProductResponse>();
    }
}
