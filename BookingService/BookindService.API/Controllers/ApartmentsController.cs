using BookingService.Application.Abstract;
using BookingService.Application.DTOs;
using BookingService.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.API.Controllers
{
    [Route("api/apartments")]
    [ApiController]
    public class ApartmentsController : ControllerBase
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
            var hostId = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            if (hostId == null )
            {
                return BadRequest("User is not authorized");
            }
            var result = await _apartmentService.CreateApartmentAsync(hostId, apartmentDto);
            return Ok($"Apartment is successfully created with id - {result.Value}");
        }
    }
}