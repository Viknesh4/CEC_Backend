using CEC_CRM.Data;
using CEC_CRM.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace CEC_CRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class TicketController : ControllerBase
    {
        
        private readonly DataContext dbc;
        public TicketController(DataContext dbc)
        {
            this.dbc = dbc;
        }


        [HttpPost]
        public async Task<IActionResult> SaveTicket([FromBody] Ticket ticket1)
        {
            if (ticket1 == null)
            {
                return BadRequest("Query cannot be null.");
            }

            if (string.IsNullOrEmpty(ticket1.title) || string.IsNullOrEmpty(ticket1.description))
            {
                return BadRequest("Title and Description are required.");
            }

            // Save the query to the database
            dbc.ticket.Add(ticket1);
            await dbc.SaveChangesAsync();

            // Return the saved query with HTTP 201 status code
            return Ok();
        }

        [HttpGet("{customerId}")]
        public IActionResult GetTicketsByCustomerId(int customerId)
        {
            try
            {
                var tickets = dbc.ticket
                    .Where(ticket => ticket.customer_id == customerId)
                    .ToList();

                if (tickets == null || !tickets.Any())
                {
                    return NotFound(new { Message = "No tickets found for the provided customer ID." });
                }

                return Ok(tickets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching tickets.", Error = ex.Message });
            }
        }

        [HttpGet("Login/{admin_type}")]
        public IActionResult GetTicketsByAdminType(string admin_type)
        {
            try
            {
                var tickets = dbc.ticket
                    .Where(ticket => ticket.admin_type == admin_type && ticket.status == "new")
                    .ToList();

                if (tickets == null || !tickets.Any())
                {
                    return NotFound(new { Message = "No tickets found for the provided Admin Type." });
                }

                return Ok(tickets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching tickets.", Error = ex.Message });
            }
        }

        [HttpGet("GetAlltickets")]
        public async Task<IActionResult> GetAllTickets()
        {
            var tickets = await dbc.ticket.ToListAsync();
            return Ok(tickets);
        }

        [HttpGet("in-progress-tickets/{admin_id}")]
        public async Task<IActionResult> GetInProgressTickets(int admin_id)
        {
            var inProgressTickets = await dbc.ticket
                .Where(t => t.status == "in_progress" && t.assigned_admin_id ==admin_id)
                .ToListAsync();

            return Ok(inProgressTickets);
        }

        [HttpPut("update-ticket/{ticket_id}")]
        public async Task<IActionResult> UpdateTicket(int ticket_id, [FromBody] TicketUpdateRequest request)
        {
            var ticket = await dbc.ticket.FindAsync(ticket_id);

            if (ticket == null)
                return NotFound(new { message = "Ticket not found" });

            if (ticket.assigned_admin_id == 0 && request.assigned_admin_id != 0)
                ticket.assigned_admin_id = request.assigned_admin_id;
            // Update status if provided
            if (!string.IsNullOrEmpty(request.status))
                ticket.status = request.status;

            // Update message if provided
            if (!string.IsNullOrEmpty(request.message))
                ticket.message = request.message;

            ticket.updated_at = DateTime.UtcNow;

            dbc.ticket.Update(ticket);
            await dbc.SaveChangesAsync();

            return Ok(new { message = "Ticket updated successfully" });
        }

        [HttpGet("resolved/{admin_id}")]
        public async Task<IActionResult> GetResolvedTickets(int admin_id)
        {
            var ResolvedTickets = await dbc.ticket
                .Where(t => t.status == "resolved" && t.assigned_admin_id == admin_id)
                .ToListAsync();

            return Ok(ResolvedTickets);
        }

        [HttpPut("forward-ticket/{ticket_id}")]
        public async Task<IActionResult> ForwardTicket(int ticket_id, [FromBody] AdminCategoryUpdateRequest request)
        {
            var ticket = await dbc.ticket.FindAsync(ticket_id);

            if (ticket == null)
                return NotFound(new { message = "Ticket not found" });

            // Update the admin category
            if (!string.IsNullOrEmpty(request.admin_category))
                ticket.admin_type = request.admin_category;

            ticket.updated_at = DateTime.UtcNow; // Update timestamp

            // Save changes to the database
            dbc.ticket.Update(ticket);
            try
            {
                await dbc.SaveChangesAsync();
                return Ok(new { message = "Admin category updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while forwarding the ticket.", details = ex.Message });
            }
        }

    }
}
