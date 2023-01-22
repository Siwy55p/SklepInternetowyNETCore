using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using partner_aluro.Data;
using partner_aluro.Models;
using partner_aluro.Services;
using partner_aluro.Services.Interfaces;
using partner_aluro.ViewModels;

namespace partner_aluro.ViewComponents
{
    public class OrderStatusViewComponent : ViewComponent
    {
        private readonly IOrderStatusService _orderStatusService;

        public OrderStatusViewComponent(IOrderStatusService orderStatusService)
        {
            _orderStatusService = orderStatusService;
        }
        public IViewComponentResult Invoke()
        {
            OrderStatusModel model = new OrderStatusModel()
            {
                OrderStatusNew = _orderStatusService.CountZamowien()
            };
            return View(model);
        }
    }
}
