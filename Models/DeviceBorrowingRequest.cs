using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LabManagementSystem.Models
{
    public class DeviceBorrowingRequest : EntityBase
    {
        [Key]
        public int RequestId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("Device")]
        public int DeviceId { get; set; }
        public virtual Device Device { get; set; }

        [Required(ErrorMessage = "Start Date is required.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End Date is required.")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public string Status { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        public int Quantity { get; set; }
        
    }
}