namespace WebShop_API.DTOs.Product
{
    public class StaticProductResponse
    {
        public int ProductID { get; set; } = 0;

        public string ProductName { get; set; } = string.Empty;

        public double ProductPrice { get; set; } = 0;

        public int ProductQuantity { get; set; } = 0;

        public string ProductDescription { get; set; } = string.Empty;

        public int ManufacturerID { get; set; } = 0;

        public int CategoryID { get; set; }

        public DateTime ReleaseDate { get; set; }

        public int ProductTypeID { get; set; } = 0;

        public int? DiscountID { get; set; }

        public string? ImageName { get; set; }
    }
}
