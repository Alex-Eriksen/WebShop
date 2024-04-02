namespace WebShop_API.DTOs.Photo
{
    public class StaticPhotoResponse
    {
        public int PhotoID { get; set; } = 0;

        public int ProductID { get; set; } = 0;

        public string ImageName { get; set; } = string.Empty;

        public string ImageLocation { get; set; } = string.Empty;
    }
}
