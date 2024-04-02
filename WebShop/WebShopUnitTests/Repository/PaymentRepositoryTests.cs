using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop_API.Database.Entities;

namespace WebShopUnitTests.Repository
{
    public class PaymentRepositoryTests
    {
        private readonly DbContextOptions<DatabaseContext> _options;
        private readonly DatabaseContext _context;
        private readonly PaymentRepository _paymentRepository;

        public PaymentRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "PaymentRepository")
                .Options;

            _context = new(_options);

            _paymentRepository = new(_context);
        }

        [Fact]
        public async void GetAll_ShouldReturnListOfPayments_WhenPaymentsExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            _context.Payment.Add(new()
            {
                PaymentID = 1,
                Customer = new Customer
                {
                    CustomerID = 1,
                    Account = new Account { AccountID = 1 },
                    FirstName = "Matthias",
                    LastName = "Bryde",
                    PhoneNumber = "42483787",
                    Country = new Country { CountryID = 1, CountryName = "Danmark" },
                    ZipCode = 2620,
                    Gender = "Male",
                }
            });

            _context.Payment.Add(new()
            {
                PaymentID = 2,
                Customer = new Customer
                {
                    CustomerID = 2,
                    Account = new Account { AccountID = 2 },
                    FirstName = "Alexander",
                    LastName = "Eriksen",
                    PhoneNumber = "12345678",
                    Country = new Country { CountryID = 2, CountryName = "Sverige" },
                    ZipCode = 2600,
                    Gender = "Female",
                }
            });

            await _context.SaveChangesAsync();

            // Act
            var result = await _paymentRepository.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Payment>>(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async void GetAll_ShouldReturnEmptyListOfPayments_WhenNoPaymentsExisted()
        {
            // Arrange  
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _paymentRepository.GetAll();

            // Assert 
            Assert.NotNull(result);
            Assert.IsType<List<Payment>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public async void GetById_ShouldReturnPayment_WhenPaymentExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int paymentId = 1;

            _context.Payment.Add(new()
            {
                PaymentID = paymentId,
                Customer = new Customer
                {
                    CustomerID = 1,
                    Account = new Account { AccountID = 1 },
                    FirstName = "Matthias",
                    LastName = "Bryde",
                    PhoneNumber = "42483787",
                    Country = new Country { CountryID = 1, CountryName = "Danmark" },
                    ZipCode = 2620,
                    Gender = "Male",
                }
            });

            await _context.SaveChangesAsync();

            // Act
            var result = await _paymentRepository.GetById(paymentId);


            // Assert
            Assert.NotNull(result);
            Assert.IsType<Payment>(result);
            Assert.Equal(paymentId, result.PaymentID);
        }

        [Fact]
        public async void GetById_ShouldReturnNull_WhenPaymentDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _paymentRepository.GetById(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void Create_ShouldAddNewIdToPayment_WhenSavingToDatabase()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int expectedId = 1;

            Payment payment = new()
            {
                Customer = new Customer
                {
                    CustomerID = 1,
                    Account = new Account { AccountID = 1 },
                    FirstName = "Matthias",
                    LastName = "Bryde",
                    PhoneNumber = "42483787",
                    Country = new Country { CountryID = 1, CountryName = "Danmark" },
                    ZipCode = 2620,
                    Gender = "Male",
                }
            };

            // Act
            var result = await _paymentRepository.Create(payment);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Payment>(result);
            Assert.Equal(expectedId, result.PaymentID);
        }

        [Fact]
        public async void Create_ShouldFailToAddNewIdToPayment_WhenPaymentIdAlreadyExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            Payment payment = new()
            {
                Customer = new Customer
                {
                    CustomerID = 1,
                    Account = new Account { AccountID = 1 },
                    FirstName = "Matthias",
                    LastName = "Bryde",
                    PhoneNumber = "42483787",
                    Country = new Country { CountryID = 1, CountryName = "Danmark" },
                    ZipCode = 2620,
                    Gender = "Male",
                }
            };

            // Act
            var result = await _paymentRepository.Create(payment);

            async Task action() => await _paymentRepository.Create(payment);

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(action);
            Assert.Contains("An item with the same key has already been added", ex.Message);
        }

        [Fact]
        public async void Update_ShouldChangeValuesOnPayment_WhenPaymentExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int paymentId = 1;

            Payment newPayment = new()
            {
                PaymentID = 1,
                Customer = new Customer
                {
                    CustomerID = 1,
                    Account = new Account { AccountID = 1 },
                    FirstName = "Matthias",
                    LastName = "Bryde",
                    PhoneNumber = "42483787",
                    Country = new Country { CountryID = 1, CountryName = "Danmark" },
                    ZipCode = 2620,
                    Gender = "Male",
                }
            };

            _context.Payment.Add(newPayment);
            await _context.SaveChangesAsync();

            Payment updatePayment = new()
            {
                PaymentID = paymentId,
                Customer = new Customer
                {
                    CustomerID = 2,
                    Account = new Account { AccountID = 2 },
                    FirstName = "Alexander",
                    LastName = "Eriksen",
                    PhoneNumber = "12345678",
                    Country = new Country { CountryID = 2, CountryName = "Sverige" },
                    ZipCode = 2600,
                    Gender = "Female",
                }
            };

            // Act
            var result = await _paymentRepository.Update(paymentId, updatePayment);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Payment>(result);
            Assert.Equal(paymentId, result.PaymentID);
            Assert.Equal(updatePayment.CustomerID, result.CustomerID);
        }

        [Fact]
        public async void Update_ShouldreturnNull_WhenPaymentDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int paymentId = 1;

            Payment updatePayment = new()
            {
                PaymentID = 1,
                Customer = new Customer
                {
                    CustomerID = 1,
                    Account = new Account { AccountID = 1 },
                    FirstName = "Matthias",
                    LastName = "Bryde",
                    PhoneNumber = "42483787",
                    Country = new Country { CountryID = 1, CountryName = "Danmark" },
                    ZipCode = 2620,
                    Gender = "Male",
                }
            };

            // Act
            var result = await _paymentRepository.Update(paymentId, updatePayment);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void Delete_ShouldReturnDeletedPayment_WhenPaymentIsDeleted()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int paymentId = 1;

            Payment newPayment = new()
            {
                PaymentID = 1,
                Customer = new Customer
                {
                    CustomerID = 1,
                    Account = new Account { AccountID = 1 },
                    FirstName = "Matthias",
                    LastName = "Bryde",
                    PhoneNumber = "42483787",
                    Country = new Country { CountryID = 1, CountryName = "Danmark" },
                    ZipCode = 2620,
                    Gender = "Male",
                }
            };

            _context.Payment.Add(newPayment);
            await _context.SaveChangesAsync();

            // Act
            var result = await _paymentRepository.Delete(paymentId);
            var payment = await _paymentRepository.GetById(paymentId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Payment>(result);
            Assert.Equal(paymentId, result.PaymentID);

            Assert.Null(payment);
        }

        [Fact]
        public async void Delete_ShouldReturnNull_WhenPaymentDoesNotExist()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _paymentRepository.Delete(1);

            // Assert
            Assert.Null(result);
        }
    }
}
