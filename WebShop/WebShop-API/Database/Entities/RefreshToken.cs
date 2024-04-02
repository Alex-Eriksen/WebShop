namespace WebShop_API.Database.Entities
{
    public class RefreshToken
    {
        [Key]
        public int RefreshTokenID { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string Token { get; set; } = string.Empty;

        [Column( TypeName = "datetime2" )]
        public DateTime Expires_At { get; set; }

        [Column( TypeName = "datetime2" )]
        public DateTime Created_At { get; set; }

        [Column( TypeName = "nvarchar(16)" )]
        public string CreatedByIp { get; set; } = string.Empty;

        [Column( TypeName = "datetime2" )]
        public DateTime? Revoked_At { get; set; }

        [Column( TypeName = "nvarchar(16)" )]
        public string RevokedByIp { get; set; } = string.Empty;

        [Column( TypeName = "nvarchar(255)" )]
        public string ReplacedByToken { get; set; } = string.Empty;

        public bool IsExpired => DateTime.UtcNow >= Expires_At;
        public bool IsActive => Revoked_At == null && !IsExpired;
    }
}
