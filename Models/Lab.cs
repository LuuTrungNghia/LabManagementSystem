using System;
using System.Collections.Generic;

namespace LabManagementSystem.Models
{
    public class Lab
    {
        public int LabId { get; set; }
        public string LabName { get; set; }
        public string Location { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public ICollection<LabBorrowingRequest> LabBorrowingRequests { get; set; }
    }
}
