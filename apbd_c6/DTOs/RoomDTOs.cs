namespace apbd_c6.DTOs;

public class RoomDTOs
{
    private int id { get; set; }
    private string name { get; set; } = string.Empty;
    private string buildingCode { get; set; } = string.Empty;
    private int floor { get; set; }
    private float capacity { get; set; }
    private bool hasProjector { get; set; }
    private bool isActive { get; set; } = true;
}