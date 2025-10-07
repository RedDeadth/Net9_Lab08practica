using Lab08.Models;

namespace Lab08.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<IEnumerable<Product>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice);
    Task<Product?> GetProductWithOrderDetailsAsync(int productId);
}