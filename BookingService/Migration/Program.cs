using BookingService.Domain.Entities;
using BookingService.Domain.Interfaces;
using BookingService.Infrastructure.Database;
using BookingService.Infrastructure.IdentityServices;
using BookingService.Infrastructure.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Migration.Abstract;
using Migration.Mapping;
using Migration.Service;

internal class Program
{
    private async static Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        try
        {
            var migrationService = host.Services.GetRequiredService<IDataMigrationService>();
            string filePath = "Files/data.json";
            var result = await migrationService.MigrateData(filePath);

            if (result.IsSuccess)
            {
                Console.WriteLine("Data migration successful.");
            }
            else
            {
                Console.WriteLine($"Data migration failed: {result.ErrorMessage}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    


        static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddDbContext<AppDbContext>(options =>
                        options.UseSqlServer("Data Source=DESKTOP-VM12JTU;Initial Catalog=BookingService;Integrated Security=True;TrustServerCertificate=True;Encrypt=False"));
                    services.AddDataProtection();
                    services.AddIdentityCore<User>()
                        .AddRoles<IdentityRole>()
                        .AddEntityFrameworkStores<AppDbContext>()
                        .AddDefaultTokenProviders();

                    services.AddScoped<IUserManager, UserManagerWrapper>();
                    services.AddScoped<IApartmentRepository, ApartmentRepository>();
                    services.AddAutoMapper(typeof(MapperProfile));
                    services.AddScoped<IDataMigrationService, DataMigrationService>();
                });
        }
    }
}