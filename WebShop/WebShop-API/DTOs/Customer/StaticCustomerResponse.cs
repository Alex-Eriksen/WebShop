namespace WebShop_API.DTOs.Customer
{
    public class StaticCustomerResponse
    {
        public int CustomerID { get; set; }

        public int AccountID { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public int CountryID { get; set; }

        public int ZipCode { get; set; } = 0;

        public string Gender { get; set; } = string.Empty;

        public DateTime Created_At { get; set; }
    }
}
