using CEC_CRM.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CEC_CRM.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly DataContext dbc;
        public OrderController(DataContext dbc)
        {
            this.dbc = dbc;
        }


        [HttpGet("{email}")]
        public async Task<IActionResult> GetOrderDetailsByEmail(string email)
        {
            try
            {
                var orderDetails = await dbc.OrderDetails
                    .Where(od => od.UserEmail == email)
                    .ToListAsync();

                if (!orderDetails.Any())
                {
                    return NotFound(new { message = $"No orders found for email: {email}" });
                }

                return Ok(orderDetails); // Return 200 OK with the orders for the specified email
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error fetching order details by email.", error = ex.Message });
            }
        }
    }
}
