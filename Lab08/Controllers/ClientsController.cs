using Lab08.Models;
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
        /// <summary>
        /// Obtener todos los clientes
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetAllClients()
        {
            var clients = await _clientService.GetAllClientsAsync();
            return Ok(clients);
        }

        /// <summary>
        /// Obtener un cliente por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClientById(int id)
        {
            var client = await _clientService.GetClientByIdAsync(id);
            if (client == null)
                return NotFound(new { message = "Cliente no encontrado" });

            return Ok(client);
        }

        /// <summary>
        /// Crear un nuevo cliente
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Client>> CreateClient(Client client)
        {
            var createdClient = await _clientService.CreateClientAsync(client);
            return CreatedAtAction(nameof(GetClientById), new { id = createdClient.Clientid }, createdClient);
        }

        /// <summary>
        /// Actualizar un cliente existente
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<Client>> UpdateClient(int id, Client client)
        {
            var updatedClient = await _clientService.UpdateClientAsync(id, client);
            if (updatedClient == null)
                return NotFound(new { message = "Cliente no encontrado" });

            return Ok(updatedClient);
        }

        /// <summary>
        /// Eliminar un cliente
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteClient(int id)
        {
            var result = await _clientService.DeleteClientAsync(id);
            if (!result)
                return NotFound(new { message = "Cliente no encontrado" });

            return NoContent();
        }

        // ==================== EJERCICIOS LINQ ====================

        /// <summary>
        /// Ejercicio 9: Obtener el cliente con mayor número de pedidos
        /// </summary>
        [HttpGet("linq/most-orders")]
        public async Task<ActionResult> GetClientWithMostOrders()
        {
            try
            {
                var result = await _clientService.GetClientWithMostOrdersAsync();
                
                if (result == null)
                    return NotFound(new { message = "No hay pedidos en la base de datos" });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener el cliente", error = ex.Message });
            }
        }

        /// <summary>
        /// Ejercicio 11: Obtener todos los productos vendidos a un cliente específico
        /// </summary>
        [HttpGet("{clientId}/linq/products")]
        public async Task<ActionResult> GetProductsSoldToClient(int clientId)
        {
            try
            {
                var result = await _clientService.GetProductsSoldToClientAsync(clientId);
                
                if (result == null)
                    return NotFound(new { message = "Cliente no encontrado" });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener los productos", error = ex.Message });
            }
        }
    }
}