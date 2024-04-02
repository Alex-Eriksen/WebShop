using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop_API.Helpers;

namespace WebShopUnitTests.Repository
{
    public class CustomerRepositoryTests
    {
        private readonly DbContextOptions<DatabaseContext> _options;
        private readonly DatabaseContext _context;
        private readonly CustomerRepository _customerRepository;

        public CustomerRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "CustomerRepository")
                .Options;

            _context = new(_options);

            _customerRepository = new(_context);
        }
 
        [Fact]
        public async void GetAll_ShouldReturnListOfCustomers_WhenCustomersExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            _context.Customer.Add(new()
            {
                CustomerID = 1,
                Account = new Account { AccountID = 1 },
                FirstName = "Matthias",
                LastName = "Bryde",
                PhoneNumber = "42483787",
                Country = new Country { CountryID = 1, CountryName = "Danmark" },
                ZipCode = 2620,
                Gender = "Male",
            });

            _context.Customer.Add(new()
            {
                CustomerID = 2,
                Account = new Account { AccountID = 2 },
                FirstName = "Alexander",
                LastName = "Eriksen",
                PhoneNumber = "12345678",
                Country = new Country { CountryID = 2, CountryName = "Sverige" },
                ZipCode = 2600,
                Gender = "Female",
            });

            await _context.SaveChangesAsync();

            // Act
            var result = await _customerRepository.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Customer>>(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async void GetAll_ShouldReturnEmptyListOfCustomers_WhenNoCustomersExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _customerRepository.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Customer>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public async void GetById_ShouldReturnCustomer_WhenCustomerExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int customerId = 1;

            _context.Customer.Add(new()
            {
                CustomerID = 1,
                Account = new Account { AccountID = 1 },
                FirstName = "Matthias",
                LastName = "Bryde",
                PhoneNumber = "42483787",
                Country = new Country { CountryID = 1, CountryName = "Danmark" },
                ZipCode = 2620,
                Gender = "Male",
            });

            await _context.SaveChangesAsync();

            // Act
            var result = await _customerRepository.GetById(customerId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Customer>(result);
            Assert.Equal(customerId, result.CustomerID);
        }

        [Fact]
        public async void GetById_ShouldReturnNull_WhenCustomerDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _customerRepository.GetById(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void Create_ShouldAddNewIdToCustomer_WhenSavingToDatabase()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int expectedId = 1;

            Customer customer = new()
            {
                Account = new Account { AccountID = 1 },
                FirstName = "Matthias",
                LastName = "Bryde",
                PhoneNumber = "42483787",
                Country = new Country { CountryID = 1, CountryName = "Danmark" },
                ZipCode = 2620,
                Gender = "Male",
            };


            // Act
            var result = await _customerRepository.Create(customer);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Customer>(result);
            Assert.Equal(expectedId, result.CustomerID);
        }
        
        [Fact]
        public async void Create_ShouldFailToAddNewCustomer_WhenCustomerIdAlreadyExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            Customer customer = new()
            {
                CustomerID = 1,
                Account = new Account { AccountID = 1 },
                FirstName = "Matthias",
                LastName = "Bryde",
                PhoneNumber = "42483787",
                Country = new Country { CountryID = 1, CountryName = "Danmark" },
                ZipCode = 2620,
                Gender = "Male",
            };

            // Act
            var result = await _customerRepository.Create(customer);

            async Task action() => await _customerRepository.Create(customer);

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(action);
            Assert.Contains("An item with the same key has already been added", ex.Message);
        }

        [Fact]
        public async void Update_ShouldChangeValuesOnCustomer_WhenCustomerExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int customerId = 1;

            Customer newCustomer = new()
            {
                Account = new Account { AccountID = 1 },
                FirstName = "Matthias",
                LastName = "Bryde",
                PhoneNumber = "42483787",
                Country = new Country { CountryID = 1, CountryName = "Danmark" },
                ZipCode = 2620,
                Gender = "Male",
            };

            _context.Customer.Add(newCustomer);
            await _context.SaveChangesAsync();

            Customer updatedCustomer = new()
            {
                FirstName = "Mathias",
                LastName = "Bryden",
                PhoneNumber = "12483787",
                Country = new Country { CountryID = 2, CountryName = "Sverige" },
                ZipCode = 2630,
                Gender = "Female",
            };

            // Act
            var result = await _customerRepository.Update(customerId, updatedCustomer);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Customer>(result);
            Assert.Equal(customerId, result.CustomerID);
            Assert.Equal(updatedCustomer.FirstName, result.FirstName);
            Assert.Equal(updatedCustomer.LastName, result.LastName);
            Assert.Equal(updatedCustomer.PhoneNumber, result.PhoneNumber);
            Assert.Equal(updatedCustomer.CountryID, result.CountryID);
            Assert.Equal(updatedCustomer.ZipCode, result.ZipCode);
            Assert.Equal(updatedCustomer.Gender, result.Gender);
        }

        [Fact]
        public async void Update_ShouldReturnNull_WhenCustomerDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int customerId = 1;

            Customer customer = new()
            {
                CustomerID = 1,
                Account = new Account { AccountID = 1 },
                FirstName = "Matthias",
                LastName = "Bryde",
                PhoneNumber = "42483787",
                Country = new Country { CountryID = 1, CountryName = "Danmark" },
                ZipCode = 2620,
                Gender = "Male",
            };

            // Act
            var result = await _customerRepository.Update(customerId, customer);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void Delete_ShouldReturnDeletedCustomer_WhenCustomerIsDeleted()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int customerId = 1;

            Customer newCustomer = new()
            {
                CustomerID = 1,
                Account = new Account { AccountID = 1 },
                FirstName = "Matthias",
                LastName = "Bryde",
                PhoneNumber = "42483787",
                Country = new Country { CountryID = 1, CountryName = "Danmark" },
                ZipCode = 2620,
                Gender = "Male",
            };

            _context.Customer.Add(newCustomer);
            await _context.SaveChangesAsync();

            // Act
            var result = await _customerRepository.Delete(customerId);
            var customer = await _customerRepository.GetById(customerId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Customer>(result);
            Assert.Equal(customerId, result.CustomerID);

            Assert.Null(customer);
        }

        [Fact]
        public async void Delete_ShouldReturnNull_WhenCustomerDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _customerRepository.Delete(1);

            // Assert
            Assert.Null(result);
        }
    }
}
