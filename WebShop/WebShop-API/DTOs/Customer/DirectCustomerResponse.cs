namespace WebShop_API.DTOs.Customer
{
    public class DirectCustomerResponse
    {
        public int CustomerID { get; set; }

        public StaticAccountResponse Account { get; set; } = null!;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public StaticCountryResponse Country { get; set; } = null!;

        public int ZipCode { get; set; } = 0;

        public string Gender { get; set; } = string.Empty;

        public DateTime Created_At { get; set; }

        public List<StaticPaymentResponse> Payments { get; set; } = new List<StaticPaymentResponse>();

        public List<StaticOrderResponse> Orders { get; set; } = new List<StaticOrderResponse>();
    }
}
