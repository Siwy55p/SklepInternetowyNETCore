using Microsoft.EntityFrameworkCore;
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
            Slider slider = _context.Sliders.Where(x=>x.ImageSliderID == id).Include(x => x.ObrazkiDostepneWSliderze).FirstOrDefault();
            _context.Sliders.Remove(slider);
            _context.SaveChanges();

        }

        public async Task EditSliderAsync(Slider slider)
        {
            _context.Update(slider);
            _context.SaveChangesAsync();
        }
        public async Task<Slider> GetAsync(int id)
        {
            Slider slider = await _context.Sliders.Where(x => x.ImageSliderID == id).Include(x=>x.ObrazkiDostepneWSliderze).FirstOrDefaultAsync();
            return slider;
        }



    }
}
