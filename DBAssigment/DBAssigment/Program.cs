using DBAssigment.Contexts;
using DBAssigment.Models;
using DBAssigment.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// connection string
var connect = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connect);
});


// inject the usermanagerservices
builder.Services.AddScoped(typeof(IUserManagerServices), typeof(UserManagerServices));


#region Identity
builder.Services.AddIdentity<User, IdentityRole>(Options =>
{
    Options.User.RequireUniqueEmail = true;
    Options.Password.RequiredLength = 8;
    Options.Lockout.MaxFailedAccessAttempts = 5;
    Options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(20);


}).AddEntityFrameworkStores<ApplicationDbContext>();
#endregion

#region Authentication
builder.Services.AddAuthentication(Options =>
{
    Options.DefaultAuthenticateScheme = "Default";
    Options.DefaultChallengeScheme = "Default";
})
.AddJwtBearer("Default", options =>
{
    var KeyString = builder.Configuration.GetSection("JWT").GetValue<string>("Key");
    var KeyInBytes = Encoding.ASCII.GetBytes(KeyString);
    var Key = new SymmetricSecurityKey(KeyInBytes);
    options.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = Key,
        ValidateIssuer = false,
        ValidateAudience = false
    };
}).AddGoogle(googleOptions =>
{
    googleOptions.ClientId = builder.Configuration.GetSection("GoogleAuthenSetting").GetValue<string>("ClientId");
    googleOptions.ClientSecret = builder.Configuration.GetSection("GoogleAuthenSetting").GetValue<string>("ClientSecret");
});

#endregion

#region Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("user", policy => policy
    .RequireClaim(ClaimTypes.Role, "Client")
    .RequireClaim(ClaimTypes.NameIdentifier)
    );
});
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseCors(c => c
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
