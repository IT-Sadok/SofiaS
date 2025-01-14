using AutoMapper;
using BookingService.Domain.Constants;
using BookingService.Domain.Entities;
using BookingService.Domain.Interfaces;
using BookingService.Infrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Migration.Abstract;
using Migration.DataModels;
using System.Text.Json;

namespace Migration.Service
{
    internal class DataMigrationService : IDataMigrationService
    {
        private readonly IUserManager _userManager;
        private readonly IRoleManager _roleManager;
        private readonly IApartmentRepository _apartmentRepository;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        private const string DefaultPassword = "DefaultPassword1*";
        public DataMigrationService(IUserManager userManager, IRoleManager roleManager, IApartmentRepository apartmentRepository, IMapper mapper, AppDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
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
                var migrationResult = await MigrateUserData(data);
                if (!migrationResult.IsSuccess)
                {
                    throw new Exception(migrationResult.ErrorMessage);
                }
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return Result.Failure(ex.Message);
            }
        }

        private async Task<Result> MigrateUserData(string data)
        {
            var hostsData = JsonSerializer.Deserialize<List<UserDataModel>>(data);
            if (hostsData == null)
            {
                return Result.Failure("There is no data in file");
            }

            var hosts = _mapper.Map<List<User>>(hostsData);
            var hostRole = await _roleManager.FindByName(Roles.Host);
            var hostRoleId = hostRole.Id;

            foreach (var host in hosts)
            {
                host.UserRoles.Add(new IdentityUserRole<string>()
                {
                    UserId = host.Id,
                    RoleId = hostRoleId
                });

                foreach (var apartment in host.Apartments)
                {
                    apartment.Host = host;
                }

                var creationResult = await _userManager.CreateAsync(host, DefaultPassword);
                if (!creationResult.Succeeded)
                {
                    return Result.Failure("Failure with user migration");
                }
            }


            return Result.Success();
        }
    }
}