using BookingService.Application.DTOs;
using BookingService.Domain.DTOs;
using BookingService.Domain.Entities;
using BookingService.Domain.Interfaces;
using BookingService.Infrastructure.Database;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace BookingService.Infrastructure.Repository
{
    public class BookingRepository : IBookingRepository
    {
        private readonly AppDbContext _context;
        private readonly DbConnection _connection;

        public BookingRepository(AppDbContext context)
        {
            _context = context;
            _connection = _context.Database.GetDbConnection();
        }
        public async Task<int> CreateAsync(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
            return booking.Id; //TODO: fix problem - not return new id cause there is not calling SaveChanges before
        }

        public async Task<IEnumerable<HostsProfitQueryResult>> GetHostsProfit()
        {
            var sql = SqlScripts.SqlScripts.GetHostsProfit;
            
            await using var connection = _context.Database.GetDbConnection();

            return await connection.QueryAsync<HostsProfitQueryResult>(sql);
        }

        public async Task<IEnumerable<BookedApartmentQueryResult>> GetTop5MostBookingApartment()
        {
            var sql = SqlScripts.SqlScripts.GetTop5MostBookingApartment;

            await using var connection = _connection;

            return await connection.QueryAsync<BookedApartmentQueryResult>(sql);
        }

        public async Task<IEnumerable<RepeatedBookingQueryResult>> GetRepeatedBookingPerApartmentClient()
        {
            var sql = SqlScripts.SqlScripts.GetRepeatedBookingPerApartmentClient;

            await using var connection = _connection;

            return await connection.QueryAsync<RepeatedBookingQueryResult>(sql);
        }

        public async Task<IEnumerable<BookingDurationQueryResult>> GetAverageBookingDurationPerApartment()
        {
            var sql = SqlScripts.SqlScripts.GetAverageBookingDurationPerApartment;

            await using var connection = _connection;

            return await connection.QueryAsync<BookingDurationQueryResult>(sql);
        }
    }
}