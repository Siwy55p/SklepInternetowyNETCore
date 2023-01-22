
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using partner_aluro.Data;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;
using System.Reflection;
using System.Xml.Linq;

namespace partner_aluro.Services
{
    public class OrderStatusService : IOrderStatusService
    {
        private readonly ApplicationDbContext _context;
        public OrderStatusService(ApplicationDbContext context)
        {
            _context = context;
        }
        public int CountZamowien()
        {
            int Count = _context.Orders.Where(x => x.StanZamowienia == StanZamowienia.Nowe).Count();
            return Count;
        }

    }
}
