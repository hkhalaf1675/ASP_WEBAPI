using Day2_Lab_WebAPI.Contexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// create variable for getting the connection string from appsetting
var con = builder.Configuration.GetConnectionString("DefaultConnection");
//inject the dbContext and give it the conncetion string
builder.Services.AddDbContext<ITIDbContext>(options =>
{
    options.UseSqlServer(con);
});

// ---> add the cors policy
builder.Services.AddCors(corsOptions =>
{
    corsOptions.AddPolicy("Policy_1", corsPolicyBuilder =>
    {
        corsPolicyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();// to enable using static files like {html pages,images,the files on wwwroot}

app.UseCors("Policy_1");

app.UseAuthorization();

app.MapControllers();

app.Run();
