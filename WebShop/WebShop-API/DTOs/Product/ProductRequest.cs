namespace WebShop_API.DTOs.Product
{
    public class ProductRequest
    {
        [Required(ErrorMessage = "Product price is required")]
        public double ProductPrice { get; set; } = 0;

        [Required(ErrorMessage = "Product name is required")]
        public string ProductName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Product quantity is required")]
        public int ProductQuantity { get; set; } = 0;

        [Required(ErrorMessage = "Product description is required")]
        public string ProductDescription { get; set; } = string.Empty;

        [Required(ErrorMessage = "ManufacturerID is required")]
        public int ManufacturerID { get; set; }

        [Required(ErrorMessage = "CategoryID is required")]
        public int CategoryID { get; set; }

        public DateTime ReleaseDate { get; set; }

        [Required(ErrorMessage = "ProductTypeID is required")]
        public int ProductTypeID { get; set; }

        public int? DiscountID { get; set; }

        public List<PhotoRequest> Photos { get; set; } = new();
    }
}
