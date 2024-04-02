namespace WebShop_API.Database.Entities
{
    /// <summary>
    /// Properties in Customer: CustomerID, AccountID, Account, FirstName, LastName, 
    /// PhoneNumber, CountryID, Country, ZipCode, Gender, Created_At, Payments and Orders
    /// </summary>
    public class Customer
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        [Key]
        public int CustomerID { get; set; }

        /// <summary>
        /// Foreign Key
        /// </summary>
        [ForeignKey("Account.AccountID")]
        public int AccountID { get; set; }
        public Account Account { get; set; }

        [Column(TypeName = "nvarchar(32)")]
        public string FirstName { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar(32)")]
        public string LastName { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar(32)")]
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// Foreign Key
        /// </summary>
        [ForeignKey("Country.CountryID")]
        public int CountryID { get; set; }
        public Country Country { get; set; }

        [Column(TypeName = "int")]
        public int ZipCode { get; set; } = 0;

        [Column(TypeName = "nvarchar(32)")]
        public string Gender { get; set; } = string.Empty;

        /// <summary>
        /// Navigation reference
        /// </summary>
        [Column(TypeName = "datetime2")]
        public DateTime Created_At { get; set; }

        /// <summary>
        /// Navigation reference
        /// </summary>
        [Column(TypeName = "datetime2")]
        public DateTime Modified_At { get; set; }

        /// <summary>
        /// Navigation reference
        /// </summary>
        public ICollection<Payment> Payments { get; set; }
        /// <summary>
        /// Navigation reference
        /// </summary>
        public ICollection<Order> Orders { get; set; }
    }
}
