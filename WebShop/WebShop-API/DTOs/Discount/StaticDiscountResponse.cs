namespace WebShop_API.DTOs.Discount
{
    public class StaticDiscountResponse
    {
        public int DiscountID { get; set; } = 0;
        
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int DiscountPercent { get; set; } = 0;

        public DateTime Created_At { get; set; }

        public DateTime Modified_At { get; set; }

        public DateTime Revoked_At { get; set; }
    }
}
