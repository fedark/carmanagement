using DapperAccess.Infrastructure;
using Web.Models.Mappings;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? builder.Configuration.GetConnectionString("SqlServer")
    ?? throw new Exception("Connection string is not provided");

builder.Services.AddDapperDataContext(connectionString);
builder.Services.AddAutoMapper(typeof(CarMappingProfile));

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
