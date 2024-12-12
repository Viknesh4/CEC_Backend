using System.ComponentModel.DataAnnotations;

namespace CEC_CRM.models
{
    public class Ticket
    {
        [Key]
        public int ticket_id { get; set; } // Auto-incremented primary key
        public int customer_id { get; set; } // Foreign key to Users table
        public int order_id { get; set; }

        public string admin_type { get; set; }
        public int? assigned_admin_id { get; set; } // Nullable, Foreign key to Admin table
        public string title { get; set; } // Query title
        public string description { get; set; } // Query description
        public string status { get; set; } // Query status: 'new', 'in_progress', 'resolved'
        public DateTime updated_at { get; set; } = DateTime.Now;// Updated timestamp
        public int priority { get; set; }
        public string? message { get; set; }

    }
}
