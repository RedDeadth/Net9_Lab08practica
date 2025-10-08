using Lab08.Models;
using Lab08.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lab08.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderDetailsController : ControllerBase
    {
        private readonly IOrderDetailService _orderDetailService;

        public OrderDetailsController(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }

        /// <summary>
        /// Obtener todos los detalles de órdenes
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Orderdetail>>> GetAllOrderDetails()
        {
            var orderDetails = await _orderDetailService.GetAllOrderDetailsAsync();
            return Ok(orderDetails);
        }

        /// <summary>
        /// Obtener un detalle de orden por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Orderdetail>> GetOrderDetailById(int id)
        {
            var orderDetail = await _orderDetailService.GetOrderDetailByIdAsync(id);
            if (orderDetail == null)
                return NotFound(new { message = "Detalle de orden no encontrado" });

            return Ok(orderDetail);
        }

        /// <summary>
        /// Crear un nuevo detalle de orden
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Orderdetail>> CreateOrderDetail(Orderdetail orderDetail)
        {
            var createdOrderDetail = await _orderDetailService.CreateOrderDetailAsync(orderDetail);
            return CreatedAtAction(nameof(GetOrderDetailById), new { id = createdOrderDetail.Orderdetailid }, createdOrderDetail);
        }

        /// <summary>
        /// Actualizar un detalle de orden existente
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<Orderdetail>> UpdateOrderDetail(int id, Orderdetail orderDetail)
        {
            var updatedOrderDetail = await _orderDetailService.UpdateOrderDetailAsync(id, orderDetail);
            if (updatedOrderDetail == null)
                return NotFound(new { message = "Detalle de orden no encontrado" });

            return Ok(updatedOrderDetail);
        }

        /// <summary>
        /// Eliminar un detalle de orden
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrderDetail(int id)
        {
            var result = await _orderDetailService.DeleteOrderDetailAsync(id);
            if (!result)
                return NotFound(new { message = "Detalle de orden no encontrado" });

            return NoContent();
        }

        // ==================== EJERCICIOS LINQ ====================

        /// <summary>
        /// Ejercicio 10: Obtener todos los pedidos con sus detalles (nombre del producto y cantidad)
        /// </summary>
        [HttpGet("linq/all-with-products")]
        public async Task<ActionResult> GetAllOrdersWithDetails()
        {
            try
            {
                var orderDetails = await _orderDetailService.GetAllOrdersWithDetailsAsync();
                return Ok(new 
                { 
                    Count = orderDetails.Count(),
                    OrderDetails = orderDetails 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener los detalles", error = ex.Message });
            }
        }
    }
}