

using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace WebShopUnitTests.Controllers
{
    public class CustomerControllerTests
    {
        private readonly CustomerController m_customerController;
        private readonly Mock<ICustomerService> m_costumerServiceMock = new();

        public CustomerControllerTests()
        {
            m_customerController = new(m_costumerServiceMock.Object);
        }

        [Fact]
        public async void GetAll_ShouldReturnStatusCode200_WhenCustomersExists()
        {
            List<StaticCustomerResponse> customers = new();

            customers.Add(new StaticCustomerResponse
            {
                CustomerID = 1,
                FirstName = "Matthias",
                LastName = "Bryde",
                PhoneNumber = "42483787",
                Gender = "Male"
            });

            m_costumerServiceMock.Setup(x => x.GetAll()).ReturnsAsync(customers);

            var result = (IStatusCodeActionResult) await m_customerController.GetAll();

            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async void GetAll_ShouldReturnStatusCode204_WhenNoCustomersExists()
        {
            List<StaticCustomerResponse> cutomers = new();

            m_costumerServiceMock.Setup(x => x.GetAll()).ReturnsAsync(cutomers);

            var result = (IStatusCodeActionResult)await m_customerController.GetAll();

            Assert.Equal(204, result.StatusCode);
        }

        [Fact]
        public async void GetAll_ShouldReturnStatusCode500_WhenServiceReturnsNull()
        {
            // Arrange
            m_costumerServiceMock
                .Setup( x => x.GetAll() )
                .ReturnsAsync( () => null );

            // Act
            var result = (IStatusCodeActionResult) await m_customerController.GetAll();

            // Assert
            Assert.Equal( 500, result.StatusCode );
        }

        [Fact]
        public async void GetAll_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            m_costumerServiceMock
                .Setup(x => x.GetAll())
                .ReturnsAsync( () => throw new Exception() );

            // Act
            var result = (IStatusCodeActionResult) await m_customerController.GetAll();

            // Assert
            Assert.Equal( 500, result.StatusCode );
        }

        [Fact]
        public async void GetById_ShouldReturnStatusCode200_WhenCustomerExists()
        {
            // Arrange
            int customerId = 1;
            
            DirectCustomerResponse response = new()
            {
                CustomerID = customerId,
                FirstName = "Alexander",
                LastName = "Eriksen",
                Gender = "Male",
                PhoneNumber = "11223344",
                ZipCode = 3300,
                Account = new StaticAccountResponse()
                { AccountID = 1, Email = "test@test.com", Role = "Customer", Username = "alex" },
                Country = new StaticCountryResponse()
                { CountryID = 1, CountryName = "Danmark" },
                Created_At = DateTime.UtcNow,
                Orders = new List<StaticOrderResponse>(),
                Payments = new List<StaticPaymentResponse>()
            };

            m_costumerServiceMock
                .Setup( x => x.GetById( It.IsAny<int>() ) )
                .ReturnsAsync( response );

            // Act
            var result = (IStatusCodeActionResult) await m_customerController.GetById( customerId );

            // Assert
            Assert.Equal( 200, result.StatusCode );
        }

        [Fact]
        public async void GetById_ShouldReturnStatusCode404_WhenCustomerDoesNotExist()
        {
            //Arrange
            int customerId = 1;

            m_costumerServiceMock
                .Setup( x => x.GetById( It.IsAny<int>() ) )
                .ReturnsAsync( () => null );

            //Act
            var result = (IStatusCodeActionResult) await m_customerController.GetById( customerId );

            //Assert
            Assert.Equal( 404, result.StatusCode );
        }

        [Fact]
        public async void GetById_ShouldReturnStatusCode500_WhenExectionIsRaised()
        {
            //Arrange
            int customerId = 1;

            m_costumerServiceMock
                .Setup( x => x.GetById( It.IsAny<int>() ) )
                .ReturnsAsync( () => throw new Exception() );

            //Act
            var result = (IStatusCodeActionResult) await m_customerController.GetById( customerId );

            //Assert
            Assert.Equal( 500, result.StatusCode );
        }

        [Fact]
        public async void Create_ShouldReturnStatusCode200_WhenCustomerIsSuccessfullyCreated()
        {
            //Arrange
            NewCustomerRequest request = new() { Account = new AccountRequest(), Customer = new CustomerRequest() };

            int customerId = 1;

            DirectCustomerResponse response = new()
            {
                CustomerID = customerId,
                FirstName = "Alexander",
                LastName = "Eriksen",
                Gender = "Male",
                PhoneNumber = "11223344",
                ZipCode = 3300,
                Account = new StaticAccountResponse()
                { AccountID = 1, Email = "test@test.com", Role = "Customer", Username = "alex" },
                Country = new StaticCountryResponse()
                { CountryID = 1, CountryName = "Danmark" },
                Created_At = DateTime.UtcNow,
                Orders = new List<StaticOrderResponse>(),
                Payments = new List<StaticPaymentResponse>()
            };

            m_costumerServiceMock
                .Setup( x => x.Create( It.IsAny<NewCustomerRequest>() ) )
                .ReturnsAsync( response );

            //Act
            var result = (IStatusCodeActionResult) await m_customerController.Create( request );

            //Assert
            Assert.Equal( 200, result.StatusCode );
        }

        [Fact]
        public async void Create_ShouldReturnStatusCode500_WhenServiceReturnsNull()
        {
            //Arrange
            NewCustomerRequest request = new()
            {
                Account = new AccountRequest(),
                Customer = new CustomerRequest()
            };

            m_costumerServiceMock
                .Setup( x => x.Create( It.IsAny<NewCustomerRequest>() ) )
                .ReturnsAsync( () => null );

            //Act
            var result = (IStatusCodeActionResult) await m_customerController.Create( request );

            //Assert
            Assert.Equal( 500, result.StatusCode );
        }
        
        [Fact]
        public async void Create_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            //Arrange
            NewCustomerRequest request = new()
            {
                
            };

            m_costumerServiceMock
                .Setup( x => x.Create( It.IsAny<NewCustomerRequest>() ) )
                .ReturnsAsync( () => throw new Exception() );

            //Act
            var result = (IStatusCodeActionResult) await m_customerController.Create( request );

            //Assert
            Assert.Equal( 500, result.StatusCode );
        }

        [Fact]
        public async void Update_ShouldReturnStatusCode200_WhenUpdateIsSuccessfullyUpdated()
        {
            //Arrange
            NewCustomerRequest request = new();

            int customerId = 1;

            DirectCustomerResponse response = new()
            {
                CustomerID = customerId,
                FirstName = "Alexander",
                LastName = "Eriksen",
                Gender = "Male",
                PhoneNumber = "11223344",
                ZipCode = 3300,
                Account = new StaticAccountResponse()
                { AccountID = 1, Email = "test@test.com", Role = "Customer", Username = "alex" },
                Country = new StaticCountryResponse()
                { CountryID = 1, CountryName = "Danmark" },
                Created_At = DateTime.UtcNow,
                Orders = new List<StaticOrderResponse>(),
                Payments = new List<StaticPaymentResponse>()
            };

            m_costumerServiceMock
                .Setup( x => x.Update( It.IsAny<int>(), It.IsAny<NewCustomerRequest>() ) )
                .ReturnsAsync( response );

            //Act
            var result = (IStatusCodeActionResult) await m_customerController.Update( customerId, request );

            //Assert
            Assert.Equal( 200, result.StatusCode );
        }

        [Fact]
        public async void Update_ShouldReturnStatusCode404_WhenCustomerDoesNotExist()
        {
            //Arrange
            NewCustomerRequest request = new();

            int customerId = 1;

            m_costumerServiceMock
                .Setup( x => x.Update( It.IsAny<int>(), It.IsAny<NewCustomerRequest>() ) )
                .ReturnsAsync( () => null );

            //Act
            var result = (IStatusCodeActionResult) await m_customerController.Update( customerId, request );

            //Assert
            Assert.Equal( 404, result.StatusCode );
        }        
        
        [Fact]
        public async void Update_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            //Arrange
            NewCustomerRequest request = new();

            int customerId = 1;

            m_costumerServiceMock
                .Setup( x => x.Update( It.IsAny<int>(), It.IsAny<NewCustomerRequest>() ) )
                .ReturnsAsync( () => throw new Exception() );

            //Act
            var result = (IStatusCodeActionResult) await m_customerController.Update( customerId, request );

            //Assert
            Assert.Equal( 500, result.StatusCode );
        }

        [Fact]
        public async void Delete_ShouldReturnStatusCode200_WhenCustomerIsSuccessfullyDeleted()
        {
            //Arrange
            int customerId = 1;

            DirectCustomerResponse response = new()
            {
                CustomerID = customerId,
                FirstName = "Alexander",
                LastName = "Eriksen",
                Gender = "Male",
                PhoneNumber = "11223344",
                ZipCode = 3300,
                Account = new StaticAccountResponse()
                { AccountID = 1, Email = "test@test.com", Role = "Customer", Username = "alex" },
                Country = new StaticCountryResponse()
                { CountryID = 1, CountryName = "Danmark" },
                Created_At = DateTime.UtcNow,
                Orders = new List<StaticOrderResponse>(),
                Payments = new List<StaticPaymentResponse>()
            };

            m_costumerServiceMock
                .Setup( x => x.Delete( It.IsAny<int>() ) )
                .ReturnsAsync( response );

            //Act
            var result = (IStatusCodeActionResult) await m_customerController.Delete( customerId );

            //Assert
            Assert.Equal( 200, result.StatusCode );
        }

        [Fact]
        public async void Delete_ShouldReturnStatusCode404_WhenCustomerDoesNotExist()
        {
            //Arrange
            int customerId = 1;

            m_costumerServiceMock
                .Setup( x => x.Delete( It.IsAny<int>() ) )
                .ReturnsAsync( () => null );

            //Act
            var result = (IStatusCodeActionResult) await m_customerController.Delete( customerId );

            //Assert
            Assert.Equal( 404, result.StatusCode );
        }

        [Fact]
        public async void Delete_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            //Arrange
            int customerId = 1;

            m_costumerServiceMock
                .Setup( x => x.Delete( It.IsAny<int>() ) )
                .ReturnsAsync( () => throw new Exception() );

            //Act
            var result = (IStatusCodeActionResult) await m_customerController.Delete( customerId );

            //Assert
            Assert.Equal( 500, result.StatusCode );
        }

        [Fact]
        public async void CreatePayment_ShouldReturnStatusCode200_WhenPaymentIsSuccessfullyCreated()
        {
            // Arrange
            PaymentRequest request = new()
            {
                CardNumber = "1234123412341234",
                CustomerID = 1,
                Expiry = DateTime.UtcNow,
                PaymentType = "Card",
                Provider = "VISA"
            };

            DirectPaymentResponse response = new()
            {
                PaymentID = 1,
                CardNumber = "1234123412341234",
                Customer = new StaticCustomerResponse() { CustomerID = 1 },
                Expiry = DateTime.UtcNow,
                PaymentType = "Card",
                Provider = "VISA",
                Created_At = DateTime.UtcNow
            };

            m_costumerServiceMock
                .Setup( x => x.CreatePayment( It.IsAny<PaymentRequest>() ) )
                .ReturnsAsync( response );

            // Act
            var result = (IStatusCodeActionResult) await m_customerController.CreatePayment( request );

            // Assert
            Assert.Equal( 200, result.StatusCode );
        }

        [Fact]
        public async void CreatePayment_ShouldReturnStatusCode500_WhenServiceReturnsNull()
        {
            // Arrange
            PaymentRequest request = new()
            {
                CardNumber = "1234123412341234",
                CustomerID = 1,
                Expiry = DateTime.UtcNow,
                PaymentType = "Card",
                Provider = "VISA"
            };

            m_costumerServiceMock
                .Setup( x => x.CreatePayment( It.IsAny<PaymentRequest>() ) )
                .ReturnsAsync( () => null );

            // Act
            var result = (IStatusCodeActionResult) await m_customerController.CreatePayment( request );

            // Assert
            Assert.Equal( 500, result.StatusCode );
        }

        [Fact]
        public async void CreatePayment_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            PaymentRequest request = new()
            {
                CardNumber = "1234123412341234",
                CustomerID = 1,
                Expiry = DateTime.UtcNow,
                PaymentType = "Card",
                Provider = "VISA"
            };

            m_costumerServiceMock
                .Setup( x => x.CreatePayment( It.IsAny<PaymentRequest>() ) )
                .ReturnsAsync( () => throw new Exception() );

            // Act
            var result = (IStatusCodeActionResult) await m_customerController.CreatePayment( request );

            // Assert
            Assert.Equal( 500, result.StatusCode );
        }

        [Fact]
        public async void UpdatePayment_ShouldReturnStatusCode200_WhenPaymentIsSuccessfullyUpdated()
        {
            // Arrange
            int paymentId = 1;

            PaymentRequest request = new()
            {
                CardNumber = "1234123412341234",
                CustomerID = 1,
                Expiry = DateTime.UtcNow,
                PaymentType = "Card",
                Provider = "VISA"
            };

            DirectPaymentResponse response = new()
            {
                PaymentID = 1,
                CardNumber = "1234123412341234",
                Customer = new StaticCustomerResponse() { CustomerID = 1 },
                Expiry = DateTime.UtcNow,
                PaymentType = "Card",
                Provider = "VISA",
                Created_At = DateTime.UtcNow
            };

            m_costumerServiceMock
                .Setup( x => x.UpdatePayment( It.IsAny<int>(), It.IsAny<PaymentRequest>() ) )
                .ReturnsAsync( response );

            // Act
            var result = (IStatusCodeActionResult) await m_customerController.UpdatePayment( paymentId, request );

            // Assert
            Assert.Equal( 200, result.StatusCode );
        }

        [Fact]
        public async void UpdatePayment_ShouldReturnStatusCode500_WhenServiceReturnsNull()
        {
            // Arrange
            int paymentId = 1;

            PaymentRequest request = new()
            {
                CardNumber = "1234123412341234",
                CustomerID = 1,
                Expiry = DateTime.UtcNow,
                PaymentType = "Card",
                Provider = "VISA"
            };

            m_costumerServiceMock
                .Setup( x => x.UpdatePayment( It.IsAny<int>(), It.IsAny<PaymentRequest>() ) )
                .ReturnsAsync( () => null );

            // Act
            var result = (IStatusCodeActionResult) await m_customerController.UpdatePayment( paymentId, request );

            // Assert
            Assert.Equal( 500, result.StatusCode );
        }

        [Fact]
        public async void UpdatePayment_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            int paymentId = 1;

            PaymentRequest request = new()
            {
                CardNumber = "1234123412341234",
                CustomerID = 1,
                Expiry = DateTime.UtcNow,
                PaymentType = "Card",
                Provider = "VISA"
            };

            m_costumerServiceMock
                .Setup( x => x.UpdatePayment( It.IsAny<int>(), It.IsAny<PaymentRequest>() ) )
                .ReturnsAsync( () => throw new Exception() );

            // Act
            var result = (IStatusCodeActionResult) await m_customerController.UpdatePayment( paymentId, request );

            // Assert
            Assert.Equal( 500, result.StatusCode );
        }

        [Fact]
        public async void DeletePayment_ShouldReturnStatusCode200_WhenPaymentIsSuccessfullyDeleted()
        {
            // Arrange
            int paymentId = 1;

            DirectPaymentResponse response = new()
            {
                PaymentID = 1,
                CardNumber = "1234123412341234",
                Customer = new StaticCustomerResponse() { CustomerID = 1 },
                Expiry = DateTime.UtcNow,
                PaymentType = "Card",
                Provider = "VISA",
                Created_At = DateTime.UtcNow
            };

            m_costumerServiceMock
                .Setup( x => x.DeletePayment( It.IsAny<int>() ) )
                .ReturnsAsync( response );

            // Act
            var result = (IStatusCodeActionResult) await m_customerController.DeletePayment( paymentId );

            // Assert
            Assert.Equal( 200, result.StatusCode );
        }

        [Fact]
        public async void DeletePayment_ShouldReturnStatusCode500_WhenServiceReturnsNull()
        {
            // Arrange
            int paymentId = 1;

            m_costumerServiceMock
                .Setup( x => x.DeletePayment( It.IsAny<int>() ) )
                .ReturnsAsync( () => null );

            // Act
            var result = (IStatusCodeActionResult) await m_customerController.DeletePayment( paymentId );

            // Assert
            Assert.Equal( 500, result.StatusCode );
        }

        [Fact]
        public async void DeletePayment_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            int paymentId = 1;

            m_costumerServiceMock
                .Setup( x => x.DeletePayment( It.IsAny<int>() ) )
                .ReturnsAsync( () => throw new Exception() );

            // Act
            var result = (IStatusCodeActionResult) await m_customerController.DeletePayment( paymentId );

            // Assert
            Assert.Equal( 500, result.StatusCode );
        }

        [Fact]
        public async void GetPayment_ShouldReturnStatusCode200_WhenPaymentExists()
        {
            // Arrange
            int paymentId = 1;

            DirectPaymentResponse response = new()
            {
                PaymentID = 1,
                CardNumber = "1234123412341234",
                Customer = new StaticCustomerResponse() { CustomerID = 1 },
                Expiry = DateTime.UtcNow,
                PaymentType = "Card",
                Provider = "VISA",
                Created_At = DateTime.UtcNow
            };

            m_costumerServiceMock
                .Setup( x => x.GetPayment( It.IsAny<int>() ) )
                .ReturnsAsync( response );

            // Act
            var result = (IStatusCodeActionResult) await m_customerController.GetPayment( paymentId );

            // Assert
            Assert.Equal( 200, result.StatusCode );
        }

        [Fact]
        public async void GetPayment_ShouldReturnStatusCode404_WhenPaymentDoesNotExist()
        {
            // Arrange
            int paymentId = 1;

            m_costumerServiceMock
                .Setup( x => x.GetPayment( It.IsAny<int>() ) )
                .ReturnsAsync( () => null );

            // Act
            var result = (IStatusCodeActionResult) await m_customerController.GetPayment( paymentId );

            // Assert
            Assert.Equal( 404, result.StatusCode );
        }

        [Fact]
        public async void GetPayment_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            int paymentId = 1;

            m_costumerServiceMock
                .Setup( x => x.GetPayment( It.IsAny<int>() ) )
                .ReturnsAsync( () => throw new Exception() );

            // Act
            var result = (IStatusCodeActionResult) await m_customerController.GetPayment( paymentId );

            // Assert
            Assert.Equal( 500, result.StatusCode );
        }
    }
}
