using Lab08.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lab08.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
        }

        /// <summary>
        /// Ejercicio 1: Obtener clientes cuyo nombre contiene el texto especificado
        /// </summary>
        /// <param name="name">Texto a buscar en el nombre del cliente (ej: "Juan")</param>
        /// <returns>Lista de clientes que coinciden con el criterio</returns>
        /// <response code="200">Devuelve la lista de clientes encontrados</response>
        /// <response code="400">Si el parámetro name está vacío</response>
        [HttpGet("by-name/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetClientsByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest(new { message = "El parámetro 'name' no puede estar vacío" });
            }

            var clients = await _clientService.GetClientsByNameAsync(name);
            
            return Ok(new 
            { 
                message = $"Clientes encontrados con nombre que contiene '{name}'",
                count = clients.Count(),
                data = clients 
            });
        }
    }
}