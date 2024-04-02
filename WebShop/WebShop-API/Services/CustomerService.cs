using System.Net.Mail;

namespace WebShop_API.Services
{
    public interface ICustomerService
    {
        Task<List<StaticCustomerResponse>> GetAll();
        Task<DirectCustomerResponse> GetById(int customerId);
        Task<DirectCustomerResponse> Create( NewCustomerRequest request );
        Task<DirectCustomerResponse> Update(int customerId, NewCustomerRequest request);
        Task<DirectCustomerResponse> Delete(int customerId);
        Task<DirectPaymentResponse> CreatePayment( PaymentRequest request );
        Task<DirectPaymentResponse> UpdatePayment( int paymentId, PaymentRequest request );
        Task<DirectPaymentResponse> DeletePayment( int paymentId );
        Task<DirectPaymentResponse> GetPayment( int paymentId );
    }

    /// <summary>
    /// CustomerService is used to transfere data to and from CustomerRepository and CustomerController.
    /// </summary>
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository m_customerRepository;
        private readonly IPaymentRepository m_paymentRepository;
        private readonly IAccountRepository m_accountRepository;
        private readonly IMapper m_mapper;

        /// <summary>
        /// Constructor of CustomerService.
        /// </summary>
        /// <param name="customerRepository"></param>
        /// <param name="mapper"></param>
        /// <param name="paymentRepository"></param>
        public CustomerService(ICustomerRepository customerRepository, IMapper mapper, IPaymentRepository paymentRepository, IAccountRepository accountRepository )
        {
            m_customerRepository = customerRepository;
            m_mapper = mapper;
            m_accountRepository = accountRepository;
            m_paymentRepository = paymentRepository;
        }

        /// <summary>
        /// Creates a request of CustomerRequest.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>customer or null</returns>
        public async Task<DirectCustomerResponse> Create( NewCustomerRequest request )
        {
            Account account = await m_accountRepository.Create( m_mapper.Map<Account>( request.Account ) );
            if(account == null)
            {
                return null;
            }

            request.Customer.AccountID = account.AccountID;

            Customer customer = await m_customerRepository.Create( m_mapper.Map<Customer>( request.Customer ) );
            if(customer != null)
            {
                return m_mapper.Map<DirectCustomerResponse>(customer);
            }

            return null;
        }

        /// <summary>
        /// Creates a request of PaymentRequest.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>payment or null</returns>
        public async Task<DirectPaymentResponse> CreatePayment( PaymentRequest request )
        {
            Payment payment = await m_paymentRepository.Create( m_mapper.Map<Payment>( request ) );

            if(payment != null)
            {
                return m_mapper.Map<DirectPaymentResponse>( payment );
            }

            return null;
        }

        /// <summary>
        /// Deletes a customer by its id.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns>customer or null</returns>
        public async Task<DirectCustomerResponse> Delete( int customerId )
        {
            Customer customer = await m_customerRepository.Delete(customerId);
            if(customer != null)
            {
                return m_mapper.Map<DirectCustomerResponse>(customer);
            }

            return null;
        }
        
        /// <summary>
        /// Deletes a payment by its id.
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns></returns>
        public async Task<DirectPaymentResponse> DeletePayment( int paymentId )
        {
            Payment payment = await m_paymentRepository.Delete( paymentId );
            if (payment != null)
            {
                return m_mapper.Map<DirectPaymentResponse>( payment );
            }

            return null;
        }

        /// <summary>
        /// Gets all customers.
        /// </summary>
        /// <returns>customer or null</returns>
        public async Task<List<StaticCustomerResponse>> GetAll()
        {
            List<Customer> customers = await m_customerRepository.GetAll();
            if(customers != null)
            {
                return customers.Select(customer => m_mapper.Map<Customer, StaticCustomerResponse>(customer)).ToList();
            }

            return null;
        }

        /// <summary>
        /// Gets a customer by its id.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns>customer or null</returns>
        public async Task<DirectCustomerResponse> GetById( int customerId )
        {
            Customer customer = await m_customerRepository.GetById(customerId);

            if(customer != null)
            {
                return m_mapper.Map<DirectCustomerResponse>(customer);
            }

            return null;
        }

        /// <summary>
        /// Gets a payment by its id.
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns>payment or null</returns>
        public async Task<DirectPaymentResponse> GetPayment( int paymentId )
        {
            Payment payment = await m_paymentRepository.GetById( paymentId );
            if(payment != null)
            {
                return m_mapper.Map<DirectPaymentResponse>( payment );
            }

            return null;
        }

        /// <summary>
        /// Updates a customer.
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="request"></param>
        /// <returns>customer or null</returns>
        public async Task<DirectCustomerResponse> Update( int customerId, NewCustomerRequest request )
        {
            Account account = await m_accountRepository.Update( request.Customer.AccountID, m_mapper.Map<Account>( request.Account ) );
            if(account == null)
            {
                return null;
            }

            Customer customer = await m_customerRepository.Update( customerId, m_mapper.Map<Customer>( request.Customer ) );
            if(customer != null)
            {
                return m_mapper.Map<DirectCustomerResponse>(customer);
            }

            return null;
        }

        /// <summary>
        /// Uadates a payment.
        /// </summary>
        /// <param name="paymentId"></param>
        /// <param name="request"></param>
        /// <returns>payment or null</returns>
        public async Task<DirectPaymentResponse> UpdatePayment( int paymentId, PaymentRequest request )
        {
            Payment payment = await m_paymentRepository.Update( paymentId, m_mapper.Map<Payment>( request ) );
            if(payment != null)
            {
                return m_mapper.Map<DirectPaymentResponse>( payment );
            }

            return null;
        }
    }
}
