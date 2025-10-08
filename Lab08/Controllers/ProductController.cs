using Lab08.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lab08.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet("by-min-price/{minPrice}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetProductsByMinimumPrice(decimal minPrice)
        {
            if (minPrice < 0)
            {
                return BadRequest(new { message = "El precio no puede ser negativo" });
            }

            var products = await _productService.GetProductsByMinimumPriceAsync(minPrice);
            
            return Ok(new 
            { 
                message = $"Productos encontrados con precio mayor a {minPrice:C}",
                count = products.Count(),
                data = products 
            });
        }
        /// <summary>
        /// Ejercicio 5: Obtener el producto más caro
        /// </summary>
        [HttpGet("linq/most-expensive")]
        public async Task<ActionResult> GetMostExpensiveProduct()
        {
            try
            {
                var product = await _productService.GetMostExpensiveProductAsync();
        
                if (product == null)
                    return NotFound(new { message = "No hay productos en la base de datos" });

                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener el producto más caro", error = ex.Message });
            }
        }
        /// <summary>
        /// Ejercicio 7: Obtener el promedio de precio de los productos
        /// </summary>
        [HttpGet("linq/average-price")]
        public async Task<ActionResult> GetAveragePrice()
        {
            try
            {
                var averagePrice = await _productService.GetAveragePriceAsync();
                return Ok(new 
                { 
                    AveragePrice = averagePrice,
                    FormattedPrice = $"${averagePrice:F2}"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al calcular el promedio", error = ex.Message });
            }
        }
        /// <summary>
        /// Ejercicio 8: Obtener productos sin descripción
        /// </summary>
        [HttpGet("linq/without-description")]
        public async Task<ActionResult> GetProductsWithoutDescription()
        {
            try
            {
                var products = await _productService.GetProductsWithoutDescriptionAsync();
                return Ok(new 
                { 
                    Count = products.Count(),
                    Products = products 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener los productos", error = ex.Message });
            }
        }

        /// <summary>
        /// Ejercicio 12: Obtener todos los clientes que han comprado un producto específico
        /// </summary>
        /// <param name="productId">ID del producto</param>
        [HttpGet("{productId}/linq/clients")]
        public async Task<ActionResult> GetClientsWhoBoughtProduct(int productId)
        {
            try
            {
                var result = await _productService.GetClientsWhoBoughtProductAsync(productId);
        
                if (result == null)
                    return NotFound(new { message = "Producto no encontrado" });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener los clientes", error = ex.Message });
            }
        }
    }
    
}