using CEC_CRM.Data;
using CEC_CRM.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CEC_CRM.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase  
    {
        private readonly DataContext dbc;

        public UserController(DataContext dbc)
        {
            this.dbc = dbc;
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] Users user)
        {
            if (ModelState.IsValid)
            {
                
                var existingUser = await dbc.Users
                    .FirstOrDefaultAsync(u => u.email == user.email);

                if (existingUser != null)
                {
                    return Conflict("Email is already taken.");
                }
                
                
                dbc.Users.Add(user);
                await dbc.SaveChangesAsync();

                // Return a Created response with the new user
                return CreatedAtAction(nameof(AddUser), new { email = user.email }, user);
            }

            // Return BadRequest if model validation failed
            return BadRequest(ModelState);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Email) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest("Invalid login request.");
            }       
            var user = await dbc.Users.FirstOrDefaultAsync(u => u.email == loginRequest.Email);

            if (user == null || user.password != loginRequest.Password)
            {
                return Unauthorized("Invalid email or password.");
            }

            // Return success response (you can include a token here)
            return Ok(new
            {
                username = user.name,  // Assuming 'Name' is the user's username
                email = user.email,     // Email of the user
                cus_id = user.user_id
            });
        }
        public class LoginRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await dbc.Users.ToListAsync();
            return Ok(users);
        }

        // New Edit Endpoint
        [HttpPut("{id}")]
        public async Task<IActionResult> EditUser(int id, [FromBody] Users updatedUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var existingUser = await dbc.Users.FindAsync(id);
            if (existingUser == null)
            {
                return NotFound($"User with ID {id} not found.");
            }

            // Update user details
            existingUser.name = updatedUser.name;
            existingUser.phone_number = updatedUser.phone_number;

            // Save changes to the database
            try
            {
                dbc.Users.Update(existingUser);
                await dbc.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "Error updating user: " + ex.Message);
            }

            return Ok(existingUser);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                var user = dbc.Users.Find(id);
                if (user == null)
                {
                    return NotFound(new { message = "User not found." }); // 404 Not Found
                }

                dbc.Users.Remove(user);
                dbc.SaveChanges();

                return Ok(new { message = "User deleted successfully." }); // 200 OK
            }
            catch (Exception ex)
            {
                // Return a proper error status code and message
                return StatusCode(500, new { message = "An error occurred while deleting the user. Please try again." });
            }
        }
        [HttpGet("{userid}")]
        public async Task<IActionResult> getUserDetails(int userid)
        {
            try
            {
                var user = await dbc.Users.FindAsync(userid);
                if (user == null)
                {
                    return NotFound(new { message = "User not found." }); // 404 Not Found
                }
                return Ok(user); // 200 OK
            }
            catch (Exception ex)
            {
                // Return a proper error status code and message
                return StatusCode(500, new { message = "An error occurred while deleting the user. Please try again." });
            }
        }

        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var user = await dbc.Users.FindAsync(request.UserId);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            if (user.password != request.CurrentPassword)
            {
                return BadRequest(new { message = "Current password is incorrect." });
            }

            // Update the password
            user.password = request.NewPassword;

            // Save changes
            await dbc.SaveChangesAsync();

            return Ok(new { message = "Password changed successfully." });
        }
    }

    public class ChangePasswordRequest
    {
        public int UserId { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }

}
