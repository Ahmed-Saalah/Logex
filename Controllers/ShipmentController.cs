using System.Security.Claims;
using Logex.API.Constants;
using Logex.API.Dtos.ShipmentDtos;
using Logex.API.Models;
using Logex.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Logex.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipmentController : ControllerBase
    {
        private readonly IShipmentService _shipmentService;
        private readonly IUserManagement _userManagement;
        private readonly IPricingService _pricingService;

        public ShipmentController(
            IShipmentService shipmentService,
            IUserManagement userManagement,
            IPricingService pricingService
        )
        {
            _shipmentService = shipmentService;
            _userManagement = userManagement;
            _pricingService = pricingService;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var shipemnt = await _shipmentService.GetByIdAsync(id);

                if (shipemnt == null)
                {
                    return NotFound(new { Message = "shipemnt not found." });
                }

                var response = new ShipmentResponseDto
                {
                    Id = shipemnt.Id,
                    TrackingNumber = shipemnt.TrackingNumber,
                    Status = shipemnt.Status,
                    TotalCost = shipemnt.TotalCost,
                    CreatedAt = shipemnt.CreatedAt,
                    ShipmentMethod = shipemnt.ShipmentMethod.Name,
                    ShipperName = shipemnt.ShipperName,
                    ReceiverName = shipemnt.ReceiverName,
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new
                    {
                        Message = "An error occurred while processing the request.",
                        Error = ex.Message,
                    }
                );
            }
        }

        [HttpGet("tracking/{trackingNumber}")]
        public async Task<IActionResult> GetByTrackingNumber(string trackingNumber)
        {
            try
            {
                var shipemnt = await _shipmentService.GetByTrackingNumberAsync(trackingNumber);

                if (shipemnt == null)
                {
                    return NotFound(new { Message = "shipemnt not found." });
                }

                var response = new ShipmentResponseDto
                {
                    Id = shipemnt.Id,
                    TrackingNumber = shipemnt.TrackingNumber,
                    Status = shipemnt.Status,
                    TotalCost = shipemnt.TotalCost,
                    CreatedAt = shipemnt.CreatedAt,
                    ShipmentMethod = shipemnt.ShipmentMethod.Name,
                    ShipperName = shipemnt.ShipperName,
                    ReceiverName = shipemnt.ReceiverName,
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new
                    {
                        Message = "An error occurred while processing the request.",
                        Error = ex.Message,
                    }
                );
            }
        }

        [Authorize(Roles = IdentityRoles.Customer)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateShipmentDto createShipmentDto)
        {
            try
            {
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (
                    string.IsNullOrEmpty(userIdString)
                    || !int.TryParse(userIdString, out int userId)
                )
                {
                    return Unauthorized("User ID claim is missing or invalid.");
                }

                var shipment = await _shipmentService.CreateShipmentAsync(
                    createShipmentDto,
                    userId
                );

                var response = new ShipmentResponseDto
                {
                    Id = shipment.Id,
                    TrackingNumber = shipment.TrackingNumber,
                    Status = shipment.Status,
                    TotalCost = shipment.TotalCost,
                    CreatedAt = shipment.CreatedAt,
                    ShipmentMethod = shipment.ShipmentMethod.Name,
                    ShipperName = shipment.ShipperName,
                    ReceiverName = shipment.ReceiverName,
                };

                return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    new { Message = "An error occurred while creating the shipment." }
                );
            }
        }

        [Authorize(Roles = IdentityRoles.Customer)]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateShipmentDto updateShipmentDto
        )
        {
            try
            {
                var updatedShipment = await _shipmentService.UpdateAsync(id, updateShipmentDto);
                var response = new ShipmentResponseDto
                {
                    Id = updatedShipment.Id,
                    TrackingNumber = updatedShipment.TrackingNumber,
                    Status = updatedShipment.Status,
                    TotalCost = updatedShipment.TotalCost,
                    CreatedAt = updatedShipment.CreatedAt,
                    ShipmentMethod = updatedShipment.ShipmentMethod.Name,
                    ShipperName = updatedShipment.ShipperName,
                    ReceiverName = updatedShipment.ReceiverName,
                };
                return Ok(response);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "Shipment not found." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("calculate-rate")]
        public async Task<IActionResult> CalculateRate([FromBody] CreateShipmentDto dto)
        {
            try
            {
                var cost = await _pricingService.CalculateShipmentTotalAsync(
                    dto.ShipperCityId,
                    dto.ReceiverCityId,
                    dto.Quantity,
                    dto.Weight,
                    dto.ShipmentMethodId
                );

                return Ok(new { EstimatedCost = cost, Currency = "EGP" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [Authorize(Roles = IdentityRoles.Admin)]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _shipmentService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new
                    {
                        Message = "An error occurred while deleting the shipment.",
                        Error = ex.Message,
                    }
                );
            }
        }
    }
}
