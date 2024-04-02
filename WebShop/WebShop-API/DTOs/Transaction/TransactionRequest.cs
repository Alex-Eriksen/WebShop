namespace WebShop_API.DTOs.Transaction
{
    public class TransactionRequest
    {
        [Required(ErrorMessage = "ProductID is required")]
        public int ProductID { get; set; }

        [Required(ErrorMessage = "OrderID is required")]
        public int OrderID { get; set; }

        [Required(ErrorMessage = "Product amount is required")]
        public int ProductAmount { get; set; } = 0;

        [Required(ErrorMessage = "Product price is required")]
        public double ProductPrice { get; set; } = 0;

        public int? DiscountID { get; set; }

        public DateTime Created_At { get; set; }

        public DateTime Modified_At { get; set; }
    }
}
