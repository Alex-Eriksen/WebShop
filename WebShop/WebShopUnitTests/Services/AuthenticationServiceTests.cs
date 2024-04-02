using Microsoft.AspNetCore.Routing.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebShop_API.Database.Entities;
using BC = BCrypt.Net.BCrypt;

namespace WebShopUnitTests.Services
{
    public class AuthenticationServiceTests
    {
        private readonly AuthenticationService m_authenticationService;
        private readonly Mock<IAccountRepository> m_accountRepositoryMock = new();
        private readonly IMapper m_mapper;

        public AuthenticationServiceTests()
        {
            if (m_mapper == null)
            {
                var mapperConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new WebShop_API.Helpers.AutoMapper.AutoMapper());
                });
                IMapper mapper = mapperConfig.CreateMapper();
                m_mapper = mapper;
            }
            m_authenticationService = new(m_accountRepositoryMock.Object, m_mapper);
        }

        [Fact]
        public async void Authenticate_ShouldReturnAuthenticateResponse_WhenAuthenticationIsSuccessful()
        {
            // Arrange
            string ipAddress = "127.0.0.1";

            Account account = new()
            {
                AccountID = 1,
                Username = "Nicklas",
                Password = BC.HashPassword("Nicklas"),
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

            AuthenticationRequest request = new()
            {
                Username_Email = "Nicklas",
                Password = "Nicklas"
            };

            RefreshToken refreshToken = JWTHandler.GenerateRefreshToken(ipAddress);
            string accessToken = JWTHandler.GenerateJWTToken(account, new AppSettings { Secret = "12345678912345678912345678912345" });

            AuthenticationResponse response = new(refreshToken.Token, accessToken);

            m_accountRepositoryMock
                .Setup(x => x.Authenticate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(response);

            // Act
            var result = await m_authenticationService.Authenticate(request, ipAddress);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async void Authenticate_ShouldReturnNull_WhenAuthenticationCredentialsAreInvalid()
        {
            // Arrange
            string ipAddress = "127.0.0.1";

            AuthenticationRequest request = new()
            {
                Username_Email = "Nicklas",
                Password = "Nickla"
            };

            m_accountRepositoryMock
                .Setup(x => x.Authenticate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await m_authenticationService.Authenticate(request, ipAddress);

            // Assert
            Assert.Null(result);

        }

        [Fact]
        public async void RefreshToken_ShouldReturnStaticRefreshTokenResponse_WhenTokenIsValid()
        {
            // Arrange
            string ipAddress = "127.0.0.1";

            Account account = new()
            {
                AccountID = 1,
                Username = "Nicklas",
                Password = BC.HashPassword("Nicklas"),
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

            RefreshToken refreshToken = JWTHandler.GenerateRefreshToken(ipAddress);

            string accessToken = JWTHandler.GenerateJWTToken(account, new AppSettings { Secret = "12345678912345678912345678912345" });

            AuthenticationResponse response = new(refreshToken.Token, accessToken);

            m_accountRepositoryMock
                .Setup(x => x.RefreshToken(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(response);

            // Act
            var result = await m_authenticationService.RefreshToken(refreshToken.Token, ipAddress);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<StaticRefreshTokenResponse>(result);
            Assert.Equal(response.RefreshToken, response.RefreshToken);
            Assert.Equal(response.AccessToken, response.AccessToken);
        }

        [Fact]
        public async void RefreshToken_ShouldReturnNull_WhenTokenIsInvalid()
        {
			//Arrange
			string ipAddress = "127.0.0.1";

			m_accountRepositoryMock
				.Setup(x => x.RefreshToken(It.IsAny<string>(), It.IsAny<string>()))
				.ReturnsAsync(() => null);


            //Act
            var result = await m_authenticationService.RefreshToken(string.Empty, ipAddress);
			
            //Assert
            Assert.Null(result);
		}

        [Fact]
        public async void RevokeToken_ShouldReturnTrue_IfRefreshTokenIsActive()
        {
			// Arrange
			string ipAddress = "127.0.0.1";

			Account account = new()
			{
				AccountID = 1,
				Username = "Nicklas",
				Password = BC.HashPassword("Nicklas"),
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

			RefreshToken refreshToken = JWTHandler.GenerateRefreshToken(ipAddress);

			string accessToken = JWTHandler.GenerateJWTToken(account, new AppSettings { Secret = "12345678912345678912345678912345" });

			AuthenticationResponse response = new(refreshToken.Token, accessToken);

			m_accountRepositoryMock
				.Setup(x => x.RefreshToken(It.IsAny<string>(), It.IsAny<string>()))
				.ReturnsAsync(response);

			m_accountRepositoryMock
	            .Setup(x => x.RevokeToken(It.IsAny<string>(), It.IsAny<string>()))
	            .ReturnsAsync(true);

			// Act
			var result = await m_authenticationService.RefreshToken(refreshToken.Token, ipAddress);

            var revoke = await m_authenticationService.RevokeToken(refreshToken.Token, ipAddress);

			// Assert
			Assert.NotNull(result);
			Assert.IsType<StaticRefreshTokenResponse>(result);
            Assert.True(revoke);
		}

        [Fact]
        public async void RevokeToken_ShouldReturnFalse_IfRefreshTokenIsNotActive()
        {
			// Arrange
			string ipAddress = "127.0.0.1";

			Account account = new()
			{
				AccountID = 1,
				Username = "Nicklas",
				Password = BC.HashPassword("Nicklas"),
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

			RefreshToken refreshToken = JWTHandler.GenerateRefreshToken(ipAddress);

			string accessToken = JWTHandler.GenerateJWTToken(account, new AppSettings { Secret = "12345678912345678912345678912345" });

			AuthenticationResponse response = new(refreshToken.Token, accessToken);

			//Arrange
			m_accountRepositoryMock
				.Setup(x => x.RevokeToken(It.IsAny<string>(), It.IsAny<string>()))
	            .ReturnsAsync(false);
            
            //Act
			var revoke = await m_authenticationService.RevokeToken(refreshToken.Token, ipAddress);

            //Assert
            Assert.False(revoke);
		}
    }
}
