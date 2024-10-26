using System;
using System.Collections.Generic;

namespace LabManagementSystem.Models
{
    public class Lecturer
    {
        public int LecturerId { get; set; }
        public string LecturerName { get; set; }
        public string Department { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public ICollection<LabBorrowingRequest> LabBorrowingRequests { get; set; }
    }
}
