﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LabManagementSystem.Models
{
    public class Lab
    {
        [Key]
        public int LabId { get; set; }

        [Required(ErrorMessage = "Lab Name is required.")]
        public string LabName { get; set; }

        public string Location { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public virtual ICollection<LabBorrowingRequest> LabBorrowingRequests { get; set; } = new List<LabBorrowingRequest>();
        public virtual ICollection<RoomBookingRequest> RoomBookingRequests { get; set; } = new List<RoomBookingRequest>();
    }
}
