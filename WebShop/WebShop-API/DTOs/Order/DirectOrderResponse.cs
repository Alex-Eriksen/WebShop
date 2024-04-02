namespace WebShop_API.DTOs.Order
{
    public class DirectOrderResponse
    {
        public int OrderID { get; set; } = 0;

        public StaticCustomerResponse Customer { get; set; } = new();

        public double OrderTotal { get; set; } = 0;

        public List<StaticTransactionResponse> Transactions { get; set; } = new List<StaticTransactionResponse>();
    }
}
