using Lab08.Data;
using Lab08.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab08.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(LINQExampleContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Product>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            return await _dbSet
                .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
                .ToListAsync();
        }

        public async Task<Product?> GetProductWithOrderDetailsAsync(int productId)
        {
            return await _dbSet
                .Include(p => p.Orderdetails)
                .FirstOrDefaultAsync(p => p.Productid == productId);
        }
    }
}