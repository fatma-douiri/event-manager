using EventManager.API.Extensions;
using EventManager.Infrastructure.Extensions;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
    logging.AddDebug();
});

// Add services to the container
builder.Services.AddApiServices();
builder.Services.AddSwaggerServices();


// Infrastructure Services (DB + Identity)
builder.Services.AddInfrastructureServices(builder.Configuration);


// CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
      builder =>
      {
          builder
              .WithOrigins(
                  "http://localhost:5173",
                  "http://127.0.0.1:5173"
              )
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials(); // Ajouté pour supporter les cookies/credentials
      });
});


var app = builder.Build();


//Seed data
await app.SeedDataAsync();


// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.Run();