using DapperAccess.Infrastructure;
using WebApi.Models.Mappings;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? builder.Configuration.GetConnectionString("SqlServer")
    ?? throw new Exception("Connection string is not provided");

builder.Services.AddDapperDataContext(connectionString);
builder.Services.AddAutoMapper(typeof(CarMappingProfile));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
