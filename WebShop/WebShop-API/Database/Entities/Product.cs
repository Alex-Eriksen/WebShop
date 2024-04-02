namespace WebShop_API.Database.Entities
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string ProductName { get; set; } = string.Empty;

        [Column( TypeName = "float" )]
        public double ProductPrice { get; set; }

        [Column( TypeName = "int" )]
        public int ProductQuantity { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string ProductDescription { get; set; } = string.Empty;

        [ForeignKey("Manufacturer.ManufacturerID")]
        public int ManufacturerID { get; set; }
        public Manufacturer Manufacturer { get; set; }

        [ForeignKey("Category.CategoryID")]
        public int CategoryID { get; set; }
        public Category Category { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime ReleaseDate { get; set; }

        [ForeignKey("ProductType.ProductTypeID")]
        public int ProductTypeID { get; set; }
        public ProductType ProductType { get; set; }

        [ForeignKey("Discount.DiscountID")]
        public int? DiscountID { get; set; }
        public Discount? Discount { get; set; }

        [Column( TypeName = "datetime2" )]
        public DateTime Created_At { get; set; }

        [Column( TypeName = "datetime2" )]
        public DateTime Modified_At { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
        public ICollection<Photo> Photos { get; set; }

        [FromForm]
        [NotMapped]
        public IFormFileCollection Files { get; set; }
    }
}
