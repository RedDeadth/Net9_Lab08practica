using Lab08.Models;

namespace Lab08.Repositories
{
    // IClientRepository
    public interface IClientRepository : IRepository<Client>
    {
        Task<IEnumerable<Client>> GetClientsByNameAsync(string name);
        Task<Client?> GetClientWithOrdersAsync(int clientId);
    }
}