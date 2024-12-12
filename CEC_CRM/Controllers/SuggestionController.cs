using CEC_CRM.Data;
using CEC_CRM.models;
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
    }
}
