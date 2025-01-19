using BookingService.Domain.Interfaces;
using BookingService.Domain.Entities;
using BookingService.Infrastructure.Database;
using BookingService.Domain.DTOs;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Dapper;

namespace BookingService.Infrastructure.Repository
{
    public class ApartmentRepository : IApartmentRepository
    {
        private readonly AppDbContext _context;
        private readonly DbConnection _connection;

        public ApartmentRepository(AppDbContext context)
        {
            _context = context;
            _connection = _context.Database.GetDbConnection();
        }

        public async Task<int> CreateAsync(Apartment apartment)
        {
            await _context.Apartments.AddAsync(apartment);
            await _context.SaveChangesAsync();
            return apartment.Id;
        }

        public async Task UpsertCustomData(int apartmentId, string hostId, string customData)
        {
            var sql = SqlScripts.SqlScripts.UpsertApartmentCustomData;

            await using var connection = _connection;
            await connection.ExecuteAsync(sql, new { Id = apartmentId, HostId = hostId, CustomData = customData });
        }

        public async Task<Apartment?> FindByIdAsync(int apartmentId)
        {
            return await _context.Apartments.FindAsync(apartmentId);
        }

        public async Task UpdateAsync(Apartment apartment)
        {
            if (apartment != null)
            {
                _context.Apartments.Update(apartment);
                await _context.SaveChangesAsync();
            }
        }

        public bool IsAvailable(int apartmentId, DateTime startDate, DateTime endDate)
        {
            //find intersection
            var bookings = _context.Bookings
                                .Where(a => a.ApartmentId == apartmentId
                                    && a.EndDate > startDate
                                    && a.StartDate < endDate)
                                .ToList(); 
            return !bookings.Any();
        }

        public async Task<IEnumerable<ApartmentPriceQuantilesQueryResult>> GetPriceQuantiles()
        {
            var sql = SqlScripts.SqlScripts.GetPriceQuantiles;

            await using var connection = _connection;

            return await _connection.QueryAsync<ApartmentPriceQuantilesQueryResult>(sql);
        }
    }
}