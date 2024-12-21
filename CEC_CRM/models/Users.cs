using System.ComponentModel.DataAnnotations;

namespace CEC_CRM.models
{
    public class Users
    {
        [Key]
        public int user_id { get; set; } 
       
        [Required]
        public string name { get; set; }

        [Required]
        [EmailAddress]
        public string email { get; set; }


        public string? password { get; set; }

        public long? phone_number { get; set; }
    }
}
