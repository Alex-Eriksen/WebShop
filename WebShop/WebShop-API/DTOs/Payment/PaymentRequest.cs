namespace WebShop_API.DTOs.Payment
{
    public class PaymentRequest
    {
        [Required(ErrorMessage = "CustomerID is required")]
        public int CustomerID { get; set; }

        [Required(ErrorMessage = "PaymentType is required")]
        public string PaymentType { get; set; } = string.Empty;

        [Required(ErrorMessage = "CardNumber is required")]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "CardNumber must be 16 characters")]
        public string CardNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Provider is required")]
        public string Provider { get; set; } = string.Empty;

        [Required(ErrorMessage = "Expiration Date is required")]
        public DateTime Expiry { get; set; }
    }
}
