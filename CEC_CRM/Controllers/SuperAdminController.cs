using Microsoft.AspNetCore.Mvc;

namespace CEC_CRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperAdminController : ControllerBase
    {
        private const string DefaultUsername = "superadmin";
        private const string DefaultPassword = "password@123";

        [HttpPost("login")]
        public IActionResult Superlogin(Loginrr request)
        {
            try
            {
                if (request == null || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
                {
                    return BadRequest("Invalid login request.");
                }
                if (request.Username == DefaultUsername || request.Password == DefaultPassword)
                {
                    return Ok(new
                    {
                        message = "Login successfull"
                    });
                    
                }

                return BadRequest("Invalid email, password, or admin type.");
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return StatusCode(500, new { message = "An error occurred during login. Please try again later." });
            }

        }

    }
    public class Loginrr
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}

