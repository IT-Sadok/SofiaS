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
            var result = await _apartmentService.CreateApartmentAsync(apartmentDto);
            return Ok($"Apartment is successfully created with id - {result.Value}");
        }
    }
}