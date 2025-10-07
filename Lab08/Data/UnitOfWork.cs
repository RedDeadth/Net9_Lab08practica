using Lab08.Data;

namespace Lab08.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LINQExampleContext _context;
        private IClientRepository? _clients;
        private IProductRepository? _products;
        private IOrderRepository? _orders;
        private IOrderDetailRepository? _orderDetails;

        public UnitOfWork(LINQExampleContext context)
        {
            _context = context;
        }

        public IClientRepository Clients
        {
            get
            {
                if (_clients == null)
                {
                    _clients = new ClientRepository(_context);
                }
                return _clients;
            }
        }

        public IProductRepository Products
        {
            get
            {
                if (_products == null)
                {
                    _products = new ProductRepository(_context);
                }
                return _products;
            }
        }

        public IOrderRepository Orders
        {
            get
            {
                if (_orders == null)
                {
                    _orders = new OrderRepository(_context);
                }
                return _orders;
            }
        }

        public IOrderDetailRepository OrderDetails
        {
            get
            {
                if (_orderDetails == null)
                {
                    _orderDetails = new OrderDetailRepository(_context);
                }
                return _orderDetails;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}