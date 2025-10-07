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
    }
}