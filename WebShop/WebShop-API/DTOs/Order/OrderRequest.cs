namespace WebShop_API.DTOs.Order
{
    public class OrderRequest
    {
        [Required(ErrorMessage = "CustomerID is required")]
        public int CustomerID { get; set; }
        
        [Required(ErrorMessage = "OrderTotal is required")]
        public double OrderTotal { get; set; }
    }
}
