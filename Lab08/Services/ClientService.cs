using Lab08.DTOs;
using Lab08.Repositories;

namespace Lab08.Services
{
    public interface IClientService
    {
        Task<IEnumerable<ClientDto>> GetClientsByNameAsync(string name);
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
    }
}