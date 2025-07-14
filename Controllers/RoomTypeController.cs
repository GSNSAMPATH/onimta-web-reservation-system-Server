using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using OIT_Reservation.Models;
using OIT_Reservation.Services;
using System.Linq;

namespace OIT_Reservation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomTypeController : ControllerBase
    {
        private readonly RoomTypeService _service;

        public RoomTypeController(RoomTypeService service)
        {
            _service = service;
        }

        // GET: api/roomtype/getall
        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            var result = _service.GetAll();
            return Ok(result);
        }

        // POST: api/roomtype/add
        [HttpPost("add")]
        public IActionResult Create([FromBody] RoomType roomType)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new { success = false, message = "Validation failed", errors });
            }

            try
            {
                bool success = _service.Create(roomType);
                if (success)
                    return Ok(new { success = true, message = "Room type created successfully." });
                else
                    return BadRequest(new { success = false, message = "Failed to create room type." });
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Internal server error: " + ex.Message });
            }
        }

        // PUT: api/roomtype/update/5
        [HttpPut("update/{id}")]
        public IActionResult Update(int id, [FromBody] RoomType roomType)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new { success = false, message = "Validation failed", errors });
            }

            try
            {
                roomType.RoomTypeID = id; // Use ID from URL
                bool updated = _service.Update(roomType);

                if (updated)
                    return Ok(new { success = true, message = "Room type updated successfully." });
                else
                    return NotFound(new { success = false, message = "Room type not found." });
            }
            catch (SqlException ex)
            {
                return BadRequest(new { success = false, message = "SQL Error: " + ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Internal server error: " + ex.Message });
            }
        }

        // Uncomment and implement delete if needed
        /*
        // DELETE: api/roomtype/delete/5
        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var success = _service.Delete(id);
                if (!success)
                    return NotFound(new { success = false, message = "Room type not found." });

                return Ok(new { success = true, message = "Room type deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Server error: " + ex.Message });
            }
        }
        */
    }
}
