namespace CEC_CRM.models
{
    public class TicketUpdateRequest
    {
        public string? status { get; set; }
        public string? message { get; set; }
        public int? assigned_admin_id { get; set; }

    }
}


