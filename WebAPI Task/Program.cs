using Application.Command;
using Application.Services;
using Domain;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Models;
using WebAPI_Task.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register MediatR for command handling
builder.Services.AddMediatR(cfg =>
{
    // Register all commands from the current assembly
    cfg.RegisterServicesFromAssemblyContaining<SignUpCommand>();
    cfg.RegisterServicesFromAssemblyContaining<BalanceCommand>();
    cfg.RegisterServicesFromAssemblyContaining<AuthenticateCommand>();
});

// Register application services
builder.Services.AddScoped<AppDbContext>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    return new AppDbContext(connectionString);
});

// Register repository and service implementations
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.MapControllers();

app.Run();
