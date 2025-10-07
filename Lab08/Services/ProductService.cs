using Lab08.DTOs;
using Lab08.Repositories;

namespace Lab08.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProductsByMinimumPriceAsync(decimal minPrice);
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
    }
}