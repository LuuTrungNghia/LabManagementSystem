
public class RequestBorrowingLabsDto
{
    public int UserId { get; set; }
    public int LabId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Reason { get; set; }
    public int ResponsibleLecturerId { get; set; }
    public int UserType { get; set; }
}