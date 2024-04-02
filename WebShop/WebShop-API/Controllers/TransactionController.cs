using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebShop_API.Database.Entities;

namespace WebShop_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        [Route("transaction")]
        public async Task<IActionResult> GetAllTransactions()
        {
            try
            {
                List<StaticTransactionResponse> transactions = await _transactionService.GetAllTransactions();
                if(transactions == null)
                {
                    return Problem("Nothing was returned from service, this is unexpected");
                }
                if (transactions.Count == 0)
                {
                    return NoContent();
                }
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet]
        [Route("order")]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                List<StaticOrderResponse> orders = await _transactionService.GetAllOrders();
                if (orders == null)
                {
                    return Problem("Nothing was returned from service, this is unexpected");
                }
                if (orders.Count == 0)
                {
                    return NoContent();
                }
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet]
        [Route("{transactionId}")]
        public async Task<IActionResult> GetByTransactionId(int transactionId)
        {
            try
            {
                DirectTransactionResponse directTransactionResponse = await _transactionService.GetByTransactionId(transactionId);

                if (directTransactionResponse == null)
                {
                    return NotFound();
                }
                return Ok(directTransactionResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet]
        [Route("order/{orderId}")]
        public async Task<IActionResult> GetByOrderId(int orderId)
        {
            try
            {
                DirectOrderResponse directOrderResponse = await _transactionService.GetByOrderId(orderId);

                if (directOrderResponse == null)
                {
                    return NotFound();
                }
                return Ok(directOrderResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        [Route("transaction")]
        public async Task<IActionResult> CreateTransaction([FromBody] TransactionRequest request)
        {
            try
            {
                DirectTransactionResponse directTransactionResponse = await _transactionService.CreateTransaction(request);

                if (directTransactionResponse == null)
                {
                    return Problem("Transaction was not created, something failed...");
                }
                return Ok(directTransactionResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        [Route("order")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderRequest request)
        {
            try
            {
                DirectOrderResponse directordernResponse = await _transactionService.CreateOrder(request);

                if (directordernResponse == null)
                {
                    return Problem("Order was not created, something failed...");
                }
                return Ok(directordernResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPut]
        [Route("transaction/{transactionId}")]
        public async Task<IActionResult> UpdateTransaction(int transactionId, [FromBody] TransactionRequest request)
        {
            try
            {
                DirectTransactionResponse response = await _transactionService.UpdateTransaction(transactionId, request);
                if (response == null)
                {
                    return NotFound();
                }
                return Ok(request);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }


        [HttpPut]
        [Route("order/{orderId}")]
        public async Task<IActionResult> UpdateOrder(int orderId, [FromBody] OrderRequest request)
        {
            try
            {
                DirectOrderResponse response = await _transactionService.UpdateOrder(orderId, request);
                if (response == null)
                {
                    return NotFound();
                }
                return Ok(request);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
