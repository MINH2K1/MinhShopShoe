using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShopShoe.Application.AutoMapper;
using ShopShoe.Application.Implement;
using ShopShoe.Application.Interface;
using ShopShoe.Domain.Entities;
using ShopShoe.Infastruction;
using ShopShoe.Infastruction.DataEf;
using ShopShoe.Infastruction.Repository.implement;
using ShopShoe.Infastruction.Repository.Interface;
using System.Text;
using static ShopShoe.Infastruction.Repository.Interface.IRepository;
using Serilog;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Error()
    .WriteTo.Console()
    .CreateLogger();
    Log.Information("Starting web application");

    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddSerilog();

    // authentication with identity
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new
        Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Authentication:Jwt:ValidAudience"],
            ValidIssuer = builder.Configuration["Authentication:Jwt:ValidIssuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Authentication:Jwt:Secret"]))
        };
    }).AddCookie()
   .AddGoogle(googleOptions =>
   {
       googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
       googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
   });

// Add services to the container.
builder.Services.AddAutoMapper(o => o.AddProfile(new AutoMapperProfile()));
    // Add services to the container.
    builder.Services.AddControllersWithViews();
    builder.Services.AddDbContext<ShopShoeDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
        , o => o.MigrationsAssembly("ShopShoe.Infastruction")));

    builder.Services.AddSignalR();
    builder.Services.AddScoped<UserManager<AppUser>, UserManager<AppUser>>();
    builder.Services.AddScoped<RoleManager<AppRole>, RoleManager<AppRole>>();
    builder.Services.AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork));
    builder.Services.AddTransient(typeof(IRepository<,>), typeof(Repository<,>));
    //Services
    builder.Services.AddTransient<ITokenService, TokenService>();
    builder.Services.AddTransient<IProductService, ProductService>();
    builder.Services.AddTransient<IUserService, UserService>();
    builder.Services.AddTransient<IRoleService, RoleService>();
    builder.Services.AddTransient<IProductCategoryService, ProductCategoryService>();

    builder.Services.AddScoped<DbInitializer>();
    builder.Services.AddIdentity<AppUser, AppRole>(options =>
    {
        // Configure Identity options if needed
    })
      .AddEntityFrameworkStores<ShopShoeDbContext>() // Add EF Core store
      .AddDefaultTokenProviders();
    //builder.Services.AddTransient<IBillService, BillService>();
    //builder.Services.AddTransient<ICommonService, CommonService>();
    //builder.Services.AddTransient<IContactService, ContactService>();
    //builder.Services.AddTransient<IPageService, PageService>();
    //builder.Services.AddTransient<IReportService, ReportService>();
    //builder.Services.AddTransient<IAnnouncementService, AnnouncementService>();
    //builder.Services.AddTransient<IFunctionService, FunctionService>();
    //builder.Services.AddHttpLogging(logging =>
    //{
    //    logging.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All;
    //});


    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();
    app.UseSerilogRequestLogging();
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseStaticFiles();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();


