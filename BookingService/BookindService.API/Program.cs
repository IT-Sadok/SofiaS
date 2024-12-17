using BookingService.API.Middleware;
using BookingService.Application;
using BookingService.Application.Abstract;
using BookingService.Application.MappingProfile;
using BookingService.Application.Validation;
using BookingService.Domain;
using BookingService.Infrastructure.Authentication;
using BookingService.Infrastructure.Configuration;
using BookingService.Infrastructure.Database;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(typeof(RegisterDtoValidator).Assembly));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>() 
            .AddDefaultTokenProviders();

builder.Services.AddScoped<ITokenService, JwtTokenService>();
builder.Services.AddScoped<ICustomUserManager, CustomUserManager>();
builder.Services.AddScoped<IUserService, UserService>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseValidation();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
