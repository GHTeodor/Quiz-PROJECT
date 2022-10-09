using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Quiz_PROJECT;
using Quiz_PROJECT.Configurations;
using Quiz_PROJECT.Errors;
using Quiz_PROJECT.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCorsS();

builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpContextAccessor();

builder.Services.AddSwaggerGen();

builder.Services.AddSwagger();

builder.Services.AddAutoMapper(typeof(MapperInitializer));

builder.Services.AddDbContext<DBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyDBConnection")));

builder.Services.AddIdentity<User, ApplicationRole>()
    .AddEntityFrameworkStores<DBContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthenticationS(builder.Configuration);

builder.Services.AddServices();
builder.Services.AddRepositories();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

SeedDatabase.Initialize(app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope().ServiceProvider);

app.UseMiddleware<ExceptionHandler>();

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();