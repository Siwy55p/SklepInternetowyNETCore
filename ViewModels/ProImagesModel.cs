using partner_aluro.Models;

namespace partner_aluro.ViewModels
{
    public class ProImagesModel
    {
        public List<IFormFile> Images { get; set; }
        public Product Product { get; set; }

        public List<ImageModel> product_Images { get; set; }

    }
}
