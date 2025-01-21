using BookingService.Application.DTOs;
using BookingService.Domain.DTOs;
using BookingService.Domain.Entities;
using BookingService.Domain.Interfaces;
using BookingService.Infrastructure.Database;
using BookingService.Infrastructure.Resources;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Infrastructure.Repository
{
    public class BookingRepository : IBookingRepository
    {
        private readonly AppDbContext _context;

        public BookingRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<int> CreateAsync(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();
            return booking.Id;
        }

        public async Task<IEnumerable<HostsProfitQueryResult>> GetHostsProfit()
        {
            var sql = SqlScripts.GetHostsProfit;
            
            await using var connection = _context.Database.GetDbConnection();

            return await connection.QueryAsync<HostsProfitQueryResult>(sql);
        }

        public async Task<IEnumerable<BookedApartmentQueryResult>> GetTopMostBookedApartment(int n)
        {
            var sql = SqlScripts.GetTopMostBookedApartment;

            await using var connection = _context.Database.GetDbConnection();

            return await connection.QueryAsync<BookedApartmentQueryResult>(sql, new {N = n});
        }

        public async Task<IEnumerable<RepeatedBookingQueryResult>> GetRepeatedBookingPerApartmentClient()
        {
            var sql = SqlScripts.GetRepeatedBookingPerApartmentClient;

            await using var connection = _context.Database.GetDbConnection();

            return await connection.QueryAsync<RepeatedBookingQueryResult>(sql);
        }

        public async Task<IEnumerable<BookingDurationQueryResult>> GetAverageBookingDurationPerApartment()
        {
            var sql = SqlScripts.GetAverageBookingDurationPerApartment;

            await using var connection = _context.Database.GetDbConnection();

            return await connection.QueryAsync<BookingDurationQueryResult>(sql);
        }
    }
}