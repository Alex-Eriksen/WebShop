namespace WebShop_API.Database.Entities
{
    /// <summary>
    /// Properties in Category: CategoryID, CategoryName, Created_At, Modified_At and Products
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        [Key]
        public int CategoryID { get; set; }

        [Column(TypeName = "nvarchar(32)")]
        public string CategoryName { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime Created_At { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime Modified_At { get; set; }

        /// <summary>
        /// Navigation reference
        /// </summary>
        public ICollection<Product> Products { get; set; }
    }
}
