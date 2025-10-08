using Lab08.DTOs;
using Lab08.Models;
using Lab08.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Lab08.Services
{
    public interface IOrderService
    {
        Task<OrderDTOs.OrderDetailSummaryDto?> GetOrderProductDetailsAsync(int orderId);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order?> GetOrderByIdAsync(int id);
        Task<Order> CreateOrderAsync(Order order);
        Task<Order?> UpdateOrderAsync(int id, Order order);
        Task<bool> DeleteOrderAsync(int id);
        
        // Ejercicio 4: Cantidad total de productos por orden
        Task<OrderDTOs.OrderQuantityDto?> GetTotalProductQuantityByOrderAsync(int orderId);
        
        // Ejercicio 6: Órdenes después de una fecha
        Task<IEnumerable<Order>> GetOrdersAfterDateAsync(DateTime date);
    }
    

    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Ejercicio 3: Obtener el detalle de productos en una orden usando LINQ
        public async Task<OrderDTOs.OrderDetailSummaryDto?> GetOrderProductDetailsAsync(int orderId)
        {
            // Obtener la orden con sus detalles y productos usando el repositorio
            var order = await _unitOfWork.Orders.GetOrderWithDetailsAsync(orderId);

            if (order == null)
            {
                return null;
            }

            // Usar LINQ para proyectar los detalles de la orden
            var productDetails = order.Orderdetails
                .Where(od => od.Orderid == orderId) // Filtrar por OrderId específico
                .Select(od => new OrderDTOs.OrderProductDetailDto
                {
                    OrderDetailId = od.Orderdetailid,
                    ProductId = od.Productid,
                    ProductName = od.Product.Name,
                    ProductDescription = od.Product.Description,
                    ProductPrice = od.Product.Price,
                    Quantity = od.Quantity,
                    Subtotal = od.Product.Price * od.Quantity
                })
                .ToList(); // Convertir a lista

            // Calcular totales usando LINQ
            var totalAmount = productDetails.Sum(p => p.Subtotal);
            var totalItems = productDetails.Sum(p => p.Quantity);

            // Crear el DTO de resumen
            var orderSummary = new OrderDTOs.OrderDetailSummaryDto
            {
                OrderId = order.Orderid,
                OrderDate = order.Orderdate,
                ClientName = order.Client.Name,
                Products = productDetails,
                TotalAmount = totalAmount,
                TotalItems = totalItems
            };

            return orderSummary;
        }
        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _unitOfWork.Orders.GetAllAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _unitOfWork.Orders.GetByIdAsync(id);
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();
            return order;
        }

        public async Task<Order?> UpdateOrderAsync(int id, Order order)
        {
            var existingOrder = await _unitOfWork.Orders.GetByIdAsync(id);
            if (existingOrder == null)
                return null;

            existingOrder.Orderdate = order.Orderdate;
            existingOrder.Clientid = order.Clientid;

            _unitOfWork.Orders.Update(existingOrder);
            await _unitOfWork.SaveChangesAsync();
            return existingOrder;
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order == null)
                return false;

            _unitOfWork.Orders.Remove(order);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        // Ejercicio 4: Obtener cantidad total de productos por orden
        public async Task<OrderDTOs.OrderQuantityDto?> GetTotalProductQuantityByOrderAsync(int orderId)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
            if (order == null)
                return null;

            var totalQuantity = await _unitOfWork.Orders.GetTotalProductQuantityByOrderAsync(orderId);
            
            return new OrderDTOs.OrderQuantityDto
            {
                OrderId = orderId,
                OrderDate = order.Orderdate,
                TotalQuantity = totalQuantity
            };
        }

        // Ejercicio 6: Obtener órdenes después de una fecha
        public async Task<IEnumerable<Order>> GetOrdersAfterDateAsync(DateTime date)
        {
            return await _unitOfWork.Orders.GetOrdersAfterDateAsync(date);
        }
    }
}