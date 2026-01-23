using API.Data.DAO;
using API.Data.Interfaces;
using API.Services.Interfaces;
using API.Services.Realisations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/// ---- Injections des dépendances ----
builder.Services.AddScoped<ICompanyService, CompanyService>();

builder.Services.AddScoped<ICompanyDAO, CompanyDAO>();

builder.Services.AddScoped<IRapportService, RapportService>();

builder.Services.AddScoped<IRapportDAO, RapportDAO>();

builder.Services.AddScoped<ITagService, TagService>();

builder.Services.AddScoped<ITokenService, JwtService>();

builder.Services.AddScoped<ITagDAO, TagDAO>();

builder.Services.AddScoped<IUserDAO, UserDAO>();

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IUploadHandler, UploadHandler>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
            )
        };
    });



Console.WriteLine("1️⃣ Avant Build");
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

Console.WriteLine("2️⃣ Après Build");

app.UseHttpsRedirection();

Console.WriteLine("3️⃣ Après Https");

app.UseRouting();

Console.WriteLine("4️⃣ Après Routing");

// CORS
app.UseCors(options =>
    options
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod()
);

Console.WriteLine("5️⃣ Après CORS");


// Sécurité
app.UseAuthentication();

Console.WriteLine("6️⃣ Après Auth");

app.UseAuthorization();

Console.WriteLine("7️⃣ Après Authorize");

app.MapControllers();

Console.WriteLine("8️⃣ Après MapControllers");

app.Run();
