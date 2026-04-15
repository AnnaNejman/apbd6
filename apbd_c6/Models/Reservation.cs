namespace apbd_c6.Models;

public class Reservation
{
    public int id {get; set;}
    public int roomId {get; set;}
    public string organizerName {get; set;}
    public string topic {get; set;}
    public DateOnly date {get; set;}
    public DateTime startTime {get; set;}
    public DateTime endTime {get; set;}
    public ReservationStatus status {get; set;}
}

public enum ReservationStatus
{
    planned, confirmed, cancelled
}