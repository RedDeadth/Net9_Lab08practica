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
        public async Task<Product?> GetMostExpensiveProductAsync()
        {
            return await _context.Products
                .OrderByDescending(p => p.Price)
                .FirstOrDefaultAsync();
        }

        // Ejercicio 7: Obtener el promedio de precio de los productos
        public async Task<decimal> GetAveragePriceAsync()
        {
            var hasProducts = await _context.Products.AnyAsync();
            if (!hasProducts)
                return 0;

            return await _context.Products
                .Select(p => p.Price)
                .AverageAsync();
        }

        // Ejercicio 8: Obtener productos sin descripción
        public async Task<IEnumerable<Product>> GetProductsWithoutDescriptionAsync()
        {
            return await _context.Products
                .Where(p => string.IsNullOrEmpty(p.Description))
                .ToListAsync();
        }

        // Ejercicio 12: Obtener clientes que compraron un producto específico
        public async Task<IEnumerable<string>> GetClientsWhoBoughtProductAsync(int productId)
        {
            return await _context.Orderdetails
                .Where(od => od.Productid == productId)
                .Select(od => od.Order.Client.Name)
                .Distinct()
                .ToListAsync();
        }
    }
}