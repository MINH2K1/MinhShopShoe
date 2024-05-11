using ShopShoe.Infastruction.DataEf;
using Microsoft.EntityFrameworkCore;
using ShopShoe.Application.AutoMapper;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(o => o.AddProfile(new AutoMapperProfile() ));
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ShopShoeDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Conn") 
    ,o=>o.MigrationsAssembly("ShopShoe.Infastruction")));
var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
