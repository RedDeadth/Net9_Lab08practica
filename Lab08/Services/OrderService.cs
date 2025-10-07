using Lab08.DTOs;
using Lab08.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Lab08.Services
{
    public interface IOrderService
    {
        Task<OrderDTOs.OrderDetailSummaryDto?> GetOrderProductDetailsAsync(int orderId);
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
        
    }
}