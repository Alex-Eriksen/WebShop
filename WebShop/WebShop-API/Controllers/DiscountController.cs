using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebShop_API.Controllers
{
    /// <summary>
    /// Using DiscountController to control discounts.
    /// </summary>
    [Route( "api/[controller]" )]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        /// <summary>
        /// Uses the IDiscountService interface.
        /// </summary>
        private readonly IDiscountService m_discountService;

        /// <summary>
        /// Constructor for DiscountController.
        /// </summary>
        /// <param name="discountService"></param>
        public DiscountController( IDiscountService discountService )
        {
            m_discountService = discountService;
        }

        /// <summary>
        /// Gets all discounts.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<StaticDiscountResponse> responses = await m_discountService.GetAll();

                if( responses == null)
                {
                    return Problem( "The discount service responded with null." );
                }

                if( responses.Count == 0)
                {
                    return NoContent();
                }

                return Ok( responses );
            }
            catch (Exception ex)
            {
                return Problem( ex.Message );
            }
        }

        /// <summary>
        /// Gets discount by its id.
        /// </summary>
        /// <param name="discountId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{discountId}")]
        public async Task<IActionResult> GetById( int discountId )
        {
            try
            {
                DirectDiscountResponse response = await m_discountService.GetById( discountId );
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

        /// <summary>
        /// Creates discounts.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create( [FromBody]DiscountRequest request )
        {
            try
            {
                DirectDiscountResponse response = await m_discountService.Create( request );
                if(response == null)
                {
                    return BadRequest("Request can not be null");
                }

                return Ok( response );
            }
            catch (Exception ex)
            {
                return Problem( ex.Message );
            }
        }

        /// <summary>
        /// Updates discounts.
        /// </summary>
        /// <param name="discountId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{discountId}")]
        public async Task<IActionResult> Update( int discountId, [FromBody]DiscountRequest request )
        {
            try
            {
                DirectDiscountResponse response = await m_discountService.Update( discountId, request );
                if(response == null)
                {
                    return NotFound();
                }

                return Ok( response );
            }
            catch(Exception ex)
            {
                return Problem( ex.Message );
            }
        }

        /// <summary>
        /// Deletes discounts.
        /// </summary>
        /// <param name="discountId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{discountId}")]
        public async Task<IActionResult> Delete( int discountId )
        {
            try
            {
                DirectDiscountResponse response = await m_discountService.Delete( discountId );
                if(response == null)
                {
                    return NotFound( $"No Discount with the id: {discountId}" );
                }

                return Ok( response );
            }
            catch(Exception ex)
            {
                return Problem( ex.Message );
            }
        }
    }
}
