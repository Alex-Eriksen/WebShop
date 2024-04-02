using WebShop_API.Database.Entities;

namespace WebShop_API.Repositories
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAll();
        Task<Order> GetById(int orderId);
        Task<Order> Create(Order request);
        Task<Order> Update(int orderId, Order request);
    }

    public class OrderRepository : IOrderRepository
    {
        private readonly DatabaseContext m_context;

        public OrderRepository(DatabaseContext context)
        {
            m_context = context;
        }

        public async Task<Order> Create(Order request)
        {
            m_context.Order.Add(request);
            await m_context.SaveChangesAsync();
            return await GetById(request.OrderID);
        }

        public async Task<List<Order>> GetAll()
        {
            return await m_context.Order.ToListAsync();
        }

        public async Task<Order> GetById(int orderId)
        {
            return await m_context.Order
                .Include(x => x.Customer)
                .Include(x => x.Transactions)
                .FirstOrDefaultAsync(x => x.OrderID == orderId);
        }

        public async Task<Order> Update(int orderId, Order request)
        {
            Order order = await GetById(orderId);
            if(order != null)
            {
                order.CustomerID = request.CustomerID;
                order.OrderTotal = request.OrderTotal;

                await m_context.SaveChangesAsync();
            }

            return order;
        }
    }
}
