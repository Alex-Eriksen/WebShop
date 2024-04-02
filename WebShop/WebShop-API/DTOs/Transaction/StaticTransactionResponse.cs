namespace WebShop_API.DTOs.Transaction
{
    public class StaticTransactionResponse
    {
        public int TransactionID { get; set; }

        public int ProductID { get; set; }

        public int OrderID { get; set; }

        public int ProductAmount { get; set; } = 0;

        public double ProductPrice { get; set; } = 0;

        public int? DiscountID { get; set; }
        
        public DateTime Created_At { get; set; }
        
        public DateTime Modified_At { get; set; }
    }
}
