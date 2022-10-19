using partner_aluro.Services.Interfaces;

namespace partner_aluro.Services
{
    public class UnitOfWorkOrder : IUnitOfWorkOrder
    {
        public IOrderService OrderService { get; set; }

        public UnitOfWorkOrder(IOrderService OrderService)
        {
            OrderService = OrderService;
        }
    }
}
