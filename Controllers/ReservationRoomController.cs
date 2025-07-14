using Microsoft.AspNetCore.Mvc;
using OIT_Reservation.Models;
using OIT_Reservation.Services;

namespace OIT_Reservation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly IReservationRoomService _service;

        public RoomController(IReservationRoomService service)
        {
            _service = service;
        }

        [HttpPost("save")]
        public IActionResult SaveRoom([FromBody] ReservationRoom room)
        {
            try
            {
                var roomCode = _service.SaveRoom(room);
                return Ok(new { success = true, message = "Room saved successfully", roomCode });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
    }
}
