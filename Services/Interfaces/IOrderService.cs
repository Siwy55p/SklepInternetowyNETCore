using partner_aluro.Models;

namespace partner_aluro.Services.Interfaces
{
    public interface IOrderService
    {
        Task<List<OrderItem>> ListAsync(int id);
        Task<List<OrderItem>> ListAsync();
        OrderItem GetItem(int id);

        Task<List<Order>> ListOrdersAll();

        List<Order> ListOrdersUser(string UserID);

        int CountZamowien();
        void Add(Order order);
        Task<Order> GetOrder(int id);

        Adress1rozliczeniowy GetUserAdress1(string UserID);
        Adress2dostawy GetUserAdress2(string UserID);

        Order Update(Order order);

    }
}
