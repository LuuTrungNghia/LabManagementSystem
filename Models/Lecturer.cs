using System;
using System.Collections.Generic;

namespace LabManagementSystem.Models
{
    public class Lecturer : EntityBase
    {
        public int LecturerId { get; set; }
        public string LecturerName { get; set; }
        public string Department { get; set; }
        public ICollection<LabBorrowingRequest> LabBorrowingRequests { get; set; }
    }
}