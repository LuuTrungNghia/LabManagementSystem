using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LabManagementSystem.Models
{
    public class Device: EntityBase
    {
        public int DeviceId { get; set; }

        [Required(ErrorMessage = "Device Name is required.")]
        public string DeviceName { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public int Status { get; set; }

        public bool IsAvailable { get; set; }
        

        [ForeignKey("DeviceType")]
        public int DeviceTypeId { get; set; }
        public DeviceType? DeviceType { get; set; } 
        public ICollection<DeviceBorrowingRequest>? DeviceBorrowingRequests { get; set; }
    }
}
