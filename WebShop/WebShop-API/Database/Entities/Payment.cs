namespace WebShop_API.Database.Entities
{
    public class Payment
    {
        [Key]
        public int PaymentID { get; set; }

        [ForeignKey("Customer.CustomerID")]
        public int CustomerID { get; set; }
        public Customer Customer { get; set; }

        [Column(TypeName = "nvarchar(32)")]
        public string PaymentType { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar(255)")]
        public string CardNumber { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar(32)")]
        public string Provider { get; set; } = string.Empty;

        [Column(TypeName = "datetime2")]
        public DateTime Expiry { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime Created_At { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime Modified_At { get; set; }
    }
}
