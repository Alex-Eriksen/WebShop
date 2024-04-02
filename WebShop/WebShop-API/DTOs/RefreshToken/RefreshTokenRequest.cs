namespace WebShop_API.DTOs.RefreshToken
{
    public class RefreshTokenRequest
    {
        public int RefreshTokenID { get; set; } = 0;

        public string Token { get; set; } = string.Empty;

        public DateTime Expires_At { get; set; }

        public DateTime Created_At { get; set; }

        public string CreatedByIp { get; set; } = string.Empty;

        public DateTime Revoked_At { get; set; }

        public string RevokeddByIp { get; set; } = string.Empty;

        public string ReplacdedByToken { get; set; } = string.Empty;
    }
}
