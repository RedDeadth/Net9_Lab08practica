using Lab08.Models;

namespace Lab08.Repositories;

public interface IOrderDetailRepository : IRepository<Orderdetail>
{
    Task<IEnumerable<Orderdetail>> GetOrderDetailsByOrderIdAsync(int orderId);
    Task<IEnumerable<Orderdetail>> GetOrderDetailsByProductIdAsync(int productId);
}