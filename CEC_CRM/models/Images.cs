using System.ComponentModel.DataAnnotations;

namespace CEC_CRM.models
{
    public class Images
    {
        [Key]
        public int? Id { get; set; }   // Auto-incrementing primary key
        public int TicketId { get; set; }  // Foreign Key referencing Ticket
        public byte[] ImageData { get; set; } // Store the image as byte array

    }
}
