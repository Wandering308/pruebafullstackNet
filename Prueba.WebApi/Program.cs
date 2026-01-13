using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Prueba.Application.Orders.CreateOrder;
using Prueba.Domain.Interfaces;
using Prueba.Domain.Services;
using Prueba.Infrastructure.Persistence;
using Prueba.Infrastructure.Repositories;

using Prueba.Application.Reports.CustomerIntervals;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddScoped<CustomerIntervalsReportService>();


// ---------- MVC / Controllers ----------
builder.Services.AddControllers();

// ---------- Swagger ----------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ---------- Validation (FluentValidation) ----------
builder.Services.AddFluentValidationAutoValidation();

// (Si luego creamos validators, se registran aquí automáticamente si están en el assembly)
builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderService>();

// ---------- DB (SQL Server) ----------
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

// ---------- DI (Clean Architecture) ----------
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddScoped<IDistanceCalculator, HaversineDistanceCalculator>();
builder.Services.AddScoped<ICostCalculator, IntervalCostCalculator>();

builder.Services.AddScoped<CreateOrderService>();

// ---------- Health checks (útil para demo) ----------
builder.Services.AddHealthChecks();

// ---------- CORS (opcional; útil si luego la Web consume la API desde otro host) ----------
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("Default", p =>
        p.AllowAnyOrigin()
         .AllowAnyHeader()
         .AllowAnyMethod());
});

var app = builder.Build();

// ---------- Global error handler (simple y limpio) ----------
app.UseExceptionHandler(errApp =>
{
    errApp.Run(async context =>
    {
        var feature = context.Features.Get<IExceptionHandlerFeature>();
        var ex = feature?.Error;

        context.Response.ContentType = "application/json";

        // Validaciones / reglas
        if (ex is ArgumentOutOfRangeException or ArgumentException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new
            {
                error = ex.Message
            });
            return;
        }

        // Default
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
