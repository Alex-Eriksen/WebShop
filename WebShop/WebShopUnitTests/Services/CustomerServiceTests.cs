namespace WebShopUnitTests.Services
{
    public class CustomerServiceTests
    {
        private readonly CustomerService m_customerService;
        private readonly IPaymentRepository paymentRepository;
        private readonly Mock<IAccountRepository> m_accountRepositoryMock = new Mock<IAccountRepository>();
        private readonly Mock<ICustomerRepository> m_customerRepositoryMock = new Mock<ICustomerRepository>();
        private readonly IMapper m_mapper;

        public CustomerServiceTests()
        {
            if(m_mapper == null)
            {
                var mapperConfig = new MapperConfiguration( mc => {
                    mc.AddProfile( new WebShop_API.Helpers.AutoMapper.AutoMapper() );
                } );
                IMapper mapper = mapperConfig.CreateMapper();
                m_mapper = mapper;
            }
            m_customerService = new( m_customerRepositoryMock.Object, m_mapper, paymentRepository, m_accountRepositoryMock.Object );
        }

        [Fact]
        public async void GetAll_ShouldReturnListOfDirectCustomerResponses_WhenCustomersExists()
        {
            List<Customer> customers = new();
            List<StaticCustomerResponse> responses = new();

            customers.Add(new()
            {
                CustomerID = 1,
                Account = new Account { AccountID = 1},
                FirstName = "Matthias",
                LastName = "Bryde",
                PhoneNumber = "42483787",
                Country = new Country { CountryID = 1, CountryName = "Danmark"},
                ZipCode = 2620,
                Gender = "Male"
            });

            customers.Add(new()
            {
                CustomerID = 2,
                Account = new Account { AccountID = 2 },
                FirstName = "Alexander",
                LastName = "Eriksen",
                PhoneNumber = "12345678",
                Country = new Country { CountryID = 1, CountryName = "Danmark" },
                ZipCode = 2600,
                Gender = "Female"
            });

            m_customerRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(customers);

            var result = await m_customerService.GetAll();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.IsType<List<StaticCustomerResponse>>(result);
            Assert.NotNull( result[ 0 ] );
            Assert.NotNull( result[ 1 ] );
        }

        [Fact]
        public async void GetAll_ShouldReturnEmptyListOfDirectCustomerResponses_WhenNoCustomersExists()
        {
            List<Customer> responses = new();

            m_customerRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(responses);

            var result = await m_customerService.GetAll();

            Assert.NotNull(result);
            Assert.Empty(result);
            Assert.IsType<List<StaticCustomerResponse>>(result);
        }

        [Fact]
        public async void GetById_ShouldReturnDirectCustomerResponse_WhenCustomerExists()
        {
            int customerId = 1;

            Customer customer = new()
            {
                CustomerID = customerId,
                Account = new Account { AccountID = 1 },
                FirstName = "Matthias",
                LastName = "Bryde",
                PhoneNumber = "42483787",
                Country = new Country { CountryID = 1, CountryName = "Danmark" },
                ZipCode = 2620,
                Gender = "Male"
            };

            m_customerRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(customer);

            var result = await m_customerService.GetById(customerId);

            Assert.NotNull(result);
            Assert.IsType<DirectCustomerResponse>(result);
            Assert.Equal(customer.CustomerID, result.CustomerID);
        }

        [Fact]
        public async void GetById_ShouldReturnNull_WhenCustomerDoseNotExists()
        {
            int customerId = 1;

            m_customerRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(() => null);

            var result = await m_customerService.GetById(customerId);

            Assert.Null(result);
        }

        [Fact]
        public async void Create_ShouldReturnDirectCustomerResponse_WhenCreateIsSuccess()
        {
            // Arrange
            NewCustomerRequest newCustomer = new() 
            { 
                Account = new AccountRequest(),
                Customer = new CustomerRequest()
            };

            int customerId = 1;

            Account account = new()
            {
                AccountID = 1,
                Username = "alex8863",
                Password = BCrypt.Net.BCrypt.HashPassword( "Passw0rd" ),
                Email = "example@test.com",
                Role = "Customer"
            };

            Customer customer = new()
            {
                CustomerID = customerId,
                Account = account,
                FirstName = "Matthias",
                LastName = "Bryde",
                PhoneNumber = "42483787",
                Country = new Country {  CountryID = 1, CountryName = "Danamrk"},
                ZipCode = 2620,
                Gender = "Male"
            };


            m_customerRepositoryMock
                .Setup( x => x.Create( It.IsAny<Customer>() ) )
                .ReturnsAsync( customer );

            m_accountRepositoryMock
                .Setup( x => x.Create( It.IsAny<Account>() ) )
                .ReturnsAsync( account );

            // Act
            var result = await m_customerService.Create( newCustomer );

            // Assert
            Assert.NotNull( result );
            Assert.IsType<DirectCustomerResponse>( result );
        }

        [Fact]
        public async void Create_ShouldRetrunNull_WhenRepositoryReturnsNull()
        {
            NewCustomerRequest newCustomer = new();

           m_customerRepositoryMock.Setup(x => x.Create(It.IsAny<Customer>())).ReturnsAsync(() => null);

           var result = await m_customerService.Create(newCustomer);

           Assert.Null(result);
        }

        [Fact]
        public async void Update_ShouldReturnDirectCustomerResponse_WhenUpdateIsSuccess()
        {
            NewCustomerRequest customerRequest = new()
            {
                Customer = new()
                {
                    AccountID = 1,
                    FirstName = "Matthias",
                    LastName = "Bryde",
                    PhoneNumber = "42483787",
                    CountryID = 1,
                    ZipCode = 2620,
                    Gender = "Male"
                },
                Account = new()
                {
                    Email = "test@test.com",
                    Password = "POG",
                    Role = "Customer",
                    Username = "test"
                }
            };

            int customerId = 1;

            Customer customer = new()
            {
                CustomerID = customerId,
                Account = new Account { AccountID = 1 },
                FirstName = "Matthias",
                LastName = "Bryde",
                PhoneNumber = "42483787",
                Country = new Country { CountryID = 1, CountryName = "Danamrk" },
                ZipCode = 2620,
                Gender = "Male"
            };

            m_customerRepositoryMock.Setup(x => x.Update(It.IsAny<int>(), It.IsAny<Customer>())).ReturnsAsync(customer);

            var result = await m_customerService.Update(customerId, customerRequest);

            Assert.NotNull(result);
            Assert.IsType<DirectCustomerResponse>(result);
            Assert.Equal(customer.CustomerID, result.CustomerID);
            Assert.Equal(customerRequest.Customer.FirstName, result.FirstName);
            Assert.Equal(customerRequest.Customer.LastName, result.LastName);
            Assert.Equal(customerRequest.Customer.PhoneNumber, result.PhoneNumber);
            Assert.Equal(customerRequest.Customer.ZipCode, result.ZipCode);
            Assert.Equal(customerRequest.Customer.Gender, result.Gender);
        }

        [Fact]
        public async void Update_ShouldReturnNull_WhenCustomerDoesNotExists()
        {
            NewCustomerRequest customerRequest = new();

            int customerId = 1;

            m_customerRepositoryMock.Setup(x => x.Update(It.IsAny<int>(), It.IsAny<Customer>())).ReturnsAsync(() => null);

            var result = await m_customerService.Update(customerId, customerRequest);

            Assert.Null(result);
        }

        [Fact]
        public async void Delete_ShouldReturnDirectCustomerResponse_WhenDeleteIsSuccess()
        {
            int customerId = 1;

            Customer customer = new()
            {
                CustomerID = customerId,
                Account = new Account { AccountID = 1 },
                FirstName = "Matthias",
                LastName = "Bryde",
                PhoneNumber = "42483787",
                Country = new Country { CountryID = 1, CountryName = "Danamrk" },
                ZipCode = 2620,
                Gender = "Male"
            };

            m_customerRepositoryMock.Setup(x => x.Delete(It.IsAny<int>())).ReturnsAsync(customer);

            var result = await m_customerService.Delete(customerId);

            Assert.NotNull(result);
            Assert.IsType<DirectCustomerResponse>(result);
            Assert.Equal(customerId, result.CustomerID);
        }

        [Fact]
        public async void Delete_ShouldReturnNull_WhenCustomerDoesNotExists()
        {
            int customerId = 1;

            m_customerRepositoryMock.Setup(x => x.Delete(It.IsAny<int>())).ReturnsAsync(() => null);

            var result = await m_customerService.Delete(customerId);

            Assert.Null(result);
        }

    }
}
