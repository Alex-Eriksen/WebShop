namespace WebShop_API.DTOs.Photo
{
    public class PhotoRequest
    {
        [Required(ErrorMessage = "PhotoID is required")]
        public int PhotoID { get; set; }

        [Required(ErrorMessage = "ProductID is required")]
        public int ProductID { get; set; }

        public string ImageName { get; set; }

        public string ImageLocation { get; set; }
    }
}
