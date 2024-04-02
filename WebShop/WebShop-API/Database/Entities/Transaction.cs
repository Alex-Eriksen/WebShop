namespace WebShop_API.Database.Entities
{
    public class Transaction
    {
        [Key]
        public int TransactionID { get; set; }

        [ForeignKey("Product.ProductID")]
        public int ProductID { get; set; }
        public Product Product { get; set; }

        [ForeignKey("Order.OrderID")]
        public int OrderID { get; set; }
        public Order Order { get; set; }

        [Column( TypeName = "int" )]
        public int ProductAmount { get; set; }

        [Column( TypeName = "float" )]
        public double ProductPrice { get; set; }

        [ForeignKey("Discount.DiscountID")]
        public int? DiscountID { get; set; }
        public Discount? Discount { get; set; }

        [Column( TypeName = "datetime2" )]
        public DateTime Created_At { get; set; }

        [Column( TypeName = "datetime2" )]
        public DateTime Modified_At { get; set; }
    }
}
