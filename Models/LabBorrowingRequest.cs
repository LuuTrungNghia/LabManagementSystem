using System;
using System.ComponentModel.DataAnnotations;

namespace LabManagementSystem.Models
{
    public class LabBorrowingRequest
    {
        [Key]
        public int RequestId { get; set; }

        public int UserId { get; set; }
        public int LabId { get; set; }

        [Required(ErrorMessage = "Start Date is required.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End Date is required.")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public string Status { get; set; }

        [Required(ErrorMessage = "Reason is required.")]
        public string Reason { get; set; }

        public int ResponsibleLecturerId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public virtual User User { get; set; }
        public virtual Lab Lab { get; set; }
    }
}
