using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using WebShop_API.Controllers;
using WebShop_API.DTOs.Authentication;
using WebShop_API.DTOs.RefreshToken;

namespace WebShopUnitTests.Controllers
{
    public class AuthenticationControllerTests
    {
		private readonly AuthenticationController m_authController;
        private readonly DefaultHttpContext m_httpContext;
		private readonly Mock<IAuthenticationService> m_authenticationServiceMock = new Mock<IAuthenticationService>();
        private readonly Mock<IRequestCookieCollection> m_requestCookieCollectionMock = new Mock<IRequestCookieCollection>();

		public AuthenticationControllerTests()
		{
            m_httpContext = new DefaultHttpContext();

            m_httpContext.Request.Cookies = m_requestCookieCollectionMock.Object;

            ControllerContext controllerContext = new()
            {
                HttpContext = m_httpContext
            };

            m_authController = new( m_authenticationServiceMock.Object )
            {
                ControllerContext = controllerContext
            };
        }

		[Fact]
		public async void Authenticate_ShouldReturnStatusCode200_WhenSuccessfullyAuthenticated()
		{
            //Arrange
            m_authController.Request.Headers.Add( "X-Forwarded-For", "127.0.0.1" );

			AuthenticationRequest request = new()
			{
				Username_Email = "alex",
				Password = "Passw0rd"
			};

			AuthenticationResponse response = new( "EmptyToken", "EmptyToken" );

			m_authenticationServiceMock
				.Setup( x => x.Authenticate( It.IsAny<AuthenticationRequest>(), It.IsAny<string>() ) )
				.ReturnsAsync( response );

			//Act
			var result = (IStatusCodeActionResult) await m_authController.Authenticate( request );

            //Assert
			Assert.Equal( 200, result.StatusCode );
        }

        [Fact]
        public async void Authenticate_ShouldReturnStatusCode401_WhenFailedAuthenticated()
        {
            //Arrange
            m_authController.Request.Headers.Add( "X-Forwarded-For", "127.0.0.1" );

            AuthenticationRequest request = new()
            {
                Username_Email = "alex",
                Password = "Passw0rd"
            };

            m_authenticationServiceMock
                .Setup( x => x.Authenticate( It.IsAny<AuthenticationRequest>(), It.IsAny<string>() ) )
                .ReturnsAsync( () => null );

            //Act
            var result = (IStatusCodeActionResult) await m_authController.Authenticate( request );

            //Assert
            Assert.Equal( 401, result.StatusCode );
        }

        [Fact]
        public async void Authenticate_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            //Arrange
            AuthenticationRequest request = new()
            {
                Username_Email = "alex",
                Password = "Passw0rd"
            };

            m_authenticationServiceMock
                .Setup( x => x.Authenticate( It.IsAny<AuthenticationRequest>(), It.IsAny<string>() ) )
                .ReturnsAsync( () => throw new Exception() );

            //Act
            var result = (IStatusCodeActionResult) await m_authController.Authenticate( request );

            //Assert
            Assert.Equal( 500, result.StatusCode );
        }

