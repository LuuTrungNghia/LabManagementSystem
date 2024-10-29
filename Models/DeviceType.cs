using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LabManagementSystem.Models
{
    public class DeviceType
    {
        public int DeviceTypeId { get; set; }

        [Required(ErrorMessage = "Type Name is required.")]
        public string TypeName { get; set; } 

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public ICollection<Device> Devices { get; set; }
    }
}