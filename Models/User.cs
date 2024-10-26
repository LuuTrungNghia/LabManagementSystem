using System;
using System.Collections.Generic;

namespace LabManagementSystem.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public ICollection<DeviceBorrowingRequest> DeviceBorrowingRequests { get; set; }
        public ICollection<LabBorrowingRequest> LabBorrowingRequests { get; set; }
    }
}
