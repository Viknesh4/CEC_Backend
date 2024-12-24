using CEC_CRM.Data;
using CEC_CRM.models;
using MailKit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics.Eventing.Reader;
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

        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetAllSuggestionbyID(int id)
        {
            try
            {
                var nsuggestions = await dbc.suggestions
                    .Where(s => s.user_id == id)
                    .ToListAsync();

                if (nsuggestions == null || !nsuggestions.Any())
                {
                    return NotFound(new { message = "No suggestions found for the provided user ID." });
                }

                return Ok(new { suggestions = nsuggestions });
            }
            catch (Exception ex)
            {
                // Log the exception internally (e.g., using a logging framework)
                return StatusCode(500, new { message = "An error occurred while fetching suggestions." });
            }
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
                // Find the suggestion by ID
                var suggestion = await dbc.suggestions.FindAsync(id);

                // Check if the suggestion exists
                if (suggestion == null)
                {
                    return NotFound(new { message = $"Suggestion with ID {id} not found." });
                }

                // Remove the suggestion
                dbc.suggestions.Remove(suggestion);
                await dbc.SaveChangesAsync();

                // Return a structured success response
                return Ok(new { message = "Suggestion deleted successfully.", suggestionId = id });
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here, but could use a logging framework)
                return StatusCode(500, new { message = "An error occurred while deleting the suggestion.", error = ex.Message });
            }
        }

    }
}
