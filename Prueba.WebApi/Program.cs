using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Prueba.Application.Orders.CreateOrder;
using Prueba.Domain.Interfaces;
using Prueba.Domain.Services;
using Prueba.Infrastructure.Persistence;
using Prueba.Infrastructure.Repositories;
using Prueba.Application.Reports.CustomerIntervals;

var builder = WebApplication.CreateBuilder(args);

// ---------- Controllers ----------
builder.Services.AddControllers();

builder.Services.AddScoped<CustomerIntervalsReportService>();
builder.Services.AddScoped<CustomerIntervalsExcelExporter>();


// ---------- AutoMapper (AutoMapper 16) ----------
builder.Services.AddAutoMapper(cfg =>
{
    // Escanea profiles del assembly Application
    cfg.AddMaps(typeof(Prueba.Application.Orders.OrderMappingProfile).Assembly);
}, typeof(Prueba.Application.Orders.OrderMappingProfile).Assembly);

// ---------- Swagger ----------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ---------- FluentValidation ----------
builder.Services.AddFluentValidationAutoValidation();
// Asegura el scan de validators en Application
builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderHandler>();

// ---------- MediatR (CQRS) ----------
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<CreateOrderHandler>());

// ---------- DB (SQL Server) ----------
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

// ---------- Repositories ----------
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// ---------- Domain services ----------
builder.Services.AddScoped<IDistanceCalculator, HaversineDistanceCalculator>();
builder.Services.AddScoped<ICostCalculator, IntervalCostCalculator>();

// ---------- Health checks ----------
builder.Services.AddHealthChecks();

// ---------- CORS ----------
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("Default", p =>
        p.AllowAnyOrigin()
         .AllowAnyHeader()
         .AllowAnyMethod());
});

var app = builder.Build();

// ---------- Global error handler ----------
app.UseExceptionHandler(errApp =>
{
    errApp.Run(async context =>
    {
        var feature = context.Features.Get<IExceptionHandlerFeature>();
        var ex = feature?.Error;

        context.Response.ContentType = "application/json";

        if (ex is ArgumentOutOfRangeException or ArgumentException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new { error = ex.Message });
            return;
        }

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(new
        {
            error = "Unexpected error",
            detail = ex?.Message
        });
    });
});

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("Default");

app.MapHealthChecks("/health");
app.MapControllers();

app.Run();
