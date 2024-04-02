using WebShop_API.Database.Entities;
using WebShop_API.DTOs.Product;

namespace WebShop_API.Services
{
    public interface ITransactionService
    {
        Task<List<StaticTransactionResponse>> GetAllTransactions();
        Task<DirectTransactionResponse> GetByTransactionId(int transactionId);
        Task<DirectTransactionResponse> CreateTransaction(TransactionRequest request);
        Task<DirectTransactionResponse> UpdateTransaction(int transactionId, TransactionRequest request);
        Task<List<StaticOrderResponse>> GetAllOrders();
        Task<DirectOrderResponse> GetByOrderId(int orderId);
        Task<DirectOrderResponse> CreateOrder(OrderRequest request);
        Task<DirectOrderResponse> UpdateOrder(int orderId, OrderRequest request);

    }

    public class TransactionService : ITransactionService
    {

        private readonly ITransactionRepository m_transactionRepository;
        private readonly IOrderRepository m_orderRepository;
        private readonly IMapper m_mapper;
        private readonly IProductRepository m_productRepository;

        public TransactionService(ITransactionRepository transactionRepository, IOrderRepository orderRepository, IMapper mapper, IProductRepository productRepository)
        {
            m_transactionRepository = transactionRepository;
            m_orderRepository = orderRepository;
            m_mapper = mapper;
            m_productRepository = productRepository;
        }

        public async Task<DirectOrderResponse> CreateOrder(OrderRequest request)
        {
            Order order = await m_orderRepository.Create(m_mapper.Map<Order>(request));
            if(order != null)
            {
                return m_mapper.Map<DirectOrderResponse>(order);
            }

            return null;
        }

        public async Task<DirectTransactionResponse> CreateTransaction(TransactionRequest request)
        {
            Transaction transaction = await m_transactionRepository.Create(m_mapper.Map<Transaction>(request));
            if (transaction != null)
            {
                await m_productRepository.UpdateQuantity(transaction.ProductID, transaction.ProductAmount);
                return m_mapper.Map<DirectTransactionResponse>(transaction);
            }

            return null;
        }

        public async Task<List<StaticOrderResponse>> GetAllOrders()
        {
            List<Order> orders = await m_orderRepository.GetAll();
            if(orders != null)
            {
                return orders.Select(order => m_mapper.Map<StaticOrderResponse>(order)).ToList();
            }

            return null;
        }

        public async Task<List<StaticTransactionResponse>> GetAllTransactions()
        {
            List<Transaction> transactions = await m_transactionRepository.GetAll();
            if (transactions != null)
            {
                return transactions.Select(transaction => m_mapper.Map<StaticTransactionResponse>(transaction)).ToList();
            }

            return null;
        }

        public async Task<DirectOrderResponse> GetByOrderId(int orderId)
        {
            Order order = await m_orderRepository.GetById(orderId);
            if (order != null)
            {
                return m_mapper.Map<DirectOrderResponse>(order);
            }

            return null;
        }

        public async Task<DirectTransactionResponse> GetByTransactionId(int transactionId)
        {
            Transaction transaction = await m_transactionRepository.GetById(transactionId);
            if (transaction != null)
            {
                return m_mapper.Map<DirectTransactionResponse>(transaction);
            }

            return null;
        }

        public async Task<DirectOrderResponse> UpdateOrder(int orderId, OrderRequest request)
        {
            Order order = await m_orderRepository.Update(orderId, m_mapper.Map<Order>(request));

            if (order != null)
            {
                return m_mapper.Map<DirectOrderResponse>(order);
            }

            return null;
        }

        public async Task<DirectTransactionResponse> UpdateTransaction(int transactionId, TransactionRequest request)
        {
            Transaction transaction = await m_transactionRepository.Update(transactionId, m_mapper.Map<Transaction>(request));
            if (transaction != null)
            {
                return m_mapper.Map<DirectTransactionResponse>(transaction);
            }

            return null;
        }
    }
}
