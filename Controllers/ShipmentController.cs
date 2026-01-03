using Logex.API.Dtos.ShipmentDtos;
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

                var shipmentDTO = new ShipmentDTO
                {
                    ShipmentId = shipemnt.Id,
                    ShipperName = shipemnt.ShipperName,
                    ReceiverName = shipemnt.ReceiverName,
                    CreatedAt = shipemnt.CreatedAt,
                    TotalCost = shipemnt.TotalCost,
                    Status = shipemnt.Status!,
                };

                return Ok(shipmentDTO);
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

                var shipmentDTO = new ShipmentDTO
                {
                    ShipmentId = shipemnt.Id,
                    ShipperName = shipemnt.ShipperName,
                    ReceiverName = shipemnt.ReceiverName,
                    TotalCost = shipemnt.TotalCost,
                    CreatedAt = shipemnt.CreatedAt,
                    Status = shipemnt.Status,
                };

                return Ok(shipmentDTO);
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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateShipmentDto createShipmentDto)
        {
            try
            {
                var username = User.Identity?.Name;
                if (string.IsNullOrEmpty(username))
                {
                    return Unauthorized("User is not authenticated.");
                }

                var user = await _userManagement.GetUserByEmail(username);
                if (user == null)
                {
                    return Unauthorized("User not found.");
                }

                var shipment = await _shipmentService.CreateShipmentAsync(
                    createShipmentDto,
                    user.Id
                );

                return CreatedAtAction(nameof(GetById), new { id = shipment.Id }, shipment);
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

        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateShipmentDto updateShipmentDto
        )
        {
            try
            {
                var updatedShipment = await _shipmentService.UpdateAsync(id, updateShipmentDto);
                return Ok(updatedShipment);
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

        [Authorize]
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
