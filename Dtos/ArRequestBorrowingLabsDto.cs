namespace LabManagementSystem.Dtos
{
    public class ArRequestBorrowingLabsDto
    {
        public string Status { get; set; } = default!;
        public int[] LabBorrowingRequestIds { get; set; } = default!;
    }
}
