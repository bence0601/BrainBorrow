using BrainBorrowAPI.Data;
using BrainBorrowAPI.Services;
using BrainBorrowAPI.Services.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<INoteService, NotesService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();

// Add DbContexts
builder.Services.AddDbContext<NoteContext>(options =>
{
    ConfigureDbContext(options, builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddDbContext<UserContext>(options =>
{
    ConfigureDbContext(options, builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Configure Identity
builder.Services.AddIdentityCore<IdentityUser>(options =>
{
    ConfigureIdentityOptions(options);
})
.AddEntityFrameworkStores<UserContext>();

// Configure authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        ConfigureJwtBearerOptions(options);
    });

// Configure Swagger
builder.Services.AddSwaggerGen(options =>
{
    ConfigureSwagger(options);
});

var app = builder.Build();

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

// Helper methods
void ConfigureDbContext(DbContextOptionsBuilder options, string connectionString)
{
    options.UseNpgsql(connectionString, npgsqlOptions =>
    {
        npgsqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
    });
}

void ConfigureIdentityOptions(IdentityOptions options)
{
    options.SignIn.RequireConfirmedAccount = false;
    options.User.RequireUniqueEmail = true;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
}

void ConfigureJwtBearerOptions(JwtBearerOptions options)
{
    options.IncludeErrorDetails = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ClockSkew = TimeSpan.Zero,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "apiWithAuthBackend",
        ValidAudience = "apiWithAuthBackend",
        IssuerSigningKey = new SymmetricSecurityKey(PadKey(Encoding.UTF8.GetBytes("!SomethingSecret!"), 32))
    };
}

void ConfigureSwagger(Swashbuckle.AspNetCore.SwaggerGen.SwaggerGenOptions options)
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
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
            },
            new string[]{}
        }
    });
}

byte[] PadKey(byte[] key, int length)
{
    if (key.Length >= length)
    {
        return key;
    }

    byte[] paddedKey = new byte[length];
    Array.Copy(key, paddedKey, key.Length);
    return paddedKey;
}
