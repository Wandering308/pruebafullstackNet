namespace Prueba.Web.Models;

public sealed class OrderVm
{
    public Guid Id { get; set; }
    public string Customer { get; set; } = "";
    public string Product { get; set; } = "";
    public int Quantity { get; set; }

    public double OriginLat { get; set; }
    public double OriginLon { get; set; }
    public double DestinationLat { get; set; }
    public double DestinationLon { get; set; }

    public double DistanceKm { get; set; }
    public double CostUsd { get; set; }
}
