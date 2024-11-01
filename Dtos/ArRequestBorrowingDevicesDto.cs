using System.ComponentModel.DataAnnotations;

namespace LabManagementSystem.Dtos;

public class ArRequestBorrowingDevicesDto
{
    [Required]
    public string Status { get; set; } = default!;
    public int[] ArRequestIds { get; set; } = default!;
}