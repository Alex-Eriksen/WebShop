namespace WebShop_API.Database.Entities
{
    public class ProductType
    {
        [Key]
        public int ProductTypeID { get; set; }

        [Column( TypeName = "nvarchar(32)" )]
        public string ProductTypeName { get; set; }

        [Column( TypeName = "datetime2" )]
        public DateTime Created_At { get; set; }

        [Column( TypeName = "datetime2" )]
        public DateTime Modified_At { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
