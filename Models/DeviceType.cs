namespace LabManagementSystem.Models
{
    public class DeviceType
    {
        public int DeviceTypeId { get; set; }
        public string TypeName { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
