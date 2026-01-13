using System.Collections.Generic;

namespace Prueba.Web.Models;

public sealed class OrdersByCustomerVm
{
    public string Customer { get; set; } = "";
    public List<OrderDto> Orders { get; set; } = new();
    public string? Error { get; set; }
}

public sealed class OrderDto
{
    public string Id { get; set; } = "";
    public string Customer { get; set; } = "";
    public string Product { get; set; } = "";
    public int Quantity { get; set; }

    public GeoPointDto Origin { get; set; } = new();
    public GeoPointDto Destination { get; set; } = new();

    public double DistanceKm { get; set; }
    public double CostUsd { get; set; }
}

public sealed class GeoPointDto
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
