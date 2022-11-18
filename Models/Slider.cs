
namespace partner_aluro.Models
{

    public class Slider
    {

        public int ImageSliderID { get; set; }

        public virtual ImageModel? slider_Image { get; set; } = new ImageModel();
        //public IFormFile? FrontImage { get; set; }

        public virtual List<ImageModel>? Slider_Images { get; set; } = new List<ImageModel>();


    }
}
