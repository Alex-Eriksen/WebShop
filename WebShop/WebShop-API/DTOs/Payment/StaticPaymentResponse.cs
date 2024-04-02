namespace WebShop_API.DTOs.Payment
{
    public class StaticPaymentResponse
    {
        public int PaymentID { get; set; } = 0;

        public int CustomerID { get; set; } = 0;

        public string PaymentType { get; set; } = string.Empty;

        public string CardNumber { get; set; } = string.Empty;

        public string Provider { get; set; } = string.Empty; 

        public DateTime Expiry { get; set; }

        public DateTime Created_At { get; set; }
    }
}
