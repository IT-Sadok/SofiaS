using AutoMapper;
using BookingService.Domain.Constants;
using BookingService.Domain.Entities;
using BookingService.Domain.Interfaces;
using BookingService.Infrastructure.Database;
using Migration.Abstract;
using Migration.DataModels;
using System.Text.Json;

namespace Migration.Service
{
    internal class DataMigrationService : IDataMigrationService
    {
        private readonly IUserManager _userManager;
        private readonly IApartmentRepository _apartmentRepository;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        private const string DefaultPassword = "DefaultPassword1*";
        public DataMigrationService(IUserManager userManager, IApartmentRepository apartmentRepository, IMapper mapper, AppDbContext context)
        {
            _userManager = userManager;
            _apartmentRepository = apartmentRepository;
            _mapper = mapper;
            _context = context;
        }

        public async Task<Result> MigrateData(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return Result.Failure("File does not exist.");
            }

            var data = await File.ReadAllTextAsync(filePath);

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var migrationResult = await UserDataMigration(data);
                if (!migrationResult.IsSuccess)
                {
                    throw new Exception(migrationResult.ErrorMessage);
                }
                //await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return Result.Failure(ex.Message);
            }
        }

        private async Task<Result> UserDataMigration(string data)
        {
            var hostsData = JsonSerializer.Deserialize<List<UserDataModel>>(data);
            if (hostsData == null)
            {
                return Result.Failure("There is no data in file");
            }

            foreach (var hostData in hostsData)
            {
                var host = _mapper.Map<User>(hostData);
                var creationResult = await _userManager.CreateAsync(host, DefaultPassword);
                if (!creationResult.Succeeded)
                {
                    return Result.Failure("Failure with user migration");
                }
                var addToRoleResult = await _userManager.AddToRoleAsync(host, Roles.Host);
                if (!addToRoleResult.Succeeded)
                {
                    return Result.Failure("Failure with user adding to role");
                }

                foreach (var apartmentData in hostData.Apartments)
                {
                    var apartment = _mapper.Map<Apartment>(apartmentData);
                    apartment.Host = host;
                    await _apartmentRepository.CreateAsync(apartment);
                }
            }
            return Result.Success();

        }    
    }
}