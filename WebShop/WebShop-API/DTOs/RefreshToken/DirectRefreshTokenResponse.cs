namespace WebShop_API.DTOs.RefreshToken
{
    public class DirectRefreshTokenResponse
    {
        public string Token { get; set; } = string.Empty;

        public DateTime Expires_At { get; set; }
    }
}
