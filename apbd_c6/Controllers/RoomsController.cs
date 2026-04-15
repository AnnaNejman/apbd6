using apbd_c6.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace apbd_c6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        public static List<Room> _rooms = new List<Room>
        {
            new Room
            {
                id = 1, name = "Sekretariat", buildingCode = "A12", floor = 1, capacity = 20,
                hasProjector = true, isActive = true
            },
            new Room
            {
                id = 2, name = "SalaWykladowa", buildingCode = "B6", floor = 2, capacity = 160,
                hasProjector = true, isActive = true
            },
            new Room
            {
                id = 3, name = "Inne", buildingCode = "A2", floor = 0, capacity = 6,
                hasProjector = false, isActive = false
            },
            new Room
            {
                id = 4, name = "aha", buildingCode = "A1", floor = 0, capacity = 20,
                hasProjector = true, isActive = true
            },
        };  
        // [HttpGet]
        // public IActionResult GetAll()
        // {
        //     return Ok(_rooms);
        // }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var room = _rooms.FirstOrDefault(r => r.id == id);
            if (room == null)
            {
                return NotFound($"Room with id {id} not found");
            }
            return Ok(room);
        }
        [HttpGet("building/{buildingCode}")]
        public IActionResult GetAllByBuildingCode(string buildingCode)
        {
            var rooms = _rooms.Where(r => r.buildingCode == buildingCode).ToList();
            if (!rooms.Any())
            {
                return NotFound($"Rooms with buildingCode {buildingCode} not found");
            }
            return Ok(rooms);
        }
        
        [HttpGet]
        public IActionResult GetRooms(
            int? minCapacity,
            bool? hasProjector,
            bool? activeOnly)
        {
            var query = _rooms.AsQueryable();

            if (minCapacity.HasValue)
                query = query.Where(r => r.capacity >= minCapacity.Value);

            if (hasProjector.HasValue)
                query = query.Where(r => r.hasProjector == hasProjector.Value);

            if (activeOnly == true)
                query = query.Where(r => r.isActive);

            return Ok(query.ToList());
        }
        
        [HttpPost]
        public IActionResult CreateRoom([FromBody] Room newRoom)
        {
            if (newRoom == null)
            {
                return BadRequest("No data");
            }
            
            newRoom.id = _rooms.Any() ? _rooms.Max(r => r.id) + 1 : 1;
            _rooms.Add(newRoom);

            return CreatedAtAction(nameof(GetById), new { id = newRoom.id }, newRoom);
        }
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var room = _rooms.FirstOrDefault(r => r.id == id);

            if (room == null)
            {
                return NotFound("No room like that");
            }

            _rooms.Remove(room);

            return NoContent();
        }
    }
}
