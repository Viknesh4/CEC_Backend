using CEC_CRM.Data;
using CEC_CRM.models;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CEC_CRM.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : Controller
    {
        private readonly DataContext dbc;

        public AdminController(DataContext dbc)
        {
            this.dbc = dbc;
        }

        [HttpPost]
        public async Task<IActionResult> AddAdmin([FromBody] Admin admin)
        {
            if (ModelState.IsValid)
            {

                var existingAdmin = await dbc.Admin
                    .FirstOrDefaultAsync(u => u.email == admin.email);

                if (existingAdmin != null)
                {
                    return Conflict("Email is already taken.");
                }


                dbc.Admin.Add(admin);
                await dbc.SaveChangesAsync();

                // Return a Created response with the new user
                return Ok();
            }

            // Return BadRequest if model validation failed
            return BadRequest(ModelState);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAdmins()
        {
            var admins = await dbc.Admin.ToListAsync();
            return Ok(admins); // Return all admin data
        }

        //Edit

        [HttpPut("{id}")]
        public async Task<IActionResult> EditAdmin(int id, [FromBody] Admin updatedAdmin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingAdmin = await dbc.Admin.FindAsync(id);
            if (existingAdmin == null)
            {
                return NotFound($"User with ID {id} not found.");
            }

            // Update user details
            existingAdmin.name = updatedAdmin.name;
            existingAdmin.email = updatedAdmin.email;
            existingAdmin.admin_type = updatedAdmin.admin_type;

            // Save changes to the database
            try
            {
                await dbc.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "Error updating user: " + ex.Message);
            }

            return Ok(existingAdmin);
        }

        //Delete

        [HttpDelete("{id}")]
        public IActionResult DeleteAdmin(int id)
        {
            try
            {
                var admin = dbc.Admin.Find(id);
                if (admin == null)
                {
                    return NotFound(new { message = "User not found." }); // 404 Not Found
                }

                dbc.Admin.Remove(admin);
                dbc.SaveChanges();

                return Ok(new { message = "User deleted successfully." }); // 200 OK
            }
            catch (Exception ex)
            {
                // Return a proper error status code and message
                return StatusCode(500, new { message = "An error occurred while deleting the user. Please try again." });
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> AdminLogin([FromBody] LoginBody loginBody)
        {
            try
            {
                if (loginBody == null || string.IsNullOrEmpty(loginBody.Email) || string.IsNullOrEmpty(loginBody.Password))
                {
                    return BadRequest("Invalid login request.");
                }

                var admin = await dbc.Admin.FirstOrDefaultAsync(u => u.email == loginBody.Email);
                if (admin == null || admin.password != loginBody.Password || admin.admin_type != loginBody.AdminType)
                {
                    return BadRequest("Invalid email, password, or admin type.");
                }

                return Ok(new
                {
                    admintype = admin.admin_type,
                    adminid = admin.admin_id
                });
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return StatusCode(500, new { message = "An error occurred during login. Please try again later." });
            }
        }

        public class LoginBody
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string AdminType { get; set; }
        }

    }

}

