/**********************************************
 * 
 *      Portfolio Work For Matlock White
 *          User Service Example
 *           Program: Program.cs
 *              
 * ********************************************/

using Microsoft.EntityFrameworkCore;
using UserService.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// Database connections
// NOTE:    We have 3 DB Connections each representing the different Database Users Authorization for Data Access and Manipulation based
//          on the Database Users Access Level in the DB.
var readConnectionString = builder.Configuration.GetConnectionString("UserServiceDatabase_ReadAccess") 
    ?? throw new InvalidOperationException("UserService database read connection string configuration is not properly set.");

var writeConnectionString = builder.Configuration.GetConnectionString("UserServiceDatabase_WriteAccess")
    ?? throw new InvalidOperationException("UserService database write connection string configuration is not properly set.");

var readWriteConnectionString = builder.Configuration.GetConnectionString("UserServiceDatabase_ReadWriteAccess")
    ?? throw new InvalidOperationException("UserService database read/write connection string configuration is not properly set.");

// ---- Add DB Context ----
// Read-Only Context
builder.Services.AddDbContext<UserServiceReadContext>(options =>
    options.UseMySql(
            readConnectionString,
            ServerVersion.AutoDetect(readConnectionString)
    ));

// Write-Only Context
builder.Services.AddDbContext<UserServiceWriteContext>(options =>
    options.UseMySql(
            writeConnectionString,
            ServerVersion.AutoDetect(writeConnectionString)
    ));

// Read & Write Context
builder.Services.AddDbContext<UserServiceReadWriteContext>(options =>
    options.UseMySql(
            readWriteConnectionString,
            ServerVersion.AutoDetect(readWriteConnectionString)
    ));


var jwtKey = builder.Configuration["JWT:Key"] ?? throw new InvalidOperationException("JWT Key was not properly configured.");
var jwtIssuer = builder.Configuration["JWT:Issuer"] ?? throw new InvalidOperationException("JWT Issuer was not properly configured");
var jwtAudience = builder.Configuration["JWT:Audience"] ?? throw new InvalidOperationException("JWT Audience was not properly configured");

// Authentication just using JWT but would normally also use OAuth.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,

            ValidateAudience = true,
            ValidAudience = jwtAudience,

            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter the JWT Token."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { 
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            }, Array.Empty<string>()
        }
    });
});

// Register ASP.NET Core health check services.
builder.Services.AddHealthChecks();

var app = builder.Build();

// Expose application health status for Docker Compose.
app.MapHealthChecks("/healthcheck");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
