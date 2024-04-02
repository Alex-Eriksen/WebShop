namespace WebShop_API.DTOs.Category
{
    public class StaticCategoryResponse
    {
        public int CategoryID { get; set; }
        
        public string CategoryName { get; set; } = string.Empty;

        public int ProductCount { get; set; } = 0;
    }
}
