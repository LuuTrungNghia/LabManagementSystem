namespace LabManagementSystem.Dtos
{
    public class ArRequestRoomBookingDto
    {
        public string Status { get; set; } = default!;
        public int[] RoomBookingIds { get; set; } = default!;
    }
}