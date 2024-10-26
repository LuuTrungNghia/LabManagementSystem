using System;
using System.ComponentModel.DataAnnotations;

namespace LabManagementSystem.Models
{
    public class Device
    {
        public int DeviceId { get; set; }

        [Required(ErrorMessage = "Device Name is required.")]
        public string DeviceName { get; set; }

        [Required(ErrorMessage = "Device Type is required.")]
        public string DeviceType { get; set; } 

        [Required(ErrorMessage = "Quantity is required.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public string Status { get; set; }

        public bool IsAvailable { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
