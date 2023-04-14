using DapperAccess.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using WebApi.Models.Mappings;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? builder.Configuration.GetConnectionString("SqlServer")
    ?? throw new Exception("Connection string is not provided");

builder.Services.AddDapperDataContext(connectionString);
builder.Services.AddAutoMapper(typeof(CarMappingProfile));

builder.Services.Configure<JwtValidationOptions>(builder.Configuration.GetRequiredSection(nameof(JwtValidationOptions)));

builder.Services.AddAuthentication()
    .AddJwtBearer(options =>
    {
        var jwtOptions = builder.Configuration.GetRequiredSection(nameof(JwtValidationOptions)).Get<JwtValidationOptions>()
            ?? throw new Exception("Jwt options are not provided");

        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = jwtOptions.GetSecurityKey()
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddScoped<JwtService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "Car Manage API", Version = "v1" });
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new()
    {
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Name = "Authorization",
        Description = "A valid jwt token should be provided",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        BearerFormat = "JWT"
    });
    options.AddSecurityRequirement(new()
    {
        {
            new()
            {
                Reference = new()
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            },
            new string[] { }
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
