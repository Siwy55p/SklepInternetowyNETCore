using partner_aluro.Data;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;





namespace partner_aluro.Services
{
    public class SliderService : ISliderService
    {

        private readonly ApplicationDbContext _context;

        public SliderService(ApplicationDbContext context)
        {
            _context = context;
        }   

        public void AddSlider(Slider slider)
        {
            _context.Sliders.Add(slider);
            _context.SaveChanges();
        }

        public void DeleteSlider(int id)
        {
            Slider slider = _context.Sliders.Where(x=>x.ImageSliderID == id).FirstOrDefault();
            _context.Sliders.Remove(slider);
            _context.SaveChanges();

        }

        public void EditSlider(int id)
        {
            Slider slider = _context.Sliders.Where(x => x.ImageSliderID == id).FirstOrDefault();
            _context.Update(slider);
            _context.SaveChanges();
        }

        public Slider Get(int id)
        {
            Slider slider = _context.Sliders.Where(x => x.ImageSliderID == id).FirstOrDefault();
            return slider;
        }



    }
}
