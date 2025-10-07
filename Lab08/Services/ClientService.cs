using Lab08.DTOs;
using Lab08.Models;
using Lab08.Repositories;

namespace Lab08.Services
{
    public interface IClientService
    {
        Task<IEnumerable<ClientDto>> GetClientsByNameAsync(string name);
        Task<IEnumerable<Client>> GetAllClientsAsync();
        Task<Client?> GetClientByIdAsync(int id);
        Task<Client> CreateClientAsync(Client client);
        Task<Client?> UpdateClientAsync(int id, Client client);
        Task<bool> DeleteClientAsync(int id);
        
        // Ejercicio 9: Cliente con mayor número de pedidos
        Task<ClientWithOrderCountDto?> GetClientWithMostOrdersAsync();
        
        // Ejercicio 11: Productos vendidos a un cliente específico
        Task<ClientProductsDto?> GetProductsSoldToClientAsync(int clientId);
        
        // Ejercicio 12: Clientes que compraron un producto específico
        Task<IEnumerable<string>> GetClientsWhoBoughtProductAsync(int productId);
    }

    public class ClientService : IClientService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClientService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Ejercicio 1: Obtener clientes por nombre usando LINQ
        public async Task<IEnumerable<ClientDto>> GetClientsByNameAsync(string name)
        {
            var clients = await _unitOfWork.Clients.GetClientsByNameAsync(name);

            // Usar LINQ para mapear a DTOs
            var clientDtos = clients
                .Where(c => c.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                .Select(c => new ClientDto
                {
                    Clientid = c.Clientid,
                    Name = c.Name,
                    Email = c.Email
                })
                .ToList();

            return clientDtos;
        }
        public async Task<IEnumerable<Client>> GetAllClientsAsync()
        {
            return await _unitOfWork.Clients.GetAllAsync();
        }

        public async Task<Client?> GetClientByIdAsync(int id)
        {
            return await _unitOfWork.Clients.GetByIdAsync(id);
        }

        public async Task<Client> CreateClientAsync(Client client)
        {
            await _unitOfWork.Clients.AddAsync(client);
            await _unitOfWork.SaveChangesAsync();
            return client;
        }

        public async Task<Client?> UpdateClientAsync(int id, Client client)
        {
            var existingClient = await _unitOfWork.Clients.GetByIdAsync(id);
            if (existingClient == null)
                return null;

            existingClient.Name = client.Name;
            existingClient.Email = client.Email;

            _unitOfWork.Clients.Update(existingClient);
            await _unitOfWork.SaveChangesAsync();
            return existingClient;
        }

        public async Task<bool> DeleteClientAsync(int id)
        {
            var client = await _unitOfWork.Clients.GetByIdAsync(id);
            if (client == null)
                return false;

            _unitOfWork.Clients.Remove(client);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        // Ejercicio 9: Cliente con mayor número de pedidos
        public async Task<ClientWithOrderCountDto?> GetClientWithMostOrdersAsync()
        {
            var (clientId, orderCount) = await _unitOfWork.Orders.GetClientWithMostOrdersAsync();
            
            if (clientId == 0)
                return null;

            var client = await _unitOfWork.Clients.GetByIdAsync(clientId);
            
            if (client == null)
                return null;

            return new ClientWithOrderCountDto
            {
                ClientId = clientId,
                ClientName = client.Name,
                Email = client.Email,
                OrderCount = orderCount
            };
        }

        // Ejercicio 11: Productos vendidos a un cliente específico
        public async Task<ClientProductsDto?> GetProductsSoldToClientAsync(int clientId)
        {
            var client = await _unitOfWork.Clients.GetByIdAsync(clientId);
            if (client == null)
                return null;

            var products = await _unitOfWork.Orders.GetProductsSoldToClientAsync(clientId);
            
            return new ClientProductsDto
            {
                ClientId = clientId,
                ClientName = client.Name,
                ProductCount = products.Count(),
                Products = products.ToList()
            };
        }

        // Ejercicio 12: Clientes que compraron un producto específico
        public async Task<IEnumerable<string>> GetClientsWhoBoughtProductAsync(int productId)
        {
            return await _unitOfWork.Products.GetClientsWhoBoughtProductAsync(productId);
        }
        
    }
}