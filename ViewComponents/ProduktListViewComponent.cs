using Microsoft.AspNetCore.Mvc;
using partner_aluro.Services.Interfaces;

namespace partner_aluro.ViewComponents
{
    public class ProduktListViewComponent : ViewComponent
    {
        private readonly IProductService _productService;
        public ProduktListViewComponent(IProductService productService)
        {
            _productService = productService;
        }

        public IViewComponentResult Invoke()
        {
            return View(_productService.GetProductList());
        }
    }
}
