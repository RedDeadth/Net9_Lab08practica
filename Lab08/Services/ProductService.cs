using Lab08.DTOs;
using Lab08.Models;
using Lab08.Repositories;

namespace Lab08.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProductsByMinimumPriceAsync(decimal minPrice);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task<Product> CreateProductAsync(Product product);
        Task<Product?> UpdateProductAsync(int id, Product product);
        Task<bool> DeleteProductAsync(int id);
        
        // Ejercicio 5: Producto más caro
        Task<Product?> GetMostExpensiveProductAsync();
        
        // Ejercicio 7: Promedio de precio
        Task<decimal> GetAveragePriceAsync();
        
        // Ejercicio 8: Productos sin descripción
        Task<IEnumerable<Product>> GetProductsWithoutDescriptionAsync();
        
        // Ejercicio 12: Clientes que compraron un producto
        Task<ProductClientsDto?> GetClientsWhoBoughtProductAsync(int productId);
    }
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByMinimumPriceAsync(decimal minPrice)
        {
            var allProducts = await _unitOfWork.Products.GetAllAsync();

            var filteredProducts = allProducts
                .Where(p => p.Price > minPrice)
                .Select(p => new ProductDto
                {
                    Productid = p.Productid,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price
                })
                .ToList();

            return filteredProducts;
        }
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _unitOfWork.Products.GetAllAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _unitOfWork.Products.GetByIdAsync(id);
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> UpdateProductAsync(int id, Product product)
        {
            var existingProduct = await _unitOfWork.Products.GetByIdAsync(id);
            if (existingProduct == null)
                return null;

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Description = product.Description;

            _unitOfWork.Products.Update(existingProduct);
            await _unitOfWork.SaveChangesAsync();
            return existingProduct;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
                return false;

            _unitOfWork.Products.Remove(product);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        // Ejercicio 5: Obtener el producto más caro
        public async Task<Product?> GetMostExpensiveProductAsync()
        {
            return await _unitOfWork.Products.GetMostExpensiveProductAsync();
        }

        // Ejercicio 7: Obtener el promedio de precio
        public async Task<decimal> GetAveragePriceAsync()
        {
            return await _unitOfWork.Products.GetAveragePriceAsync();
        }

        // Ejercicio 8: Obtener productos sin descripción
        public async Task<IEnumerable<Product>> GetProductsWithoutDescriptionAsync()
        {
            return await _unitOfWork.Products.GetProductsWithoutDescriptionAsync();
        }

        // Ejercicio 12: Obtener clientes que compraron un producto
        public async Task<ProductClientsDto?> GetClientsWhoBoughtProductAsync(int productId)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(productId);
            if (product == null)
                return null;

            var clients = await _unitOfWork.Products.GetClientsWhoBoughtProductAsync(productId);
            
            return new ProductClientsDto
            {
                ProductId = productId,
                ProductName = product.Name,
                ClientCount = clients.Count(),
                Clients = clients.ToList()
            };
        }
    }
}