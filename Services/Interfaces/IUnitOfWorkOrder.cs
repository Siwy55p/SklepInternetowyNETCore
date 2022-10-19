namespace partner_aluro.Services.Interfaces
{
    public interface IUnitOfWorkOrder
    {
        IOrderService OrderService { get; }
    }
}
