using System;
using System.Collections.Generic;

namespace LabManagementSystem.Models
{
    public class User : EntityBase
    {
        public int UserId { get; set; }
        
        public string UserName { get; set; }
        public string Email { get; set; }
        public int Status { get; set; }
        
        public bool IsApproved { get; set; } = false;

        public ICollection<DeviceBorrowingRequest>? DeviceBorrowingRequests { get; set; }
        
        public ICollection<LabBorrowingRequest>? LabBorrowingRequests { get; set; }
        
        public ICollection<RoomBookingRequest>? RoomBookingRequests { get; set; }
        
    }
}