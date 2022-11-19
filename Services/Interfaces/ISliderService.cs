using partner_aluro.Models;

namespace partner_aluro.Services.Interfaces
{
    public interface ISliderService
    {
        void AddSlider(Slider slider);

        Task EditSliderAsync(Slider slider);


        void DeleteSlider(int id);

        Task<Slider> GetAsync(int id);
    }
}
