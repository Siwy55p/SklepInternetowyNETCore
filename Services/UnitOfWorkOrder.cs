using partner_aluro.Services.Interfaces;

namespace partner_aluro.Services
{
    public class UnitOfWorkOrder : IUnitOfWorkOrder
    {
        public IOrderService OrderService { get; }

        public UnitOfWorkOrder(IOrderService orderService)
        {
            OrderService = orderService;
        }
    }
}
