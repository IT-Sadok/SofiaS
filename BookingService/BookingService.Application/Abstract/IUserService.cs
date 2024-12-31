using BookingService.Application.DTOs;
using BookingService.Domain.Entities;

namespace BookingService.Application.Abstract
{
    public interface IUserService
    {
        Task<Result> TopUpBalance(string userId, WalletTopUpDto topUpDto);
    }
}
