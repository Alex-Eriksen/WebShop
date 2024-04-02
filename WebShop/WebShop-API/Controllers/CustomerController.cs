using Microsoft.AspNetCore.Authorization;
using System.Net.Mail;

namespace WebShop_API.Controller
{
    /// <summary>
    /// Using CustomerController to control customer.
    /// </summary>
    // Route is used to determine what there is in the URL.
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase // We are inheriting from ControllerBase.
    {
        /// <summary>
        /// Using ICustomerService as interface.
        /// </summary>
        private readonly ICustomerService m_customerService;


        /// <summary>
        /// Constructor for CustomerController
        /// </summary>
        /// <param name="customerService"></param>
        public CustomerController( ICustomerService customerService )
        {
            m_customerService = customerService;
        }

        /// <summary>
        /// Gets all customers.
        /// </summary>
        /// <returns>customers, message or exeption</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                // List of StaticCustomerResponse as customers using the service.GetAll()
                List<StaticCustomerResponse> customers = await m_customerService.GetAll(); 

                if (customers == null)
                {
                    return Problem( "Nothing was returned from service, this was unexpected" );
                }

                if (customers.Count == 0)
                {
                    return NoContent();
                }

                return Ok( customers );
            }
            catch (Exception ex)
            {
                return Problem( ex.Message );
            }
        }

        /// <summary>
        /// Gets customer by its id.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route( "{customerId}" )]
        public async Task<IActionResult> GetById( int customerId )
        {
            try
            {
                DirectCustomerResponse directCustomerResponse = await m_customerService.GetById( customerId );

                if (directCustomerResponse == null)
                {
                    return NotFound();
                }
                return Ok( directCustomerResponse );
            }
            catch (Exception ex)
            {
                return Problem( ex.Message );
            }
        }

        /// <summary>
        /// Creates a customer.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create( [FromBody] NewCustomerRequest request )
        {
            try
            {
                DirectCustomerResponse directCustomerResponse = await m_customerService.Create( request );

                if (directCustomerResponse == null)
                {
                    return Problem("Customer was not created, something failed...");
                }
                return Ok(directCustomerResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Updates a customer.
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{customerId}")]
        public async Task<IActionResult> Update(int customerId, NewCustomerRequest request)
        {
            try
            {
                DirectCustomerResponse directCustomerResponse = await m_customerService.Update(customerId, request);
                if (directCustomerResponse == null)
                {
                    return NotFound();
                }
                return Ok(directCustomerResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a customer.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{customerId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int customerId)
        {
            try
            {
                DirectCustomerResponse directCustomerResponse = await m_customerService.Delete(customerId);

                if (directCustomerResponse == null)
                {
                    return NotFound();
                }
                return Ok(directCustomerResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Creates a payment.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("payment")]
        public async Task<IActionResult> CreatePayment( [FromBody] PaymentRequest request )
        {
            try
            {
                DirectPaymentResponse response = await m_customerService.CreatePayment( request );

                if(response == null)
                {
                    return Problem( "Payment was not created, something went wrong..." );
                }

                return Ok( response );
            }
            catch (Exception ex)
            {
                return Problem( ex.Message );
            }
        }

        /// <summary>
        /// Updates a payment.
        /// </summary>
        /// <param name="paymentId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("payment/{paymentId}")]
        public async Task<IActionResult> UpdatePayment( int paymentId, [FromBody] PaymentRequest request )
        {
            try
            {
                DirectPaymentResponse response = await m_customerService.UpdatePayment( paymentId, request );

                if(response == null)
                {
                    return Problem( "Something went wrong..." );
                }

                return Ok( response );
            }
            catch(Exception ex)
            {
                return Problem( ex.Message );
            }
        }

        /// <summary>
        /// Deletes a payment.
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("payment/{paymentId}")]
        public async Task<IActionResult> DeletePayment( int paymentId )
        {
            try
            {
                DirectPaymentResponse response = await m_customerService.DeletePayment( paymentId );

                if (response == null)
                {
                    return Problem( "Something went wrong..." );
                }

                return Ok( response );
            }
            catch (Exception ex)
            {
                return Problem( ex.Message );
            }
        }

        /// <summary>
        /// Gets a payment by its id.
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("payment/{paymentId}")]
        public async Task<IActionResult> GetPayment( int paymentId )
        {
            try
            {
                DirectPaymentResponse response = await m_customerService.GetPayment( paymentId );

                if(response == null)
                {
                    return NotFound();
                }

                return Ok( response );
            }
            catch (Exception ex)
            {
                return Problem( ex.Message );
            }
        }
    }
}
