using Lab08.Data;
using Lab08.DTOs;
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
        public async Task<int> GetTotalProductQuantityByOrderAsync(int orderId)
        {
            return await _context.Orderdetails
                .Where(od => od.Orderid == orderId)
                .Select(od => od.Quantity)
                .SumAsync();
        }

        // Ejercicio 6: Obtener todos los pedidos después de una fecha específica
        public async Task<IEnumerable<Order>> GetOrdersAfterDateAsync(DateTime date)
        {
            return await _context.Orders
                .Where(o => o.Orderdate > date)
                .Include(o => o.Client)
                .ToListAsync();
        }

        // Ejercicio 9: Obtener el cliente con mayor número de pedidos
        public async Task<(int ClientId, int OrderCount)> GetClientWithMostOrdersAsync()
        {
            var result = await _context.Orders
                .GroupBy(o => o.Clientid)
                .Select(g => new 
                { 
                    ClientId = g.Key, 
                    OrderCount = g.Count() 
                })
                .OrderByDescending(x => x.OrderCount)
                .FirstOrDefaultAsync();

            return result != null ? (result.ClientId, result.OrderCount) : (0, 0);
        }

        // Ejercicio 10: Obtener todos los pedidos y sus detalles
        public async Task<IEnumerable<OrderDetailDto>> GetAllOrdersWithDetailsAsync()
        {
            return await _context.Orderdetails
                .Include(od => od.Product)
                .Select(od => new OrderDetailDto
                {
                    OrderId = od.Orderid,
                    ProductName = od.Product.Name,
                    Quantity = od.Quantity
                })
                .ToListAsync();
        }

        // Ejercicio 11: Obtener productos vendidos a un cliente específico
        public async Task<IEnumerable<string>> GetProductsSoldToClientAsync(int clientId)
        {
            return await _context.Orders
                .Where(o => o.Clientid == clientId)
                .SelectMany(o => o.Orderdetails)
                .Select(od => od.Product.Name)
                .Distinct()
                .ToListAsync();
        }
    }
}