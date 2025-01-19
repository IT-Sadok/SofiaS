using BookingService.Application.Abstract;
using BookingService.Application.DTOs;
using BookingService.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookingService.API.Controllers
{
    [Route("api/apartments")]
    [ApiController]
    public class ApartmentsController : BaseController
    {
        private readonly IApartmentService _apartmentService;

        public ApartmentsController(IApartmentService apartmentService)
        {
            _apartmentService = apartmentService;
        }

        [HttpPost]
        [Authorize(Roles = Roles.Host)]
        public async Task<ActionResult> CreateApartment([FromBody] ApartmentCreateDto apartmentDto)
        {
            var hostId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (hostId == null)
            {
                return BadRequest("Unable to identify the user from the provided token.");
            }
            var result = await _apartmentService.CreateApartmentAsync(hostId, apartmentDto);

            return Ok(new { 
                ApartmentId = result.Value, 
                Message = "Apartment is successfully created." }
            );
        }

        [HttpPost("{apartmentId}")]
        [Authorize(Roles = Roles.Host)]
        public async Task<IActionResult> UpsertCustomData(int apartmentId, [FromBody] List<ApartmentUpsertDto> customData)
        {
            try
            {
                var hostId = GetUserIdFromToken();
                var result = await _apartmentService.UpsertCustomDataAsync(apartmentId, hostId, customData);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}