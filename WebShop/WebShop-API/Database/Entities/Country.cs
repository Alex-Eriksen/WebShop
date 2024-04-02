namespace WebShop_API.Database.Entities
{
    /// <summary>
    /// Properties in Country: CountryID, CountryName and Customers
    /// </summary>
    public class Country
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        [Key]
        public int CountryID { get; set; }

        [Column(TypeName = "nvarchar(32)")]
        public string CountryName { get; set; } = string.Empty;

        /// <summary>
        /// Navigation reference
        /// </summary>
        public ICollection<Customer> Customers { get; set; }
    }
}
