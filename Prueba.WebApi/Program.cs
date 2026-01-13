using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Prueba.Application.Orders.CreateOrder;
using Prueba.Application.Reports.CustomerIntervals;
using Prueba.Domain.Interfaces;
using Prueba.Domain.Services;
using Prueba.Infrastructure.Persistence;
using Prueba.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddFluentValidationAutoValidation();

// Registrar validators manualmente (sin extensión)
builder.Services.AddScoped<FluentValidation.IValidator<Prueba.Application.Orders.CreateOrder.CreateOrderCommand>,
    Prueba.Application.Orders.CreateOrder.CreateOrderCommandValidator>();


// MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<CreateOrderHandler>());

// CustomerIntervals (Excel)
builder.Services.AddScoped<CustomerIntervalsReportService>();
builder.Services.AddScoped<CustomerIntervalsExcelExporter>();

// DB
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

// Repositories
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// Domain services
builder.Services.AddScoped<IDistanceCalculator, HaversineDistanceCalculator>();
builder.Services.AddScoped<ICostCalculator, IntervalCostCalculator>();

// CORS
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("Default", p =>
        p.AllowAnyOrigin()
         .AllowAnyHeader()
         .AllowAnyMethod());
});

var app = builder.Build();

// Global error handler
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

app.MapControllers();

app.Run();
