namespace WebShop_API.Database.Entities
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        [ForeignKey("Customer.CustomerID")]
        public int CustomerID { get; set; }
        public Customer Customer { get; set; }

        [Column(TypeName = "float")]
        public float OrderTotal { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime Created_At { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime Modified_At { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
    }
}
