using Lab08.Data;
using Lab08.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab08.Repositories
{
    public class ClientRepository : Repository<Client>, IClientRepository
    {
        public ClientRepository(LINQExampleContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Client>> GetClientsByNameAsync(string name)
        {
            return await _dbSet
                .Where(c => c.Name.Contains(name))
                .ToListAsync();
        }

        public async Task<Client?> GetClientWithOrdersAsync(int clientId)
        {
            return await _dbSet
                .Include(c => c.Orders)
                .FirstOrDefaultAsync(c => c.Clientid == clientId);
        }
    }
}