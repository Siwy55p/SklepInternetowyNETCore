using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using partner_aluro.Data;
using partner_aluro.ViewModels;

namespace partner_aluro.ViewComponents
{
    public class UserNewStatusViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public UserNewStatusViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            UserNewStatusModel model = new UserNewStatusModel()
            {
                UserNewStatus = await _context.Users.Where(x => x.Nowy == true).CountAsync()
            };
            return View(model);
        }
    }
}
