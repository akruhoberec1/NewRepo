global using Microsoft.EntityFrameworkCore;
using Ninject;
using Service.Data;
using Service.Mapper;
using Service.Ninject;
using Service.Service;
using Service.Service.IService;


var builder = WebApplication.CreateBuilder(args);
var kernel = new StandardKernel();

// Add services to the container.

kernel.Load(new NinjectBindings());
builder.Services.AddSingleton<IKernel>(kernel);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DataContext>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
builder.Services.AddScoped<IVehicleMake, VehicleMakeService>();
builder.Services.AddScoped<IVehicleModel, VehicleModelService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();


//prebaciti kontrolere, default ce biti model 
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=VehicleModel}/{action=Index1}/{id?}");

app.Run();