        [Fact]
        public async void RefreshToken_ShouldReturnStatusCode200_WhenSuccessfullyRefreshed()
        {
            // Arrange
            m_authController.Request.Headers.Add( "X-Forwarded-For", "127.0.0.1" );

            AuthenticationResponse response = new( "EMPTY TOKEN", "EMPTY TOKEN" );

            m_requestCookieCollectionMock
                .SetupGet( x => x[ "refreshToken" ] )
                .Returns( "[TOKEN]" );


            m_authenticationServiceMock
                .Setup(x => x.RefreshToken(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(response);

            // Act
            var result = (IStatusCodeActionResult) await m_authController.RefreshToken();

            // Assert
            Assert.Equal( 200, result.StatusCode );
        }

        [Fact]
        public async void RefreshToken_ShouldReturnStatusCode400_WhenMissingRefreshToken()
        {
            // Arrange
            m_requestCookieCollectionMock
                .SetupGet( x => x[ "refreshToken" ] )
                .Returns( () => null );

            // Act
            var result = (IStatusCodeActionResult) await m_authController.RefreshToken();

            // Assert
            Assert.Equal( 400, result.StatusCode );
        }

        [Fact]
        public async void RefreshToken_ShouldReturnStatusCode500_WhenServiceReturnsNull()
        {
            // Arrange
            m_authController.Request.Headers.Add( "X-Forwarded-For", "127.0.0.1" );

            m_requestCookieCollectionMock
                .SetupGet( x => x[ "refreshToken" ] )
                .Returns( () => "[TOKEN]" );

            m_authenticationServiceMock
                .Setup( x => x.RefreshToken( It.IsAny<string>(), It.IsAny<string>() ) )
                .ReturnsAsync( () => null );

            // Act
            var result = (IStatusCodeActionResult) await m_authController.RefreshToken();

            // Assert
            Assert.Equal( 500, result.StatusCode );
        }

        [Fact]
        public async void RefreshToken_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            m_authController.Request.Headers.Add( "X-Forwarded-For", "127.0.0.1" );

            m_requestCookieCollectionMock
                .SetupGet( x => x[ "refreshToken" ] )
                .Returns( () => "[TOKEN]" );

            m_authenticationServiceMock
                .Setup( x => x.RefreshToken( It.IsAny<string>(), It.IsAny<string>() ) )
                .ReturnsAsync( () => throw new Exception() );

            // Act
            var result = (IStatusCodeActionResult) await m_authController.RefreshToken();

            // Assert
            Assert.Equal( 500, result.StatusCode );
        }

        [Fact]
        public async void RevokeToken_ShouldReturnStatusCode200_WhenSuccessfullyRevoked()
        {
            // Arrange
            m_authController.Request.Headers.Add( "X-Forwarded-For", "127.0.0.1" );

            m_requestCookieCollectionMock
                .SetupGet( x => x[ "refreshToken" ] )
                .Returns( () => "[TOKEN]" );

            m_authenticationServiceMock
                .Setup( x => x.RevokeToken( It.IsAny<string>(), It.IsAny<string>() ) )
                .ReturnsAsync( true );

            // Act
            var result = (IStatusCodeActionResult) await m_authController.RevokeToken();

            // Assert
            Assert.Equal ( 200, result.StatusCode );
        }

        [Fact]
        public async void RevokeToken_ShouldReturnStatusCode400_WhenMissingRefreshToken()
        {
            // Arrange
            m_authController.Request.Headers.Add( "X-Forwarded-For", "127.0.0.1" );

            m_requestCookieCollectionMock
                .SetupGet( x => x[ "refreshToken" ] )
                .Returns( () => null );

            // Act
            var result = (IStatusCodeActionResult) await m_authController.RevokeToken();

            // Assert
            Assert.Equal( 400, result.StatusCode );
        }

        [Fact]
        public async void RevokeToken_ShouldReturnStatusCode400_WhenServiceReturnsFalse()
        {
            // Arrange
            m_authController.Request.Headers.Add( "X-Forwarded-For", "127.0.0.1" );

            m_requestCookieCollectionMock
                .SetupGet( x => x[ "refreshToken" ] )
                .Returns( () => "[TOKEN]" );

            m_authenticationServiceMock
                .Setup( x => x.RevokeToken( It.IsAny<string>(), It.IsAny<string>() ) )
                .ReturnsAsync( false );

            // Act
            var result = (IStatusCodeActionResult) await m_authController.RevokeToken();

            // Assert
            Assert.Equal( 400, result.StatusCode );
        }

        [Fact]
        public async void RevokeToken_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            m_authController.Request.Headers.Add( "X-Forwarded-For", "127.0.0.1" );

            m_requestCookieCollectionMock
                .SetupGet( x => x[ "refreshToken" ] )
                .Returns( () => "[TOKEN]" );

            m_authenticationServiceMock
                .Setup( x => x.RevokeToken( It.IsAny<string>(), It.IsAny<string>() ) )
                .ReturnsAsync( () => throw new Exception() );

            // Act
            var result = (IStatusCodeActionResult) await m_authController.RevokeToken();

            // Assert
            Assert.Equal( 500, result.StatusCode );
        }
    }
}
