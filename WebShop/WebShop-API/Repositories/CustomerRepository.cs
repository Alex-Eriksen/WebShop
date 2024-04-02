namespace WebShop_API.Repositories
{
    /// <summary>
    /// Interface with methods: GetAll, GetById, Create, Update and Delete.
    /// </summary>
    public interface ICustomerRepository
    {
        Task<List<Customer>> GetAll();
        Task<Customer> GetById( int customerId );
        Task<Customer> Create( Customer request );
        Task<Customer> Update( int customerId, Customer request );
        Task<Customer> Delete( int customerId );
    }

    /// <summary>
    /// Using ICustomerRepository interface.
    /// </summary>
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DatabaseContext m_context;

        /// <summary>
        /// Constructor for CustomerRepository.
        /// </summary>
        /// <param name="context"></param>
        public CustomerRepository(DatabaseContext context)
        {
            m_context = context;
        }

        /// <summary>
        /// Creates a Customer.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>request.CustomerID</returns>
        public async Task<Customer> Create( Customer request )
        {
            m_context.Customer.Add(request);
            await m_context.SaveChangesAsync();
            return await GetById(request.CustomerID);
        }

        /// <summary>
        /// Deletes a customer.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns>customer</returns>
        public async Task<Customer> Delete( int customerId )
        {
            Customer customer = await GetById(customerId);
            if (customer != null)
            {
                m_context.Customer.Remove(customer);
                await m_context.SaveChangesAsync();
            }

            return customer;
        }

        /// <summary>
        /// Gets all customers
        /// </summary>
        /// <returns>List of Customer</returns>
        public async Task<List<Customer>> GetAll()
        {
            return await m_context.Customer
                .Include(x => x.Account)
                .Include(x => x.Country)
                .Include(x => x.Payments)
                .Include(x => x.Orders)
                .ToListAsync();
        }

        /// <summary>
        /// Gets a customer by its Id.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns>CustomerID</returns>
        public async Task<Customer> GetById( int customerId )
        {
            return await m_context.Customer
                .Include(x => x.Account)
                .Include(x => x.Country)
                .Include(x => x.Payments)
                .Include(x => x.Orders)
                .FirstOrDefaultAsync(x => x.CustomerID == customerId);
        }

        /// <summary>
        /// Updates customer.
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="request"></param>
        /// <returns>customer</returns>
        public async Task<Customer> Update( int customerId, Customer request )
        {
            Customer customer = await GetById(customerId);
            if(customer != null)
            {
                customer.FirstName = request.FirstName;
                customer.LastName = request.LastName;
                customer.PhoneNumber = request.PhoneNumber;
                customer.CountryID = request.CountryID;
                customer.ZipCode = request.ZipCode;
                customer.Gender = request.Gender;
                customer.Modified_At = DateTime.UtcNow;

                await m_context.SaveChangesAsync();
            }

            return customer;
        }
    }
}
