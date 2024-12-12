﻿using System.ComponentModel.DataAnnotations;

namespace CEC_CRM.models
{
    public class OrderDetail
    {
        [Key]
        public int OrderId { get; set; } // Auto-incremented primary key
        public string UserEmail { get; set; } // Foreign key from Users table
        public string OrderItems { get; set; }

    }
}
