using System.ComponentModel.DataAnnotations;

namespace CEC_CRM.models
{
    public class Suggestion
    {
        [Key]
        public int suggestion_id { get; set; }  // Primary Key (Auto-increment)
        public int user_id { get; set; }         // Foreign Key to Users Table
        public string title { get; set; }       // Title of the suggestion
        public string description { get; set; } // Detailed description of the suggestion
        
    }
}
