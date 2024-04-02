namespace WebShop_API.DTOs.Transaction
{
    public class DirectTransactionResponse
    {
        public int TransactionID { get; set; }

        public StaticProductResponse Product { get; set; } = null!;

        public StaticOrderResponse Order { get; set; } = null!;

        public int ProductAmount { get; set; } = 0;

        public double ProductPrice { get; set; } = 0;

        public StaticDiscountResponse? Discount { get; set; }

        public DateTime Created_At { get; set; }
        
        public DateTime Modified_At { get; set; }
    }
}
