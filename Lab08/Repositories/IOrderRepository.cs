using Lab08.DTOs;
using Lab08.Models;

namespace Lab08.Repositories;

public interface IOrderRepository : IRepository<Order>
{
    Task<IEnumerable<Order>> GetOrdersByClientIdAsync(int clientId);
    Task<Order?> GetOrderWithDetailsAsync(int orderId);
    Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);
    
    Task<int> GetTotalProductQuantityByOrderAsync(int orderId);
        
    // Ejercicio 6: Órdenes después de una fecha
    Task<IEnumerable<Order>> GetOrdersAfterDateAsync(DateTime date);
        
    // Ejercicio 9: Cliente con mayor número de pedidos
    Task<(int ClientId, int OrderCount)> GetClientWithMostOrdersAsync();
        
    // Ejercicio 10: Todos los pedidos con detalles
    Task<IEnumerable<OrderDetailDto>> GetAllOrdersWithDetailsAsync();
        
    // Ejercicio 11: Productos vendidos a un cliente específico
    Task<IEnumerable<string>> GetProductsSoldToClientAsync(int clientId);
}