using Logex.API.Constants;
using Logex.API.Dtos.ShipmentMethodDtos;
using Logex.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Logex.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipmentMethodsController : ControllerBase
    {
        private readonly IShipmentMethodService _shipmentMethodService;

        public ShipmentMethodsController(IShipmentMethodService shipmentMethodService)
        {
            _shipmentMethodService = shipmentMethodService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var method = await _shipmentMethodService.GetByIdAsync(id);
            if (method == null)
            {
                return NotFound(new { Message = "Shipment method not found." });
            }
            var response = new ShipmentMethodResponseDto
            {
                Id = method.Id,
                Name = method.Name,
                Cost = method.Cost,
                Description = method.Description,
                IsActive = method.IsActive,
                Duration = method.Duration,
                CreatedAt = method.CreatedAt,
            };

            return Ok(response);
        }

        [HttpGet("{id}/cost")]
        public async Task<IActionResult> GetCost(int id)
        {
            try
            {
                var cost = await _shipmentMethodService.GetShipmentMethodCostAsync(id);
                return Ok(new { MethodId = id, CostPerKg = cost });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new { Message = "Pricing configuration error.", Details = ex.Message }
                );
            }
        }

        [Authorize(Roles = IdentityRoles.Admin)]
        [HttpGet("admin/all")]
        public async Task<IActionResult> GetAll()
        {
            var methods = await _shipmentMethodService.GetAllAsync();
            return Ok(methods);
        }

        [Authorize(Roles = IdentityRoles.Admin)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateShipmentMethodDto request)
        {
            var createdMethod = await _shipmentMethodService.CreateMethodAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = createdMethod.Id }, createdMethod);
        }

        [Authorize(Roles = IdentityRoles.Admin)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateShipmentMethodDto request)
        {
            try
            {
                var updatedMethod = await _shipmentMethodService.UpdateMethodAsync(id, request);
                var response = new ShipmentMethodResponseDto
                {
                    Id = updatedMethod.Id,
                    Name = updatedMethod.Name,
                    Cost = updatedMethod.Cost,
                    Description = updatedMethod.Description,
                    IsActive = updatedMethod.IsActive,
                    Duration = updatedMethod.Duration,
                    CreatedAt = updatedMethod.CreatedAt,
                };
                return Ok(response);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = $"Shipment method with ID {id} not found." });
            }
        }

        [Authorize(Roles = IdentityRoles.Admin)]
        [HttpPatch("{id}/toggle-status")]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            try
            {
                var newStatus = await _shipmentMethodService.ToggleStatusAsync(id);
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
                return NotFound(new { Message = "Method not found." });
            }
        }
    }
}
