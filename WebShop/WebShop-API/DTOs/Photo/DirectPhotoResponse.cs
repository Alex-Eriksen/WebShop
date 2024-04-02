using WebShop_API.DTOs.Product;

namespace WebShop_API.DTOs.Photo
{
    public class DirectPhotoResponse
    {
        public int PhotoID { get; set; } = 0;

        public StaticProductResponse Product { get; set; } = null!;

        public string ImageName { get; set; } = string.Empty;

        public string ImageLocation { get; set; } = string.Empty;
    }
}
