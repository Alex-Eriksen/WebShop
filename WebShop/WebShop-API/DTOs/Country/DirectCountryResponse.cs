namespace WebShop_API.DTOs.Country
{
    public class DirectCountryResponse
    {
        public int CountryID { get; set; }

        public string CountryName { get; set; } = string.Empty;

        public List<StaticCustomerResponse> Customers { get; set; } = new List<StaticCustomerResponse>();
    }
}
