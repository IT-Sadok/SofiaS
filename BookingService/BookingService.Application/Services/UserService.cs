using BookingService.Application.Abstract;
using BookingService.Application.DTOs;
using BookingService.Domain.Entities;
using BookingService.Domain.Interfaces;

namespace BookingService.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IUserManager _userManager;

        public UserService(IWalletRepository walletRepository, IUserManager userManager)
        {
            _walletRepository = walletRepository;
            _userManager = userManager;
        }
        public async Task<Result> TopUpBalance(string userId, WalletTopUpDto topUpDto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result.Failure($"User with id {userId} was not found");
            }

            var wallet = await _walletRepository.FindWalletByUserIdAsync(userId);
            if (wallet == null)
            {
                return Result.Failure($"User with id {userId} doesn't have wallet");
            }

            wallet.Balance += topUpDto.Amount;

            await _walletRepository.UpdateAsync(wallet);
            return Result.Success();
        }
    }
}