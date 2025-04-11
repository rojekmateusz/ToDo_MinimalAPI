using FluentValidation;
using ToDo_MinimalAPI;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IToDoService, ToDoService>();
builder.Services.AddValidatorsFromAssemblyContaining(typeof(ToDoValidator));
builder.Services.AddAuthentication("Bearer")
     .AddJwtBearer(cfg =>
     {
         cfg.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
         {
             ValidIssuer = builder.Configuration["JwtIssuer"],
             ValidAudience = builder.Configuration["JwtIssuer"],
             IssuerSigningKey = new SymmetricSecurityKey(
                 Encoding.UTF8.GetBytes(builder.Configuration["JwtKey"]))
         };
     });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.RegisterEndpoints();

app.MapGet("/token", () =>
{
    var claims = new[]
    {
         new Claim(ClaimTypes.NameIdentifier, "user-id"),
         new Claim(ClaimTypes.Name, "Test Name"),
         new Claim(ClaimTypes.Role, "Admin"),
     };

    var token = new JwtSecurityToken
    (
        issuer: builder.Configuration["JwtIssuer"],
        audience: builder.Configuration["JwtIssuer"],
        claims: claims,
        expires: DateTime.UtcNow.AddDays(60),
        notBefore: DateTime.UtcNow,
        signingCredentials: new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtKey"])),
            SecurityAlgorithms.HmacSha256)
    );

    var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
    return jwtToken;
});

app.MapGet("/hello", (ClaimsPrincipal user) =>
{
    var userName = user.Identity.Name;
    return $"Hello {userName}";
});

app.Run();
