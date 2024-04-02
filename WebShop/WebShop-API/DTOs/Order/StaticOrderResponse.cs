namespace WebShop_API.DTOs.Order
{
    public class StaticOrderResponse
    {
        public int OrderID { get; set; }

        public int CustomerID { get; set; }

        public double OrderTotal { get; set; } = 0;
    }
}
