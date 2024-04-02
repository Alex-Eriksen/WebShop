using Microsoft.Extensions.Options;
using System.Net;
using WebShop_API.DTOs.Authentication;
using BC = BCrypt.Net.BCrypt;
namespace WebShopUnitTests.Repository
{
    public class AccountRepositoryTests
    {
        private readonly DbContextOptions<DatabaseContext> _options;
        private readonly DatabaseContext m_context;
        private readonly AccountRepository m_accountRepository;
        private readonly Mock<IOptions<AppSettings>> m_appSettingsMock = new Mock<IOptions<AppSettings>>();

        public AccountRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "AccountRepository")
                .Options;

            m_appSettingsMock
                .SetupGet( x => x.Value )
                .Returns( new AppSettings() { Secret = "12345678912345678912345678912345" } );

            m_context = new(_options);

            m_accountRepository = new(m_context, m_appSettingsMock.Object);
        }

        [Fact]
        public async void GetById_ShouldReturnAccount_WhenAccountExists()
        {
            // Arrange
            await m_context.Database.EnsureDeletedAsync();

            int accountId = 1;

            m_context.Account.Add(new()
            {
                 AccountID = accountId,
                 Username = "User-1",
                 Password = "Pass-1",
                 Customer = new()
                 {
                     CustomerID = 1,
                     Account = new Account { AccountID = 1 },
                     FirstName = "Matthias",
                     LastName = "Bryde",
                     PhoneNumber = "42483787",
                     Country = new Country { CountryID = 1, CountryName = "Danmark" },
                     ZipCode = 2620,
                     Gender = "Male",
                 },
                 Role = "Customer"
            });

            await m_context.SaveChangesAsync();

            // Act
            var result = await m_accountRepository.GetById(accountId);

            // Assert
            Assert.NotNull( result );
            Assert.IsType<Account>( result );
            Assert.Equal( accountId, result.AccountID );
        }

        [Fact]
        public async void GetById_ShouldReturnNull_WhenAccountDoesNotExist()
        {
            // Arrange
            await m_context.Database.EnsureDeletedAsync();

            int accountId = 1;

            // Act
            var result = await m_accountRepository.GetById( accountId );

            // Assert
            Assert.Null( result );
        }

        [Fact]
        public async void Create_ShouldReturnAccount_WhenCreatedSuccessfully()
        {
            // Arrange
            await m_context.Database.EnsureDeletedAsync();

            int accountId = 1;

            Account account = new()
            {
                AccountID = accountId,
                Username = "User-1",
                Password = "Pass-1",
                Customer = new()
                {
                    CustomerID = 1,
                    Account = new Account { AccountID = 1 },
                    FirstName = "Matthias",
                    LastName = "Bryde",
                    PhoneNumber = "42483787",
                    Country = new Country { CountryID = 1, CountryName = "Danmark" },
                    ZipCode = 2620,
                    Gender = "Male"
                },
                Role = "Customer"
            };

            // Act
            var result = await m_accountRepository.Create( account );

            // Assert
            Assert.NotNull( result );
            Assert.IsType<Account>( result );
            Assert.Equal( accountId, result.AccountID );
        }

        [Fact]
        public async void Create_ShouldFailToCreateAccount_WhenAccountAlreadyExists()
        {
            // Arrange
            await m_context.Database.EnsureDeletedAsync();

            Account account = new()
            {
                AccountID = 1,
                Username = "User-1",
                Password = "Pass-1",
                Customer = new()
                {
                    CustomerID = 1,
                    Account = new Account { AccountID = 1 },
                    FirstName = "Matthias",
                    LastName = "Bryde",
                    PhoneNumber = "42483787",
                    Country = new Country { CountryID = 1, CountryName = "Danmark" },
                    ZipCode = 2620,
                    Gender = "Male"
                },
                Role = "Customer"
            };

            // Act
            var result = await m_accountRepository.Create( account );

            async Task action() => await m_accountRepository.Create( account );

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>( action );
            Assert.Contains( "An item with the same key has already been added", ex.Message );
        }

        [Fact]
        public async void Update_ShouldChangeValuesOnAccount_WhenAccountExists()
        {
            // Arrange
            await m_context.Database.EnsureDeletedAsync();

            int accountId = 1;

            Account account = new()
            {
                AccountID = accountId,
                Username = "User-1",
                Password = "Pass-1",
                Customer = new()
                {
                    CustomerID = 1,
                    Account = new Account { AccountID = 1 },
                    FirstName = "Matthias",
                    LastName = "Bryde",
                    PhoneNumber = "42483787",
                    Country = new Country { CountryID = 1, CountryName = "Danmark" },
                    ZipCode = 2620,
                    Gender = "Male"
                },
                Role = "Customer"
            };

            m_context.Account.Add( account );
            await m_context.SaveChangesAsync();

            Account updateAccount = new()
            {
                AccountID = accountId,
                Username = "new User-1",
                Password = "new Pass-1",
                Email = "test@test.com",
                Role = "Admin"
            };

            // Act
            var result = await m_accountRepository.Update( accountId, updateAccount );

            // Assert
            Assert.NotNull( result );
            Assert.IsType<Account>( result );
            Assert.Equal( updateAccount.Username, result.Username );
            Assert.Equal( updateAccount.Password, result.Password );
            Assert.Equal( updateAccount.Email, result.Email );
            Assert.Equal( updateAccount.Role, result.Role );
        }

        [Fact]
        public async void Update_ShouldReturnNull_WhenAccountDoesNotExist()
        {
            // Arrange
            await m_context.Database.EnsureDeletedAsync();

            int accountId = 1;

            Account account = new()
            {
                AccountID = accountId,
                Username = "User-1",
                Password = "Pass-1",
                Customer = new()
                {
                    CustomerID = 1,
                    Account = new Account { AccountID = 1 },
                    FirstName = "Matthias",
                    LastName = "Bryde",
                    PhoneNumber = "42483787",
                    Country = new Country { CountryID = 1, CountryName = "Danmark" },
                    ZipCode = 2620,
                    Gender = "Male"
                },
                Role = "Customer"
            };

            // Act
            var result = await m_accountRepository.Update( accountId, account );

            // Assert
            Assert.Null( result );
        }

        [Fact]
        public async void Authenticate_ShouldReturnAuthenticationResponse_WhenCredentialsIsVerified()
        {
            // Arrange
            await m_context.Database.EnsureDeletedAsync();

            string username = "User-1";
            string password = "Pass-1";
            string ipAddress = "127.0.0.1";

            Account account = new()
            {
                AccountID = 1,
                Username = username,
                Password = BC.HashPassword( password ),
                Customer = new()
                {
                    CustomerID = 1,
                    Account = new Account { AccountID = 1 },
                    FirstName = "Matthias",
                    LastName = "Bryde",
                    PhoneNumber = "42483787",
                    Country = new Country { CountryID = 1, CountryName = "Danmark" },
                    ZipCode = 2620,
                    Gender = "Male"
                },
                Role = "Customer",
                RefreshTokens = new List<RefreshToken>()
            };

            m_context.Account.Add( account );
            await m_context.SaveChangesAsync();

            // Act
            var result = await m_accountRepository.Authenticate( username, password, ipAddress );

            // Assert
            Assert.NotNull( result );
            Assert.IsType<AuthenticationResponse>( result );
            Assert.NotEmpty( account.RefreshTokens );
        }

        [Fact]
        public async void Authenticate_ShouldReturnNull_WhenCredentialsAreNotVerified()
        {
            // Arrange
            await m_context.Database.EnsureDeletedAsync();

            string username = "User-1";
            string password = "Pass-1";
            string ipAddress = "127.0.0.1";

            Account account = new()
            {
                AccountID = 1,
                Username = username,
                Password = BC.HashPassword( password ),
                Customer = new()
                {
                    CustomerID = 1,
                    Account = new Account { AccountID = 1 },
                    FirstName = "Matthias",
                    LastName = "Bryde",
                    PhoneNumber = "42483787",
                    Country = new Country { CountryID = 1, CountryName = "Danmark" },
                    ZipCode = 2620,
                    Gender = "Male"
                },
                Role = "Customer",
                RefreshTokens = new List<RefreshToken>()
            };

            m_context.Account.Add( account );
            await m_context.SaveChangesAsync();

            // Act
            var result_pass = await m_accountRepository.Authenticate( username, "IncorrectPassword", ipAddress );
            var result_user = await m_accountRepository.Authenticate( "IncorrectUsername", password, ipAddress );

            // Assert
            Assert.Null( result_pass );
            Assert.Null( result_user );
        }

        [Fact]
        public async void RefreshToken_ShouldReturnAuthenticationResponse_WhenRefreshTokenWasCreated()
        {
            // Arrange
            await m_context.Database.EnsureDeletedAsync();

            string ipAddress = "127.0.0.1";

            RefreshToken refreshToken = JWTHandler.GenerateRefreshToken( ipAddress );
            
            Account account = new()
            {
                AccountID = 1,
                Username = "User-1",
                Password = BC.HashPassword( "Pass-1" ),
                Customer = new()
                {
                    CustomerID = 1,
                    Account = new Account { AccountID = 1 },
                    FirstName = "Matthias",
                    LastName = "Bryde",
                    PhoneNumber = "42483787",
                    Country = new Country { CountryID = 1, CountryName = "Danmark" },
                    ZipCode = 2620,
                    Gender = "Male"
                },
                Role = "Customer",
                RefreshTokens = new List<RefreshToken>() { refreshToken }
            };

            m_context.Account.Add( account );
            await m_context.SaveChangesAsync();

            // Act
            var result = await m_accountRepository.RefreshToken( refreshToken.Token, ipAddress );

            // Assert
            Assert.NotNull( result );
            Assert.IsType<AuthenticationResponse>( result );
            Assert.NotEqual( refreshToken.Token, result.RefreshToken );
        }

        [Fact]
        public async void RefreshToken_ShouldReturnNull_WhenRefreshTokenIsNotActive()
        {
            // Arrange
            await m_context.Database.EnsureDeletedAsync();

            string ipAddress = "127.0.0.1";

            RefreshToken refreshToken = JWTHandler.GenerateRefreshToken( ipAddress );
            refreshToken.Revoked_At = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;

            Account account = new()
            {
                AccountID = 1,
                Username = "User-1",
                Password = BC.HashPassword( "Pass-1" ),
                Customer = new()
                {
                    CustomerID = 1,
                    Account = new Account { AccountID = 1 },
                    FirstName = "Matthias",
                    LastName = "Bryde",
                    PhoneNumber = "42483787",
                    Country = new Country { CountryID = 1, CountryName = "Danmark" },
                    ZipCode = 2620,
                    Gender = "Male"
                },
                Role = "Customer",
                RefreshTokens = new List<RefreshToken>() { refreshToken }
            };

            m_context.Account.Add( account );
            await m_context.SaveChangesAsync();

            // Act
            var result = await m_accountRepository.RefreshToken( refreshToken.Token, ipAddress );

            // Assert
            Assert.Null( result );
        }

        [Fact]
        public async void RevokeToken_ShouldReturnTrue_WhenRefreshTokenIsRevoked()
        {
            // Arrange
            await m_context.Database.EnsureDeletedAsync();

            string ipAddress = "127.0.0.1";

            RefreshToken refreshToken = JWTHandler.GenerateRefreshToken( ipAddress );

            Account account = new()
            {
                AccountID = 1,
                Username = "User-1",
                Password = BC.HashPassword( "Pass-1" ),
                Customer = new()
                {
                    CustomerID = 1,
                    Account = new Account { AccountID = 1 },
                    FirstName = "Matthias",
                    LastName = "Bryde",
                    PhoneNumber = "42483787",
                    Country = new Country { CountryID = 1, CountryName = "Danmark" },
                    ZipCode = 2620,
                    Gender = "Male"
                },
                Role = "Customer",
                RefreshTokens = new List<RefreshToken>() { refreshToken }
            };

            m_context.Account.Add( account );
            await m_context.SaveChangesAsync();

            // Act
            var result = await m_accountRepository.RevokeToken( refreshToken.Token, ipAddress );

            // Assert
            Assert.True( result );
            Assert.IsType<bool>( result );
        }

        [Fact]
        public async void RevokeToken_ShouldReturnFalse_WhenTokenIsAlreadyRevoked()
        {
            // Arrange
            await m_context.Database.EnsureDeletedAsync();

            string ipAddress = "127.0.0.1";

            RefreshToken refreshToken = JWTHandler.GenerateRefreshToken( ipAddress );
            refreshToken.Revoked_At = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;

            Account account = new()
            {
                AccountID = 1,
                Username = "User-1",
                Password = BC.HashPassword( "Pass-1" ),
                Customer = new()
                {
                    CustomerID = 1,
                    Account = new Account { AccountID = 1 },
                    FirstName = "Matthias",
                    LastName = "Bryde",
                    PhoneNumber = "42483787",
                    Country = new Country { CountryID = 1, CountryName = "Danmark" },
                    ZipCode = 2620,
                    Gender = "Male"
                },
                Role = "Customer",
                RefreshTokens = new List<RefreshToken>() { refreshToken }
            };

            m_context.Account.Add( account );
            await m_context.SaveChangesAsync();

            // Act
            var result = await m_accountRepository.RevokeToken( refreshToken.Token, ipAddress );

            // Assert
            Assert.False( result );
        }
    }
}
