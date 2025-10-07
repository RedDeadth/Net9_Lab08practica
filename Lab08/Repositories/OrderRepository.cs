using Lab08.Data;
using Lab08.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab08.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(LINQExampleContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Order>> GetOrdersByClientIdAsync(int clientId)
        {
            return await _dbSet
                .Where(o => o.Clientid == clientId)
                .Include(o => o.Orderdetails)
                .ToListAsync();
        }

        public async Task<Order?> GetOrderWithDetailsAsync(int orderId)
        {
            return await _dbSet
                .Include(o => o.Orderdetails)
                .ThenInclude(od => od.Product)
                .Include(o => o.Client)
                .FirstOrDefaultAsync(o => o.Orderid == orderId);
        }

        public async Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Where(o => o.Orderdate >= startDate && o.Orderdate <= endDate)
                .Include(o => o.Client)
                .ToListAsync();
        }
    }
}