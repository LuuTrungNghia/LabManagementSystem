using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LabManagementSystem.Models
{
    public class DeviceType : EntityBase
    {
        public int DeviceTypeId { get; set; }

        [Required(ErrorMessage = "Type Name is required.")]
        public string TypeName { get; set; }

        public string? Description { get; set; } = default!;
        
        public bool? Active { get; set; } =  true;

        public ICollection<Device>? Devices { get; set; }
    }
}