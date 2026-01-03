using Logex.API.Dtos.CityDtos;
using Logex.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Logex.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly ICityService _cityService;

        public CitiesController(ICityService cityService)
        {
            _cityService = cityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cities = await _cityService.GetAllAsync();
            return Ok(cities);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var city = await _cityService.GetByIdAsync(id);
                return Ok(city);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "City not found." });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCityDto request)
        {
            try
            {
                var city = await _cityService.CreateCityAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = city.Id }, city);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCityDto request)
        {
            try
            {
                var updatedCity = await _cityService.UpdateCityAsync(id, request);
                return Ok(updatedCity);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "City not found." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _cityService.DeleteCityAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "City not found." });
            }
        }
    }
}
