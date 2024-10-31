namespace LabManagementSystem.Dtos;

public class RequestBookingDeviceDto
{
    public int UserId { get; set; }
    
    public int[] DeviceIds { get; set; }
    
    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string Status { get; set; }

    public int Quantity { get; set; }
    
    
}