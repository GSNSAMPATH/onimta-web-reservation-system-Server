using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using OIT_Reservation.Models;
using OIT_Reservation.Services;

namespace OIT_Reservation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TravelAgentController : ControllerBase
    {
        private readonly TravelAgentService _service;

        public TravelAgentController(TravelAgentService service)
        {
            _service = service;
        }

        // POST: api/travelagent[HttpPost]
        [HttpPost("add")]
        public IActionResult Create([FromBody] TravelAgent travelAgent)
        {
            try
            {
                bool success = _service.Create(travelAgent);
                return Ok(new
                {
                    message = "Travel agent created successfully.",
                    generatedCode = travelAgent.TravelAgentCode
                });
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
