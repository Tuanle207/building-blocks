using Authentication.Bearer.Configurations;
using Authentication.Bearer.Dtos;
using Authentication.Bearer.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAuthentication(option =>
    {
        option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.Configure<AppSetting>(builder.Configuration.GetSection("AppSetting"));

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/api/login", ([FromBody] LoginDto loginDto, IUserRepository userRepository) =>
{
    if (string.IsNullOrEmpty(loginDto.Email))
    {
        return Results.BadRequest();
    }
    var user = userRepository.GetUserByEmail(loginDto.Email);
    if (user == default)
    {
        return Results.Unauthorized();
    }
    var hashedCandidatePassword = user.HashedPassword; // Should replace with actual hasher
    if (user.HashedPassword != hashedCandidatePassword)
    {
        return Results.Unauthorized();
    }
    var issuer = builder.Configuration["Jwt:Issuer"];
    var audience = builder.Configuration["Jwt:Audience"];
    var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);
    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(new[]
        {
            new Claim("id", user.Id.ToString()),
            new Claim("first_name", user.FirstName),
            new Claim("last_name", user.LastName),
            new Claim("email", user.Email)
        }),
        Expires = DateTime.UtcNow.AddMinutes(5),
        Issuer = issuer,
        Audience = audience,
        SigningCredentials = new SigningCredentials
        (new SymmetricSecurityKey(key),
        SecurityAlgorithms.HmacSha512Signature)
    };
    var tokenHandler = new JwtSecurityTokenHandler();
    var token = tokenHandler.CreateToken(tokenDescriptor);
    var jwtToken = tokenHandler.WriteToken(token);
    return Results.Ok(jwtToken);
});

app.MapGet("/api/resources", [Authorize] () =>
{
    return Results.Ok("resources");
});

app.Run();
