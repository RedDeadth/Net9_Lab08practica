namespace Lab08.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IClientRepository Clients { get; }
        IProductRepository Products { get; }
        IOrderRepository Orders { get; }
        IOrderDetailRepository OrderDetails { get; }
        
        Task<int> SaveChangesAsync();
        int SaveChanges();
    }
}