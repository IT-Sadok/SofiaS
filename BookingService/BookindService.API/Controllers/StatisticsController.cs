using BookingService.Application.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.API.Controllers
{
    [Route("api/statistics")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticService _statisticService;

        public StatisticsController(IStatisticService statisticService)
        {
            _statisticService = statisticService;
        }

        [HttpGet("hosts-profit")]
        public async Task<IActionResult> GetHostsWithProfit()
        {
            var result = await _statisticService.GetHostsProfit();
            return Ok(result.Value.ToList());
        }

        [HttpGet("top-5-apartments")]
        public async Task<IActionResult> GetTopMostBookedApartment()
        {
            var result = await _statisticService.GetTop5MostBookedApartment();
            return Ok(result.Value.ToList());
        }

        [HttpGet("apartment-price-quantiles")]
        public async Task<IActionResult> GetApartmentPriceQuantiles()
        {
            var result = await _statisticService.GetApartmentPriceQuantiles();
            return Ok(result.Value.ToList());
        }

        [HttpGet("repeated-bookings")]
        public async Task<IActionResult> GetRepeatedBookingPerApartmentClient()
        {
            var result = await _statisticService.GetRepeatedBookingPerApartmentClient();
            return Ok(result.Value.ToList());
        }

        [HttpGet("avg-booking-duration-per-apartment")]
        public async Task<IActionResult> GetAverageBookingDurationPerApartment()
        {
            var result = await _statisticService.GetAverageBookingDurationPerApartment();
            return Ok(result.Value.ToList());
        }

    }
}