using partner_aluro.Models;

namespace partner_aluro.Services.Interfaces
{
    public interface ISliderService
    {
        void AddSlider(Slider slider);

        void EditSlider(int id);

        void DeleteSlider(int id);

        Slider Get(int id);
    }
}
