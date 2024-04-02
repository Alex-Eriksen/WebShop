namespace WebShop_API.DTOs.Authentication
{
    public class AuthenticationResponse
    {
        [JsonIgnore]
        public string RefreshToken { get; set; } = string.Empty;

        public string AccessToken { get; set; } = string.Empty;

        public AuthenticationResponse(string refreshToken, string accessToken )
        {
            RefreshToken = refreshToken;
            AccessToken = accessToken;
        }
    }
}
