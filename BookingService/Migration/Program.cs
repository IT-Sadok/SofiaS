using BookingService.Domain.Entities;
using BookingService.Domain.Interfaces;
using BookingService.Infrastructure.Database;
using BookingService.Infrastructure.IdentityServices;
using BookingService.Infrastructure.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Migration.Abstract;
using Migration.Mapping;
using Migration.Service;
using Serilog;

internal class Program
{
    private async static Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()  
            .CreateLogger();

        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            try
            {
                var dataMigrationService = services.GetRequiredService<IDataMigrationService>();
                var configuration = services.GetRequiredService<IConfiguration>();
                string filePath = configuration["FilePaths:UserDataFilePath"];

                var result = await dataMigrationService.MigrateData(filePath);

                if (result.IsSuccess)
                {
                    Log.Information("Data migration succeeded.");
                }
                else
                {
                    Log.Error($"Data migration failed: {result.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An error occurred: {ex.Message}");
            }

            Log.CloseAndFlush();
        }
    
        static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    var path = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
                    config.AddJsonFile(path, optional: false, reloadOnChange: true);
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddDbContext<AppDbContext>(options =>
                        options.UseSqlServer(context.Configuration.GetConnectionString("DefaultConnection")));
                    services.AddDataProtection();
                    services.AddIdentityCore<User>()
                        .AddRoles<IdentityRole>()
                        .AddEntityFrameworkStores<AppDbContext>()
                        .AddDefaultTokenProviders();

                    services.AddScoped<IUserManager, UserManagerWrapper>();
                    services.AddScoped<IRoleManager, RoleManagerWrapper>();
                    services.AddScoped<IApartmentRepository, ApartmentRepository>();
                    services.AddAutoMapper(typeof(MapperProfile));
                    services.AddScoped<IDataMigrationService, DataMigrationService>();
                });
        }
    }
}