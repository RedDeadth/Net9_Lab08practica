using Lab08.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lab08.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("{orderId}/products")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrderProductDetails(int orderId)
        {
            var orderDetails = await _orderService.GetOrderProductDetailsAsync(orderId);

            if (orderDetails == null)
            {
                return NotFound(new 
                { 
                    message = $"No se encontró la orden con ID {orderId}" 
                });
            }

            return Ok(new
            {
                message = $"Detalle de la orden #{orderId}",
                data = orderDetails
            });
        }

        [HttpGet("{orderId}/products/simple")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrderProductsSimple(int orderId)
        {
            var orderDetails = await _orderService.GetOrderProductDetailsAsync(orderId);

            if (orderDetails == null)
            {
                return NotFound(new 
                { 
                    message = $"No se encontró la orden con ID {orderId}" 
                });
            }

            // Proyección simplificada usando LINQ
            var simpleProducts = orderDetails.Products
                .Select(p => new 
                {
                    ProductName = p.ProductName,
                    Quantity = p.Quantity
                })
                .ToList();

            return Ok(new
            {
                message = $"Productos en la orden #{orderId}",
                orderId = orderDetails.OrderId,
                orderDate = orderDetails.OrderDate,
                clientName = orderDetails.ClientName,
                totalItems = orderDetails.TotalItems,
                products = simpleProducts
            });
        }
    }
}