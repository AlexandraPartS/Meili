using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Services;
using backend.Interfaces;
using backend.Models;
using backend.Infrastructure;
using AutoMapper;
using backend.Middleware;


var builder = WebApplication.CreateBuilder(
      new WebApplicationOptions { WebRootPath = GlobalVariables.WebRootPath});

Mapper mapperUser = new Mapper(new MapperConfiguration(cfg =>
{
    cfg.CreateMap<UserDao, UserDto>()
    .ForMember(d => d.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber.PhoneNumber))
    .ReverseMap();
}));
builder.Services.AddSingleton<Mapper>(mapperUser);

builder.Services.AddControllers(opts =>
{
    opts.ModelBinderProviders.Insert(0, new CollectionUserDtoFileModelBinderProvider());
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<DataStorage<UserDao>>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddSingleton<IFileService, FileService>();
builder.Services.AddDbContext<UserContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("UserContext")));

var app = builder.Build();

app.UseCustomExceptionHandler();

app.UseDefaultFiles();
app.UseStaticFiles();
app.MapControllers();

app.Run();



