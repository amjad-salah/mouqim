using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Models.DTOs.EducationLevel;
using Models.DTOs.Family;
using Models.DTOs.Occupation;
using Models.DTOs.Person;
using MouqimApi.Data;
using MouqimApi.Endpoints;
using MouqimApi.Services.EducationLevel;
using MouqimApi.Services.Family;
using MouqimApi.Services.Occupation;
using MouqimApi.Services.Person;
using MouqimApi.Utils;
using MouqimApi.Validations.EducationLevel;
using MouqimApi.Validations.Family;
using MouqimApi.Validations.Occupation;
using MouqimApi.Validations.Person;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandler<AppExceptionHandler>();

builder.Services.AddDbContext<MouqimDbContext>(options => options.UseNpgsql(
    builder.Configuration.GetConnectionString("DefaultConnection")));

#region Validations

builder.Services.AddScoped<IValidator<AddFamilyDto>, AddFamilyValidation>();
builder.Services.AddScoped<IValidator<UpdateFamilyDto>, UpdateFamilyValidation>();
builder.Services.AddScoped<IValidator<AddOccupationDto>, AddOccupationValidation>();
builder.Services.AddScoped<IValidator<UpdateOccupationDto>, UpdateOccupationValidation>();
builder.Services.AddScoped<IValidator<AddEducationLevelDto>, AddEducationLeveValidation>();
builder.Services.AddScoped<IValidator<UpdateEducationLevelDto>, UpdateEducationLeveValidation>();
builder.Services.AddScoped<IValidator<AddPersonDto>, AddPersonValidation>();
builder.Services.AddScoped<IValidator<UpdatePersonDto>, UpdatePersonValidation>();

#endregion

#region Services

builder.Services.AddScoped<IFamilyService, FamilyService>();
builder.Services.AddScoped<IOccupationsService, OccupationsService>();
builder.Services.AddScoped<IEducationLevelService, EducationLevelService>();
builder.Services.AddScoped<IPersonService, PersonService>();

#endregion

var app = builder.Build();

app.MapGet("/", () => "Server is running!");

app.MapFamiliesEndpoints();
app.MapOccupationsEndpoints();
app.MapEducationLevelsEndpoints();
app.MapPersonsEndpoints();

app.Run();

//TODO: Add authentication to the project