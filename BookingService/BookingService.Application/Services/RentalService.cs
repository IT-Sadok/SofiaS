using AutoMapper;
using BookingService.Application.Abstract;
using BookingService.Application.DTOs;
using BookingService.Domain.Entities;
using BookingService.Domain.Interfaces;

namespace BookingService.Application.Services
{
    public class RentalService : IRentalService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RentalService(IUnitOfWork unitOfWork,IMapper mapper)
        { 
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<int>> CreateBooking(string clientId, BookingCreateDto bookingCreateDto)
        {
            var apartmentId = bookingCreateDto.ApartmentId;
            var startDate = bookingCreateDto.StartDate;
            var endDate = bookingCreateDto.EndDate;

            var apartment = await _unitOfWork.ApartmentRepository.FindByIdAsync(apartmentId);
            if (apartment == null)
            {
                return Result<int>.Failure($"Apartment is not found.");
            }
            if (!_unitOfWork.ApartmentRepository.IsAvailable(apartmentId, startDate, endDate))
            {
                return Result<int>.Failure("Apartment is not available for this date.");
            }
            var hostId = apartment.HostId;
            var rentalDuration = Math.Abs((endDate - startDate).Days) + 1;
            var totalPrice = apartment.Price * rentalDuration;

            var users = await _unitOfWork.UserManager.FindByIdsAsync(clientId, hostId);
            var client = users.FirstOrDefault(u => u.Id == clientId);
            var host = users.FirstOrDefault(u => u.Id == hostId);

            if (client == null || host == null)
            {
                return Result<int>.Failure("Host or client is not found.");
            }
            var wallets = await _unitOfWork.WalletRepository.FindByUserIdsAsync(clientId, hostId);
            var clientWallet = wallets.FirstOrDefault(w => w.UserId == clientId);
            var hostWallet = wallets.FirstOrDefault(w => w.UserId == hostId);

            if (clientWallet == null || hostWallet == null)
            {
                return Result<int>.Failure("Client or host wallet is not found.");
            }

            if (clientWallet.Balance < totalPrice)
            {
                return Result<int>.Failure("Not enough money on the balance");
            }

            await _unitOfWork.BeginTransactionAsync();
            
            try
            {
                hostWallet.Balance += totalPrice;
                clientWallet.Balance -= totalPrice;

                await _unitOfWork.WalletRepository.UpdateAsync(hostWallet);
                await _unitOfWork.WalletRepository.UpdateAsync(clientWallet);

                var booking = _mapper.Map<Booking>(bookingCreateDto);
                booking.TotalPrice = totalPrice;
                booking.ClientId = clientId;

                var bookingId = await _unitOfWork.BookingRepository.CreateAsync(booking);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
                return Result<int>.Success(bookingId);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return Result<int>.Failure(ex.ToString());

            }
        }
    }
}