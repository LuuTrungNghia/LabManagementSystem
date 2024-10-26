using System;
using System.ComponentModel.DataAnnotations;

namespace LabManagementSystem.Models
{
    public class RoomBookingRequest
    {
        [Key]
        public int BookingId { get; set; }

        [Required(ErrorMessage = "Room Name is required.")]
        public string RoomName { get; set; }

        [Required(ErrorMessage = "Start Date is required.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End Date is required.")]
        public DateTime EndDate { get; set; }

        public string UserId { get; set; }
        public string Status { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
