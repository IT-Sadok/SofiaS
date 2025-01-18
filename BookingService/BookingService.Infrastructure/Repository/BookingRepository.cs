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
            await using var connection = _context.Database.GetDbConnection();

            var sql = @"SELECT u.UserName, SUM(b.TotalPrice) as TotalIncome 
                        FROM dbo.AspNetUsers as u 
                        INNER JOIN dbo.Apartments as a on a.HostId = u.Id 
                        INNER JOIN dbo.Bookings as b on a.Id = b.ApartmentId 
                        GROUP BY u.UserName;";
            return await connection.QueryAsync<HostsProfitQueryResult>(sql);
        }

        public async Task<IEnumerable<BookedApartmentQueryResult>> GetTop5MostBookingApartment()
        {
            await using var connection = _connection;

            var sql = @"SELECT TOP 5 ApartmentId, COUNT(*) AS BookingCount
                        FROM dbo.Bookings
                        GROUP BY ApartmentId 
                        ORDER BY BookingCount DESC;";
            return await connection.QueryAsync<BookedApartmentQueryResult>(sql);
        }

        public async Task<IEnumerable<RepeatedBookingQueryResult>> GetRepeatedBookingPerApartmentClient()
        {
            await using var connection = _connection;

            var sql = @"SELECT b.ApartmentId
                    FROM dbo.Bookings as b
                    GROUP BY b.ApartmentId, b.ClientId
                    HAVING COUNT(*) > 1;";
            return await connection.QueryAsync<RepeatedBookingQueryResult>(sql);
        }

        public async Task<IEnumerable<BookingDurationQueryResult>> GetAverageBookingDurationPerApartment()
        {
            await using var connection = _connection;

            var sql = @"SELECT ApartmentId, AVG(DATEDIFF(DAY, StartDate, EndDate)) as AverageDuration
                        FROM dbo.Bookings
                        GROUP BY ApartmentId;";
            return await connection.QueryAsync<BookingDurationQueryResult>(sql);
        }
    }
}