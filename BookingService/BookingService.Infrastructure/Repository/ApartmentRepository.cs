using BookingService.Domain.Interfaces;
using BookingService.Domain.Entities;
using BookingService.Infrastructure.Database;

namespace BookingService.Infrastructure.Repository
{
    public class ApartmentRepository : IApartmentRepository
    {
        private readonly AppDbContext _context;

        public ApartmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddApartmentAsync(Apartment apartment)
        {
            await _context.Apartments.AddAsync(apartment);
            await _context.SaveChangesAsync();
            return apartment.Id;
        }

        public async Task<Apartment?> FindApartmentByIdAsync(int apartmentId)
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
    }
}