namespace WebShop_API.Repositories
{
    public interface ITransactionRepository
    {
        Task<List<Transaction>> GetAll();
        Task<Transaction> GetById(int transactionId);
        Task<Transaction> Create(Transaction request);
        Task<Transaction> Update(int transactionId, Transaction request);
    }

    public class TransactionRepository : ITransactionRepository
    {
        private readonly DatabaseContext m_context;

        /// <summary>
        /// Constructor for CustomerRepository.
        /// </summary>
        /// <param name="context"></param>
        public TransactionRepository(DatabaseContext context)
        {
            m_context = context;
        }

        public async Task<Transaction> Create(Transaction request)
        {
            m_context.Transaction.Add(request);
            await m_context.SaveChangesAsync();
            return await GetById(request.TransactionID);
        }

        public async Task<List<Transaction>> GetAll()
        {
            return await m_context.Transaction.ToListAsync();
        }

        public async Task<Transaction> GetById(int transactionId)
        {
            return await m_context.Transaction
                .Include(x => x.Product)
                .Include(x => x.Order)
                .Include(x => x.Discount)
                .FirstOrDefaultAsync(x => x.TransactionID == transactionId);
        }

        public async Task<Transaction> Update(int transactionId, Transaction request)
        {
            Transaction transaction = await GetById(transactionId);
            if(transaction != null)
            {
                transaction.ProductID = request.ProductID;
                transaction.OrderID = request.OrderID;
                transaction.ProductAmount = request.ProductAmount;
                transaction.ProductPrice = request.ProductPrice;
                transaction.DiscountID = request.DiscountID;
                transaction.Modified_At = DateTime.UtcNow;

                await m_context.SaveChangesAsync();
            }

            return transaction;
        }
    }
}
