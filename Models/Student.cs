using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LabManagementSystem.Models
{
    public class Student : EntityBase
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string Class { get; set; }

        [ForeignKey("Lecturer")]
        public int ResponsibleLecturerId { get; set; }
        public virtual Lecturer ResponsibleLecturer { get; set; }

        public ICollection<LabBorrowingRequest> LabBorrowingRequests { get; set; }
    }
}