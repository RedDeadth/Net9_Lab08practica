using Lab08.Models;

namespace Lab08.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<IEnumerable<Product>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice);
    Task<Product?> GetProductWithOrderDetailsAsync(int productId);
    // Ejercicio 5: Obtener el producto más caro
    Task<Product?> GetMostExpensiveProductAsync();
        
    // Ejercicio 7: Obtener el promedio de precio de los productos
    Task<decimal> GetAveragePriceAsync();
        
    // Ejercicio 8: Obtener productos sin descripción
    Task<IEnumerable<Product>> GetProductsWithoutDescriptionAsync();
        
    // Ejercicio 12: Clientes que compraron un producto específico
    Task<IEnumerable<string>> GetClientsWhoBoughtProductAsync(int productId);
}