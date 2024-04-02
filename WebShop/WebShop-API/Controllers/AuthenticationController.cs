using Microsoft.AspNetCore.Authorization;
using WebShop_API.DTOs.Authentication;

namespace WebShop_API.Controllers
{
    /// <summary>
    /// Using AuthenticationController to authenticate.
    /// </summary>
    // Route is used to determine what there is in the URL.
    [Route( "api/[controller]" )] 
    [ApiController]
    public class AuthenticationController : ControllerBase // We are inheriting from ControllerBase.
    {
        /// <summary>
        /// Using IAuthenticationService as interface.
        /// </summary>
        private readonly IAuthenticationService m_authenticationService;

        /// <summary>
        /// Authenticate user.
        /// </summary>
        /// <param name="authenticationService"></param>
        public AuthenticationController( IAuthenticationService authenticationService )
        {
            m_authenticationService = authenticationService;
        }

        /// <summary>
        /// Used to check if you have a response from AuthenticationResponse. If you have a response, then you are authenticated and we set your refreshtoken in the browser cookies.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>response or an error</returns>
        [HttpPost]
        public async Task<IActionResult> Authenticate( [FromBody] AuthenticationRequest request ) // We use [FromBody] for security measures
        {
            try
            {
                // Makeing a veriable from AuthenticationResponse and calling it "response".
                // Using await because it's a async method. Async means that we use one of the idle threds in the cpu, so we can do more then one task on the cpu.
                // We are using the veriable request and the method IPAddress to put in response.
                AuthenticationResponse response = await m_authenticationService.Authenticate( request, IPAddress() ); 

                if(response == null)
                {
                    // Returns statuscode 401 unautherized
                    return Unauthorized( "Incorrect Username/Email or Password." ); 
                }

                //Calling the SetTokenCookie method to set the cookies in the client browser, and makes them authenticated.
                SetTokenCookie( response.RefreshToken ); 

                // Returns statuscode 200 Ok
                return Ok( response ); 
            }
            catch (Exception ex)
            {
                // Returns Problem with exception message
                return Problem( ex.Message ); 
            }
        }

        /// <summary>
        /// Used to check if you have a refreshtoken. 
        /// </summary>
        /// <returns>response or an error</returns>
        [HttpPut]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {
                // Trying to get a refreshtoken in the clients browser and putting it in a veriable.
                string refreshToken = Request.Cookies[ "refreshToken" ]; 

                if( string.IsNullOrEmpty( refreshToken ) )
                {
                    // If there is no or null, then we return badrequest.
                    return BadRequest( "Missing refresh token!" );
                }

                AuthenticationResponse response = await m_authenticationService.RefreshToken( refreshToken, IPAddress() );

                if( response == null)
                {
                    return Problem( "An unexpected error occured, please try again." );
                }

                SetTokenCookie( response.RefreshToken );

                return Ok( response );
            }
            catch (Exception ex)
            {
                return Problem( ex.Message );
            }
        }

        /// <summary>
        /// Used to delete and then revoke your refreshtoken.
        /// </summary>
        /// <returns>refreshToken or an error</returns>
        [HttpDelete]
        public async Task<IActionResult> RevokeToken()
        {
            try
            {
                string refreshToken = Request.Cookies[ "refreshToken" ];

                if (string.IsNullOrEmpty( refreshToken ))
                {
                    return BadRequest( "Missing refresh token!" );
                }

                bool result = await m_authenticationService.RevokeToken( refreshToken, IPAddress() );

                if (!result)
                {
                    return BadRequest();
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return Problem( ex.Message );
            }
        }

        /// <summary>
        /// Used to use cookies in your browser.
        /// </summary>
        /// <param name="token"></param>
        private void SetTokenCookie( string token )
        {
            // Configure cookie, setting expiration date and enabling HttpOnly
            CookieOptions options = new()
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays( 7 )
            };

            // Append the token to the cookies of the current response from the server
            Response.Cookies.Append( "refreshToken", token, options );
        }

        /// <summary>
        /// Getting your IP, so we can use it to authenticate.
        /// </summary>
        /// <returns>IP</returns>
        private string IPAddress()
        {
            // Check if the http request contains an IP if not get it from the context instead
            if (Request.Headers.ContainsKey( "X-Forwarded-For" ))
            {
                // Get the IP from the request
                return Request.Headers[ "X-Forwarded-For" ];
            }
            else
            {
                // Get the IP and map it an IPv4
                return HttpContext.Connection.RemoteIpAddress!.MapToIPv4().ToString();
            }
        }
    }
}
