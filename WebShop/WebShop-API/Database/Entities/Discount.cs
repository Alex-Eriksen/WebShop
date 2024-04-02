using System.Text.Json.Serialization;

namespace WebShop_API.Database.Entities
{
    public class Discount
    {
        [Key]
        public int DiscountID { get; set; }

        [Column(TypeName = "nvarchar(32)")]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string Description { get; set; }

        [Column(TypeName = "int")]
        public int DiscountPercent { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime Created_At { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime Modified_At { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime Revoked_At { get; set; }

        public ICollection<Product> Products { get; set; }

        public bool IsExpired => DateTime.UtcNow >= Revoked_At;

        public ICollection<Transaction> Transactions { get; set; }
    }
}
