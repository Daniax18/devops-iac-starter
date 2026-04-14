using Authentification.Data;
using Authentification.Domain;
using Authentification.Service;
using Authentification.Service.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DockerDb")));

builder.Services.AddControllers();

// =============================
// CONFIGURATION JWT
// (Authentification par token)
// =============================
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
// Récupère la clé secrète pour signer les tokens
var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("SecretKey is not configured in appsettings.json");

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    // Définit JWT comme méthode d’authentification par défaut
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
// Configuration du middleware JWT
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,                                                                  // Vérifie qui a émis le token
        ValidateAudience = true,                                                                // Vérifie à qui le token est destiné
        ValidateLifetime = true,                                                                // Vérifie expiration
        ValidateIssuerSigningKey = true,                                                        // Vérifie signature du token
        ValidIssuer = jwtSettings["Issuer"],                                                    // Doit correspondre au token
        ValidAudience = jwtSettings["Audience"],                                                // Doit correspondre au token
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),         // Clé utilisée pour signer et valider le token
        ClockSkew = TimeSpan.Zero                                                               // Pas de tolérance sur l'expiration (plus strict)
    };
});

builder.Services.AddScoped<IToken, TokenService>();
builder.Services.AddScoped<IUser, UserService>();

// Add services to the container.
var app = builder.Build();

// 5. Migration AU DÉMARRAGE (avant app.Run)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthentication();        // Active l’authentification (lecture du token JWT)
//app.UseAuthorization();         // Active l’autorisation ([Authorize])

app.MapControllers();           // Mappe les routes des controllers

app.Run();
