using Comnet.Business.Contracts;
using Comnet.Business.Manager.Managers;
using Comnet.Data.Context;
using Comnet.Data.Contracts.RepostoryInterfaces;
using Comnet.DataRepository;
using Microsoft.EntityFrameworkCore;
using Comnet.Common.Model;
using Comnet.Common.Helpers;

var builder = WebApplication.CreateBuilder(args);

using IHost host = Host.CreateDefaultBuilder(args).Build();

// Ask the service provider for the configuration abstraction.
IConfiguration Configuration = new ConfigurationBuilder()
       .SetBasePath(Directory.GetCurrentDirectory())
       .AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true)
       .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"}.json", optional: true)
       .AddCommandLine(args)
       .Build();

#region AzureKeyVault
KeyVaultModal keys = new()
{
    SQLConnection = Configuration.GetConnectionString("DefaultConnection") ?? ""
};
#endregion

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowOrigin", options => options
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
    );
});

builder.Services.AddDbContext<ComnetDbContext>(options =>
    options.UseSqlServer(@keys.SQLConnection, sqlServerOptions => sqlServerOptions.CommandTimeout(12000)));

#region Add Managers
builder.Services.AddScoped<ICarManager, CarManager>();
#endregion

#region Add Repositories
builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<ICarImageRepository, CarImageRepository>();
#endregion

#region Authentication
//builder.Services.AddAuthentication(item =>
//{
//    item.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    item.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//.AddJwtBearer(item =>
//{

//    item.RequireHttpsMetadata = true;
//    item.SaveToken = true;
//    item.TokenValidationParameters = new TokenValidationParameters()
//    {
//        ValidateIssuerSigningKey = true,
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Key"] ?? "")),
//        ValidateIssuer = false,
//        ValidateAudience = false,
//        ValidateLifetime = true,
//        ClockSkew = TimeSpan.Zero
//    };
//});
#endregion
var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
if (!Directory.Exists(wwwrootPath))
{
    Directory.CreateDirectory(wwwrootPath);
}

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ContextHelper>(); // Register ContextHelper

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

//app.UseAuthentication();
//app.UseAuthorization();

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseCors("AllowOrigin");

app.MapControllers();

app.Run();
