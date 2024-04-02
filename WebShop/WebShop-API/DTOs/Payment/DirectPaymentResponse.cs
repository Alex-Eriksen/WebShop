namespace WebShop_API.DTOs.Payment
{
    public class DirectPaymentResponse
    {
        public int PaymentID { get; set; } = 0;

        public StaticCustomerResponse Customer { get; set; } = null!;

        public string PaymentType { get; set; } = string.Empty;

        public string CardNumber { get; set; } = string.Empty;

        public string Provider { get; set; } = string.Empty;

        public DateTime Expiry { get; set; }

        public DateTime Created_At { get; set; }
    }
}
