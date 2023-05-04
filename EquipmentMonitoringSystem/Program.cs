using EquipmentMonitoringSystem.BuissnesLayer.Implementations;
using EquipmentMonitoringSystem.BuissnesLayer.Interfaces;
using EquipmentMonitoringSystem.BuissnesLayer;
using EquipmentMonitoringSystem.DataLayer;
using EquipmentMonitoringSystem.PresentationLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using EquipmentMonitoringSystem.Areas.Identity.Data;
using EquipmentMonitoringSystem.TimerTask;
using System;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AuthDbContext>(
    o => o.UseNpgsql(builder.Configuration.GetConnectionString("AuthDbContextConnection"), 
    npgsqlOptionsAction: sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 10,
            maxRetryDelay: TimeSpan.FromSeconds(5),
            errorCodesToAdd: null);
    }));


builder.Services.AddDbContext<EFDBContext>(o =>
{
    o.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
    npgsqlOptionsAction: sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
        maxRetryCount: 10,
        maxRetryDelay: TimeSpan.FromSeconds(5),
        errorCodesToAdd: null);
    });
    o.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});
    

builder.Services.AddDefaultIdentity<IdentityUser>().AddDefaultTokenProviders()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AuthDbContext>();




builder.Services.AddScoped<IStationRepository, EFStationRepository>();
builder.Services.AddScoped<IGroupRepository, EFGroupRepository>();
builder.Services.AddScoped<IEquipmentRepository, EFEquipmentRepository>();
builder.Services.AddScoped<IMaintenanceRepository, EFMaintenanceRepository>();
builder.Services.AddScoped<IUpcomingMaintenanceRepository, EFUpcomingMaintenanceRepository>();
builder.Services.AddScoped<INortifyRepository, EFNortifyRepository>();
builder.Services.AddScoped<DataManager>();
builder.Services.AddScoped<ServicesManager>();

builder.Services.AddHostedService<BackgroundTask>();
builder.Services.AddHostedService<NortifyTimer>();

builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

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
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;
    var context = service.GetRequiredService<EFDBContext>();
    SampleData.InitData(context);
}


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
        var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        await UserInitializer.InitializeAsync(userManager, rolesManager);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.MapRazorPages();

app.Run();
