using Prueba.Domain.ValueObjects;

namespace Prueba.Domain.Services;

public interface IDistanceCalculator
{
    double CalculateKm(GeoPoint origin, GeoPoint destination);
}
