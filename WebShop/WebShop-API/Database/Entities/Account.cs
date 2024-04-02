namespace WebShop_API.Database.Entities
{
    /// <summary>
    /// Properties in Account: AccountID, Username, Password, Email, Created_At, Modified_At, Role, Customer and RefreshTokens
    /// </summary>
    public class Account
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        [Key]
        public int AccountID { get; set; }

        [Column( TypeName = "nvarchar(32)")]
        public string Username { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar(255)")]
        public string Password { get; set; } = string.Empty;

        [Column( TypeName = "nvarchar(64)")]
        public string Email { get; set; } = string.Empty;

        [Column( TypeName = "datetime2" )]
        public DateTime Created_At { get; set; }

        [Column( TypeName = "datetime2" )]
        public DateTime Modified_At { get; set; }

        [Column( TypeName = "nvarchar(32)" )]
        public string Role { get; set; } = string.Empty;

        /// <summary>
        /// Navigation reference
        /// </summary>
        public Customer Customer { get; set; }
        /// <summary>
        /// Navigation reference
        /// </summary>
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
