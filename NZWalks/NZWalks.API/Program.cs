using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Mappings;
using NZWalks.API.Repositories;
using System.Text;
using NZWalks.API.DBContextData;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.FileProviders;
using Serilog;
using NZWalks.API.Middlewares;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


//1111111111111111111111111111111111111111111 SeriLog Config 111111111111111111111111111111111111111111111111111
// Configure Serilog Logger in the Services
var logger = new LoggerConfiguration()
    .WriteTo.Console()
    //.WriteTo.File("Logs/NzWalk_Log.txt", rollingInterval: RollingInterval.Minute)   //<- Commenting file log generator
    .MinimumLevel.Information()
    .MinimumLevel.Warning()
    .MinimumLevel.Debug()
    .CreateLogger();

builder.Logging.ClearProviders(); // Clears the default logging providers (like Console, Debug, etc.) so that only Serilog is used.

builder.Logging.AddSerilog(logger); //Registers the Serilog logger as the main logging provider for the app.


builder.Services.AddHttpContextAccessor();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


//1111111111111111111111111111111111111111111 5.Authentication Scheme 111111111111111111111111111111111111111111111111111
// Configuartion to display the Authentication on the Swagger Page Sp that user can authenticate on swagger itself
builder.Services.AddSwaggerGen(opt=>
{
    opt.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "NZ Walks API",
        Version = "v1",
    });

    opt.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme()
            {
                Reference = new OpenApiReference()
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                },
                Scheme = "Oauth2",
                Name = JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});


// Registering FluentValidator
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);


// Registration of NZWalksDbContext
builder.Services.AddDbContext<NZWalksDbContext>(options => 
options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalksConnectionString")));


//1111111111111111111111111111111111111111111 3.Authentication Scheme 111111111111111111111111111111111111111111111111111
// Registration of NZWalksAuthDbContext
builder.Services.AddDbContext<NZWalksAuthDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalkAuthConnectionString")));


//1111111111111111111111111111111111111111111 Register Services 111111111111111111111111111111111111111111111111111

builder.Services.AddScoped<IRegionRepository, SqlRegionRepository>();
//builder.Services.AddScoped<IRegionRepository, MemoryRegionRepository>();
builder.Services.AddScoped<IWalkRepository, SqlWalkRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<IImageRepository, LocalImageRepository>();


//1111111111111111111111111111111111111111111 AutoMapper Configuration 111111111111111111111111111111111111111111111111111
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));


//1111111111111111111111111111111111111111111 4.Authentication Scheme 111111111111111111111111111111111111111111111111111
// Register the Identity User
builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("NZWalks")
    .AddEntityFrameworkStores<NZWalksAuthDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});


//1111111111111111111111111111111111111111111 1. Authentication Scheme 111111111111111111111111111111111111111111111111111
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt=> 
    opt.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    });

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


//1111111111111111111111111111111111111111111 Config Exception Handler Middelware 111111111111111111111111111111111111111111111111111
app.UseMiddleware<ExceptionHandlerMiddelware>();


app.UseHttpsRedirection();


//1111111111111111111111111111111111111111111 2.Authentication Scheme 111111111111111111111111111111111111111111111111111
// Authenticate User before Authorize
app.UseAuthentication(); 
app.UseAuthorization();


//1111111111111111111111111111111111111111111 Static Image Config 111111111111111111111111111111111111111111111111111
// This is help to show the static images outside on the Web
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Images")),
    RequestPath = "/Images"
});


app.MapControllers();

app.Run();