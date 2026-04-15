using apbd_c6.Models;

namespace apbd_c6.DTOs;

public class UpdateReservationDto
{
    public string OrganizerName { get; set; }
    public int RoomId { get; set; }
    public string Topic { get; set; }
    public ReservationStatus Status { get; set; }
    public DateOnly Date { get; set; }
    public DateTime StartTime { get; set; }
}