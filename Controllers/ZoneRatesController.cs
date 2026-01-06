using Logex.API.Constants;
using Logex.API.Dtos.ZoneRateDtos;
using Logex.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Logex.API.Controllers
{
    [Authorize(IdentityRoles.Admin)]
    [Route("api/[controller]")]
    [ApiController]
    public class ZoneRatesController : ControllerBase
    {
        private readonly IZoneRateService _zoneRateService;

        public ZoneRatesController(IZoneRateService zoneRateService)
        {
            _zoneRateService = zoneRateService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var rates = await _zoneRateService.GetAllAsync();
            return Ok(rates);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var rate = await _zoneRateService.GetByIdAsync(id);
                return Ok(rate);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "Pricing rule not found." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateZoneRateDto request)
        {
            try
            {
                var rate = await _zoneRateService.CreateRateAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = rate.Id }, rate);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateZoneRateDto request)
        {
            try
            {
                var rate = await _zoneRateService.UpdateRateAsync(id, request);
                return Ok(rate);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "Pricing rule not found." });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _zoneRateService.DeleteRateAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "Pricing rule not found." });
            }
        }
    }
}
