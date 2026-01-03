using Logex.API.Dtos.ZoneDtos;
using Logex.API.Services.Implementations;
using Logex.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Logex.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZonesController : ControllerBase
    {
        private readonly IZoneService _zoneService;

        public ZonesController(IZoneService zoneService)
        {
            _zoneService = zoneService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var zones = await _zoneService.GetAllAsync();
            return Ok(zones);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var zone = await _zoneService.GetByIdAsync(id);
                return Ok(zone);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "Zone not found." });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ZoneDto request)
        {
            try
            {
                var zone = await _zoneService.CreateZoneAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = zone.Id }, zone);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ZoneDto request)
        {
            try
            {
                var zone = await _zoneService.UpdateZoneAsync(id, request);
                return Ok(zone);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "Zone not found." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("{id}/toggle-status")]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            try
            {
                var newStatus = await _zoneService.ToggleStatusAsync(id);
                return Ok(
                    new
                    {
                        Message = "Status updated successfully.",
                        IsActive = newStatus,
                        MethodId = id,
                    }
                );
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "Zone not found." });
            }
        }
    }
}
