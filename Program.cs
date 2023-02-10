using Microsoft.EntityFrameworkCore;
using MovieDatabase_API.Controllers.Endpoints;
using MovieDatabase_API.Db;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MovieContext>(c => c.UseSqlServer(builder.Configuration["AppDbContextConnection"]));
builder.Services.AddTransient<AddEndpoint>();
builder.Services.AddTransient<DeleteEndpoint>();
builder.Services.AddTransient<SearchEndpoint>();
builder.Services.AddTransient<UpdateEndpoint>();
builder.Services.AddTransient<GetEndpoint>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetService<MovieContext>();
dbContext!.Database.EnsureCreated();

app.Run();