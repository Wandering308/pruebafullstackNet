using Prueba.Domain.ValueObjects;

namespace Prueba.Domain.Services;

public sealed class HaversineDistanceCalculator : IDistanceCalculator
{
    private const double EarthRadiusKm = 6371.0;

    public double CalculateKm(GeoPoint origin, GeoPoint destination)
    {
        // Convert degrees to radians
        double lat1 = DegreesToRadians(origin.Latitude);
        double lon1 = DegreesToRadians(origin.Longitude);
        double lat2 = DegreesToRadians(destination.Latitude);
        double lon2 = DegreesToRadians(destination.Longitude);

        double dLat = lat2 - lat1;
        double dLon = lon2 - lon1;

        double a =
            Math.Pow(Math.Sin(dLat / 2), 2) +
            Math.Cos(lat1) * Math.Cos(lat2) *
            Math.Pow(Math.Sin(dLon / 2), 2);

        double c = 2 * Math.Asin(Math.Sqrt(a));
        return EarthRadiusKm * c;
    }

    private static double DegreesToRadians(double degrees) => degrees * (Math.PI / 180.0);
}
