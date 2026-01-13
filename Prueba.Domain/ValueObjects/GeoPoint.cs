namespace Prueba.Domain.ValueObjects;

public sealed record GeoPoint(double Latitude, double Longitude)
{
    public override string ToString() => $"{Latitude},{Longitude}";
}
