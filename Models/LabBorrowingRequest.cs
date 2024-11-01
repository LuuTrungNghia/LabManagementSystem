using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LabManagementSystem.Models
{
    public class LabBorrowingRequest : EntityBase
    {
        [Key]
        public int RequestId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        
        [ForeignKey("Lab")]
        public int LabId { get; set; }

        [Required(ErrorMessage = "Start Date is required.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End Date is required.")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public string Status { get; set; }

        [Required(ErrorMessage = "Reason is required.")]
        public string Reason { get; set; }

        [ForeignKey("Lecturer")]
        public int ResponsibleLecturerId { get; set; }
        
        public int UserType { get; set; }
        
        public virtual Lecturer ResponsibleLecturer { get; set; }
        public virtual User User { get; set; }
        public virtual Lab Lab { get; set; }
        
    }
}