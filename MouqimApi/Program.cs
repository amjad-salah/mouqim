using Microsoft.EntityFrameworkCore;
using MouqimApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MouqimDbContext>(options => options.UseNpgsql(
    builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();