namespace WebShop_API.Database.Entities
{
    public class Photo
    {
        [Key]
        public int PhotoID { get; set; }

        [ForeignKey("Product.ProductID")]
        public int ProductID { get; set; }
        public Product Product { get; set; }

        [Column( TypeName = "nvarchar(MAX)" )]
        public string ImageName { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar(MAX)")]
        public string ImageLocation { get; set; } = string.Empty;

        [Column(TypeName = "datetime2")]
        public DateTime Created_At { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime Modified_At { get; set; }
    }
}
