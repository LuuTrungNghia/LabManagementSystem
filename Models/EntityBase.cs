namespace LabManagementSystem.Models;

public class EntityBase
{
    public string? CreatedBy { get; set; } = default!;
    public string? UpdatedBy { get; set; } = default!;
    public DateTime? CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; } = DateTime.Now;
}