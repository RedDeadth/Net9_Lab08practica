using Lab08.Models;

namespace Lab08.Repositories;

public interface IOrderRepository : IRepository<Order>
{
    Task<IEnumerable<Order>> GetOrdersByClientIdAsync(int clientId);
    Task<Order?> GetOrderWithDetailsAsync(int orderId);
    Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);
}