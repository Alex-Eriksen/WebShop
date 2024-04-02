namespace WebShop_API.Repositories
{
    /// <summary>
    /// Interface with methods: GetAll, GetById, Create, Update and Delete.
    /// </summary>
    public interface IPaymentRepository
    {
        Task<List<Payment>> GetAll();
        Task<Payment> GetById( int paymentId );
        Task<Payment> Create( Payment request );
        Task<Payment> Update( int paymentId, Payment request );
        Task<Payment> Delete( int paymentId );
    }

    /// <summary>
    /// Using IPaymentRepository interface.
    /// </summary>
    public class PaymentRepository : IPaymentRepository
    {
        private readonly DatabaseContext m_context;

        /// <summary>
        /// Constructor for PaymentRepository.
        /// </summary>
        /// <param name="context"></param>
        public PaymentRepository(DatabaseContext context)
        {
            m_context = context;
        }

        /// <summary>
        /// Creates a Payment.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>request</returns>
        public async Task<Payment> Create( Payment request )
        {
            m_context.Payment.Add(request);
            await m_context.SaveChangesAsync();
            return await GetById(request.PaymentID);
        }

        /// <summary>
        /// Deletes a Payment.
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns>payment</returns>
        public async Task<Payment> Delete( int paymentId )
        {
            Payment payment = await GetById(paymentId);
            if(payment != null)
            {
                m_context.Payment.Remove(payment);
                await m_context.SaveChangesAsync();
            }

            return payment;
        }

        /// <summary>
        /// Gets all Payments.
        /// </summary>
        /// <returns>List of Payment</returns>
        public async Task<List<Payment>> GetAll()
        {
            return await m_context.Payment.Include(x => x.Customer).ToListAsync();
        }

        /// <summary>
        /// Gets a payment by its id.
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns>Payment</returns>
        public async Task<Payment> GetById( int paymentId )
        {
            return await m_context.Payment.Include(x => x.Customer).FirstOrDefaultAsync(x => x.PaymentID == paymentId);
        }

        /// <summary>
        /// Updates Payment.
        /// </summary>
        /// <param name="paymentId"></param>
        /// <param name="request"></param>
        /// <returns>payment</returns>
        public async Task<Payment> Update( int paymentId, Payment request )
        {
            Payment payment = await GetById(paymentId);
            if (payment != null)
            {
                payment.CustomerID = request.CustomerID;
                payment.PaymentType = request.PaymentType;
                payment.CardNumber = request.CardNumber;
                payment.Provider = request.Provider;
                payment.Expiry = request.Expiry;
                payment.Modified_At = DateTime.UtcNow;

                await m_context.SaveChangesAsync();
            }

            return payment;
        }
    }
}
