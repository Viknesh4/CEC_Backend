using System.ComponentModel.DataAnnotations;
using System.Data;

namespace CEC_CRM.models
{
    public class Admin
    {
        [Key]
        public int admin_id { get; set; }
        public string name {  get; set; }
        public string email {  get; set; }
        [MinLength(8)]
        public string password {  get; set; }
        public string admin_type {  get; set; }
    }
}
