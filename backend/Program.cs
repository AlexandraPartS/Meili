using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Services;
using backend.Interfaces;
using backend.Models;

var builder = WebApplication.CreateBuilder(
      new WebApplicationOptions { WebRootPath = "..\\frontend" });

builder.Services.AddControllers();
//builder.Services.AddDbContext<UserContext>(opt => opt.UseInMemoryDatabase("UsersList"));
builder.Services.AddTransient<DataStorage<UserDao>>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddDbContext<UserContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("UserContext")));//

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
app.MapControllers();

app.Run();
