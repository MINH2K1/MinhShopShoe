using ShopShoe.Infastruction.DataEf;
using Microsoft.EntityFrameworkCore;
using ShopShoe.Application.AutoMapper;
using ShopShoe.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using ShopShoe.Application.Implement;
using ShopShoe.Application.Interface;
using ShopShoe.Infastruction.Repository.Interface;
using static ShopShoe.Infastruction.Repository.Interface.IRepository;
using ShopShoe.Infastruction.Repository.implement;
using ShopShoe.Infastruction;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();


builder.Services.AddAutoMapper(o => o.AddProfile(new AutoMapperProfile() ));
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ShopShoeDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") 
    ,o=>o.MigrationsAssembly("ShopShoe.Infastruction")));



builder.Services.AddScoped<UserManager<AppUser>, UserManager<AppUser>>();
builder.Services.AddScoped<RoleManager<AppRole>, RoleManager<AppRole>>();
builder.Services.AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork));
builder.Services.AddTransient(typeof(IRepository<,>), typeof(Repository<,>));
//Services

builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IUserService, UserService>();

builder.Services.AddScoped<DbInitializer>();
builder.Services.AddIdentity<AppUser, AppRole>(options =>
{
    // Configure Identity options if needed
})
  .AddEntityFrameworkStores<ShopShoeDbContext>() // Add EF Core store
  .AddDefaultTokenProviders();

//builder.Services.AddTransient<IRoleService, RoleService>();
//builder.Services.AddTransient<IBillService, BillService>();

//builder.Services.AddTransient<ICommonService, CommonService>();
//builder.Services.AddTransient<IContactService, ContactService>();
//builder.Services.AddTransient<IPageService, PageService>();
//builder.Services.AddTransient<IReportService, ReportService>();
//builder.Services.AddTransient<IAnnouncementService, AnnouncementService>();
//builder.Services.AddTransient<IProductCategoryService, ProductCategoryService>();
//builder.Services.AddTransient<IFunctionService, FunctionService>();


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

//app.UseAuthorization();


app.MapControllerRoute(
name: "Admin",
pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
name: "default",
pattern: "{controller=Home}/{action=Index}/{id?}");




using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {

        var dbInitializer = services.GetService<DbInitializer>();
        dbInitializer.Seed().Wait();


    }
    catch (Exception ex)
    {

        var logger = services.GetService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database");
    }
}

app.Run();
