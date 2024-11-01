namespace LabManagementSystem.Dtos
{
    public class RequestRoomBookingDto
    {
        public int UserId { get; set; }
        public int LabId { get; set; }
        public string RoomName { get; set; } = default!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = default!;
    }
}