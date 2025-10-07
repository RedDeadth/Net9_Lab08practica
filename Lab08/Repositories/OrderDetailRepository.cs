using Lab08.Data;
using Lab08.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab08.Repositories
{
    public class OrderDetailRepository : Repository<Orderdetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(LINQExampleContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Orderdetail>> GetOrderDetailsByOrderIdAsync(int orderId)
        {
            return await _dbSet
                .Where(od => od.Orderid == orderId)
                .Include(od => od.Product)
                .ToListAsync();
        }

        public async Task<IEnumerable<Orderdetail>> GetOrderDetailsByProductIdAsync(int productId)
        {
            return await _dbSet
                .Where(od => od.Productid == productId)
                .Include(od => od.Order)
                .ThenInclude(o => o.Client)
                .ToListAsync();
        }
    }
}