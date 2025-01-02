using AutoMapper;
using BookingService.Application.Abstract;
using BookingService.Application.DTOs;
using BookingService.Domain.Entities;
using BookingService.Domain.Interfaces;

namespace BookingService.Application.Services
{
    public class RentalService : IRentalService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IApartmentRepository _apartmentRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly IUserManager _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RentalService(
            IBookingRepository bookingRepository,
            IApartmentRepository apartmentRepository,
            IWalletRepository walletRepository,
            IUserManager userManager,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _apartmentRepository = apartmentRepository;
            _walletRepository = walletRepository;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result> CreateBooking(string tenantId, BookingCreateDto bookingCreateDto)
        {
            var apartment = await _apartmentRepository.FindApartmentByIdAsync(bookingCreateDto.ApartmentId);
            if (apartment == null)
            {
                return Result.Failure($"Apartment is not found.");
            }
            if (!apartment.IsAvailable)
            {
                return Result.Failure("Apartment is not available.");
            }
            var hostId = apartment.HostId;
            var rentalDuration = Math.Abs((bookingCreateDto.EndDate - bookingCreateDto.StartDate).Days) + 1; //include endDate
            var totalPrice = apartment.Price * rentalDuration;

            var tenant = await _userManager.FindByIdAsync(tenantId);
            var host = await _userManager.FindByIdAsync(hostId);
            if (tenant == null || host == null)
            {
                return Result.Failure("Host or tenant is not found.");
            }
            if (tenant?.Wallet?.Balance < totalPrice)
            {
                return Result.Failure("Not enough money on the balance");
            }
            var tenantWallet = await _walletRepository.FindWalletByUserIdAsync(tenantId);
            var hostWallet = await _walletRepository.FindWalletByUserIdAsync(hostId);

            using (_unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    hostWallet.Balance += totalPrice;
                    tenantWallet.Balance -= totalPrice;

                    await _walletRepository.UpdateAsync(hostWallet);
                    await _walletRepository.UpdateAsync(tenantWallet);

                    var booking = _mapper.Map<Booking>(bookingCreateDto);
                    booking.TotalPrice = totalPrice;
                    booking.HostId = hostId;
                    booking.TenantId = tenantId;
                    apartment.IsAvailable = false;

                    await _apartmentRepository.UpdateAsync(apartment);
                    await _bookingRepository.CreateBookingAsync(booking);
                    await _unitOfWork.CommitAsync();
                    return Result.Success();
                }
                catch (Exception ex)
                {
                    await _unitOfWork.RollbackAsync();
                    return Result.Failure(ex.ToString());

                }
            }
        }
    }
}