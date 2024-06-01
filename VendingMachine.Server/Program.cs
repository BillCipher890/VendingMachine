using Microsoft.EntityFrameworkCore;
using VendingMachine.Server;
using VendingMachine.Server.Autentification;
using VendingMachine.Server.CountChange;
using VendingMachine.Server.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<ICoinRepository, CoinRepository>();
builder.Services.AddScoped<IDrinkRepository, DrinkRepository>();
builder.Services.AddSingleton<ICountChange, CountChangeRU>();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddDbContext<AppDBContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var password = builder.Configuration.GetSection("Password").Value;
builder.Services.AddScoped<IChecker<string>>(_ => new PasswordChecker(password));

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors(builder =>
{
    builder.WithOrigins("https://localhost:5173")
           .AllowAnyMethod()
           .AllowAnyHeader();
});

app.MapFallbackToFile("/index.html");

app.Run();
