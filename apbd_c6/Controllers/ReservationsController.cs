using apbd_c6.DTOs;
using apbd_c6.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace apbd_c6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private static List<Reservation> _reservations = new List<Reservation>
        {
            new Reservation
            {
                id = 1, organizerName = "FirmaA", roomId = 1, topic = "aaa", status = ReservationStatus.confirmed, 
                date = new DateOnly(2026,04,19), 
                startTime = new DateTime(2026, 4, 19, 10, 30, 0),
                endTime = new DateTime(2026, 4, 19, 19, 0, 0)
            },
            new Reservation
            {
                id = 2, organizerName = "FirmaB", roomId = 1, topic = "bbb", status = ReservationStatus.planned, 
                date = new DateOnly(2026,6,20), 
                startTime = new DateTime(2026, 6, 20, 8, 0, 0),
                endTime = new DateTime(2026, 6, 20, 20, 0, 0)
            },
            new Reservation
            {
                id = 3, organizerName = "FirmaA", roomId = 3, topic = "aaa", status = ReservationStatus.planned, 
                date = new DateOnly(2026,12,12), 
                startTime = new DateTime(2026, 12, 12, 8, 0, 0),
                endTime = new DateTime(2026, 12, 12, 20, 0, 0)
            },
            new Reservation
            {
                id = 4, organizerName = "FirmaC", roomId = 2, topic = "gg", status = ReservationStatus.cancelled, 
                date = new DateOnly(2026,1,10), 
                startTime = new DateTime(2026, 1, 10, 10, 0, 0),
                endTime = new DateTime(2026, 1, 10, 12, 45, 0)
            }
            
            
        }; 
        // [HttpGet]
        // public IActionResult GetAll()
        // {
        //     return Ok(_reservations);
        // }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var reservation = _reservations.FirstOrDefault(r => r.id == id);
            if (reservation == null)
            {
                return NotFound($"Reservation with id {id} not found");
            }
            return Ok(reservation);
        }
        [HttpGet]
        public IActionResult GetReservations(
            DateOnly? date,
            ReservationStatus? status,
            int? roomId)
        {
            var query = _reservations.AsQueryable();

            if (date.HasValue)
                query = query.Where(r => r.date == date.Value);

            if (status.HasValue)
                query = query.Where(r => r.status == status.Value);

            if (roomId.HasValue)
                query = query.Where(r => r.roomId == roomId.Value);

            return Ok(query.ToList());
        }
        [HttpPost]
        public IActionResult CreateReservation([FromBody] Reservation newReservation)
        {
            if (newReservation == null)
            {
                return BadRequest("No data");
            }
            if (!RoomsController._rooms.Any(r => r.id == newReservation.roomId))
            {
                return BadRequest("No room found");
            }

            if (!RoomsController._rooms.First(r => r.id == newReservation.roomId).isActive)
            {
                return BadRequest("Inactive room");
            }
            bool conflict = _reservations.Any(r =>
                r.roomId == newReservation.roomId &&
                r.date == newReservation.date &&
                newReservation.startTime < r.endTime &&
                newReservation.endTime > r.startTime
            );
            if (conflict)
            {
                return Conflict("Time overlap");
            }

            newReservation.id = _reservations.Any() ? _reservations.Max(r => r.id) + 1 : 1;
            _reservations.Add(newReservation);

            return CreatedAtAction(nameof(GetById), new { id = newReservation.id }, newReservation);
        }
        [HttpPut("{id:int}")]
        public IActionResult UpdateReservation(
            [FromRoute] int id,
            [FromBody] UpdateReservationDto dto)
        {
            var reservation = _reservations.FirstOrDefault(r => r.id == id);

            if (reservation == null)
            {
                return NotFound($"Reservation with id {id} not found");
            }

            reservation.organizerName = dto.OrganizerName;
            reservation.roomId = dto.RoomId;
            reservation.topic = dto.Topic;
            reservation.status = dto.Status;
            reservation.date = dto.Date;
            reservation.startTime = dto.StartTime;

            return Ok(reservation);
        }
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var reservation = _reservations.FirstOrDefault(r => r.id == id);

            if (reservation == null)
            {
                return NotFound("No reservation like that");
            }

            _reservations.Remove(reservation);

            return NoContent();
        }
    }
    //Najważniejsze. W zadaniu muszą pojawić się różne sposoby przekazywania danych: id i buildingCode z trasy, filtry z query stringa oraz dane obiektów z body żądania w formacie JSON.
}
