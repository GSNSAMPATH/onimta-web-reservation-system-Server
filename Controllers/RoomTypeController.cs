using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using OIT_Reservation.Models;
using OIT_Reservation.Services;

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

        //GET: api/roomtype
        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            var result = _service.GetAll();
            return Ok(result);
        }

        // // GET: api/roomtype/5
        // [HttpGet("{id}")]
        // public IActionResult GetById(int id)
        // {
        //     var roomType = _service.GetById(id);
        //     if (roomType == null)
        //         return NotFound("Room type not found.");
        //     return Ok(roomType);
        // }

        // POST: api/roomtype[HttpPost]
    [HttpPost("add")]
    public IActionResult Create([FromBody] RoomType roomType)
        {
            try
            {
                bool success = _service.Create(roomType);
                return Ok("Room type created successfully.");
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message); // Custom error from SQL
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        // // PUT: api/roomtype/5
[HttpPut("Update/{id}")]
public IActionResult Update(int id, [FromBody] RoomType roomType)
{
    try
    {
        roomType.RoomTypeID = id; // Set ID from route

        bool updated = _service.Update(roomType);
        if (updated)
            return Ok("Room type updated successfully.");
        else
            return NotFound("Room type not found.");
    }
    catch (SqlException ex)
    {
        return BadRequest($"SQL Error: {ex.Message}");
    }
    catch (Exception ex)
    {
        return StatusCode(500, $"Internal server error: {ex.Message}");
    }
}



        //DELETE: api/roomtype/Delete/5
        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var success = _service.Delete(id);
                if (!success)
                    return NotFound("Room type not found.");

                return Ok("Room type deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }
    }
}
