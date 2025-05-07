using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTOs;
using Models.DTOs.EducationLevel;
using Models.DTOs.Family;
using Models.DTOs.Occupation;
using Models.DTOs.Person;
using Models.DTOs.User;
using Models.Entities;
using MouqimApi.Data;
using MouqimApi.Endpoints;
using MouqimApi.Services.EducationLevel;
using MouqimApi.Services.Family;
using MouqimApi.Services.Occupation;
using MouqimApi.Services.Person;
using MouqimApi.Services.User;
using MouqimApi.Utils;
using MouqimApi.Validations.EducationLevel;
using MouqimApi.Validations.Family;
using MouqimApi.Validations.Occupation;
using MouqimApi.Validations.Person;
using MouqimApi.Validations.User;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandler<AppExceptionHandler>();

builder.Services.AddDbContext<MouqimDbContext>(options => options.UseNpgsql(
    builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication().AddCookie("default", options =>
{
    //Configure a default redirect to log in and access denied behavior
    options.Events = new CookieAuthenticationEvents
    {
        OnRedirectToLogin = (ctx) =>
        {
            ctx.Response.StatusCode = 401;
            return Task.CompletedTask;
        },

        OnRedirectToAccessDenied = (ctx) =>
        {
            ctx.Response.StatusCode = 403;
            return Task.CompletedTask;
        }
    };
});
builder.Services.AddAuthorization();
builder.Services.AddCors();

#region Validations

builder.Services.AddScoped<IValidator<AddFamilyDto>, AddFamilyValidation>();
builder.Services.AddScoped<IValidator<UpdateFamilyDto>, UpdateFamilyValidation>();
builder.Services.AddScoped<IValidator<AddOccupationDto>, AddOccupationValidation>();
builder.Services.AddScoped<IValidator<UpdateOccupationDto>, UpdateOccupationValidation>();
builder.Services.AddScoped<IValidator<AddEducationLevelDto>, AddEducationLeveValidation>();
builder.Services.AddScoped<IValidator<UpdateEducationLevelDto>, UpdateEducationLeveValidation>();
builder.Services.AddScoped<IValidator<AddPersonDto>, AddPersonValidation>();
builder.Services.AddScoped<IValidator<UpdatePersonDto>, UpdatePersonValidation>();
builder.Services.AddScoped<IValidator<AddUserDto>, AddUserValidation>();
builder.Services.AddScoped<IValidator<LoginDto>, LoginValidation>();
builder.Services.AddScoped<IValidator<UpdateUserDto>, UpdateUserValidation>();
builder.Services.AddScoped<IValidator<ResetPasswordDto>, ResetPasswordValidation>();

#endregion

#region Services

builder.Services.AddScoped<IFamilyService, FamilyService>();
builder.Services.AddScoped<IOccupationsService, OccupationsService>();
builder.Services.AddScoped<IEducationLevelService, EducationLevelService>();
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IUserService, UserService>();

#endregion

var app = builder.Build();

app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseAuthentication();
app.UseAuthorization();
// app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.MapGet("/", () => "Server is running!");

app.MapFamiliesEndpoints();
app.MapOccupationsEndpoints();
app.MapEducationLevelsEndpoints();
app.MapPersonsEndpoints();
app.MapUsersEndpoints();

#region DefaultAdmin

using (var serviceScope = app.Services.CreateScope())
{
    var context = serviceScope.ServiceProvider.GetRequiredService<MouqimDbContext>();

    var adminExists = context.Users.Any(u => u.Role == UserRole.Admin);

    if (!adminExists)
    {
        await context.Users.AddAsync(new User
        {
            Username = "admin",
            FullName = "Admin User",
            Role = UserRole.Admin,
            Password = BCrypt.Net.BCrypt.HashPassword("admin123")
        });

        await context.SaveChangesAsync();
    }
}

#endregion

app.Run();

//TODO: Add authentication to the project