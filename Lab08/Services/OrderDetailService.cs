using Lab08.DTOs;
using Lab08.Models;
using Lab08.Repositories;

namespace Lab08.Services
{
    public interface IOrderDetailService
    {
        Task<IEnumerable<Orderdetail>> GetAllOrderDetailsAsync();
        Task<Orderdetail?> GetOrderDetailByIdAsync(int id);
        Task<Orderdetail> CreateOrderDetailAsync(Orderdetail orderDetail);
        Task<Orderdetail?> UpdateOrderDetailAsync(int id, Orderdetail orderDetail);
        Task<bool> DeleteOrderDetailAsync(int id);
        
        // Ejercicio 10: Todos los pedidos con detalles
        Task<IEnumerable<OrderDetailDto>> GetAllOrdersWithDetailsAsync();
    }

    public class OrderDetailService : IOrderDetailService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderDetailService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Orderdetail>> GetAllOrderDetailsAsync()
        {
            return await _unitOfWork.OrderDetails.GetAllAsync();
        }

        public async Task<Orderdetail?> GetOrderDetailByIdAsync(int id)
        {
            return await _unitOfWork.OrderDetails.GetByIdAsync(id);
        }

        public async Task<Orderdetail> CreateOrderDetailAsync(Orderdetail orderDetail)
        {
            await _unitOfWork.OrderDetails.AddAsync(orderDetail);
            await _unitOfWork.SaveChangesAsync();
            return orderDetail;
        }

        public async Task<Orderdetail?> UpdateOrderDetailAsync(int id, Orderdetail orderDetail)
        {
            var existingDetail = await _unitOfWork.OrderDetails.GetByIdAsync(id);
            if (existingDetail == null)
                return null;

            existingDetail.Orderid = orderDetail.Orderid;
            existingDetail.Productid = orderDetail.Productid;
            existingDetail.Quantity = orderDetail.Quantity;

            _unitOfWork.OrderDetails.Update(existingDetail);
            await _unitOfWork.SaveChangesAsync();
            return existingDetail;
        }

        public async Task<bool> DeleteOrderDetailAsync(int id)
        {
            var orderDetail = await _unitOfWork.OrderDetails.GetByIdAsync(id);
            if (orderDetail == null)
                return false;

            _unitOfWork.OrderDetails.Remove(orderDetail);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        // Ejercicio 10: Obtener todos los pedidos con detalles
        public async Task<IEnumerable<OrderDetailDto>> GetAllOrdersWithDetailsAsync()
        {
            return await _unitOfWork.Orders.GetAllOrdersWithDetailsAsync();
        }
    }
}