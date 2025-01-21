using BookingService.Domain.DTOs;
using BookingService.Domain.Entities;
using BookingService.Domain.Interfaces;
using BookingService.Infrastructure.Database;
using BookingService.Infrastructure.Resources;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Infrastructure.Repository
{
    public class ApartmentRepository : IApartmentRepository
    {
        private readonly AppDbContext _context;

        public ApartmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAsync(Apartment apartment)
        {
            await _context.Apartments.AddAsync(apartment);
            await _context.SaveChangesAsync();
            return apartment.Id;
        }

        public async Task UpsertCustomData(int apartmentId, string hostId, string customData)
        {
            var sql = SqlScripts.UpsertApartmentCustomData;

            await using var connection = _context.Database.GetDbConnection();
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
            var sql = SqlScripts.GetPriceQuantiles;

            await using var connection = _context.Database.GetDbConnection();

            return await connection.QueryAsync<ApartmentPriceQuantilesQueryResult>(sql);
        }
    }
}