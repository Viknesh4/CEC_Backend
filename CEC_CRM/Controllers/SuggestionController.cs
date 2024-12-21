using CEC_CRM.Data;
using CEC_CRM.models;
using MailKit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Runtime.InteropServices;

namespace CEC_CRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuggestionController : Controller
    {
        private readonly DataContext dbc;

        public SuggestionController(DataContext dbc)
        {
            this.dbc = dbc;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSuggestion()
        {
            var nsuggestion = await dbc.suggestions.ToListAsync();
            return Ok(nsuggestion);
        }

        [HttpPost]
        public async Task<IActionResult> AddSuggestion(Suggestion suggestion)
        {
            if (ModelState.IsValid)
            {
                dbc.suggestions.Add(suggestion);
                await dbc.SaveChangesAsync();
                return Ok();
            }
            return BadRequest(ModelState);
            
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> EditUser(int id, [FromBody] Suggestion updatedsuggestion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingsuggestion = await dbc.suggestions.FindAsync(id);
            if (existingsuggestion == null)
            {
                return NotFound($"Suggestion with ID {id} not found.");
            }

            // Update user details
            existingsuggestion.title = updatedsuggestion.title;
            existingsuggestion.description = updatedsuggestion.description;

            // Save changes to the database
            try
            {
                dbc.suggestions.Update(existingsuggestion);
                await dbc.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "Error updating user: " + ex.Message);
            }

            return Ok(existingsuggestion);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> deleteSuggestion(int id)
        {
            try
            {
                var suggesstion = await dbc.suggestions.FindAsync(id);
                if (suggesstion == null)
                {
                    return NotFound(new { message = "Suggestion not found" });
                }
                dbc.suggestions.Remove(suggesstion);
                await dbc.SaveChangesAsync();
                return Ok("suggestion deleted Successfully");
            }
            catch(Exception ex)
            {
                return StatusCode(500, "Error while deleting the suggestion");
            }
        }
    }
}
