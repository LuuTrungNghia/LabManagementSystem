using System;
using System.Collections.Generic;

namespace LabManagementSystem.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string Class { get; set; }

        public int ResponsibleLecturerId { get; set; } 
        public virtual Lecturer ResponsibleLecturer { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public ICollection<LabBorrowingRequest> LabBorrowingRequests { get; set; }
    }
}
