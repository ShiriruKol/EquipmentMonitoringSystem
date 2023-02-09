using EquipmentMonitoringSystem.BuissnesLayer.Implementations;
using EquipmentMonitoringSystem.BuissnesLayer.Interfaces;
using EquipmentMonitoringSystem.BuissnesLayer;
using EquipmentMonitoringSystem.DataLayer;
using EquipmentMonitoringSystem.PresentationLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using EquipmentMonitoringSystem.Areas.Identity.Data;

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
    })); ;

builder.Services.AddDbContext<EFDBContext>(
    o => o.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
    npgsqlOptionsAction: sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 10,
            maxRetryDelay: TimeSpan.FromSeconds(5),
            errorCodesToAdd: null);
    }));

builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<AuthDbContext>();

builder.Services.AddTransient<IStationRepository, EFStationRepository>();
builder.Services.AddTransient<IGroupRepository, EFGroupRepository>();
builder.Services.AddTransient<IEquipmentRepository, EFEquipmentRepository>();
builder.Services.AddScoped<DataManager>();
builder.Services.AddScoped<ServicesManager>();


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


app.MapRazorPages();

app.Run();
