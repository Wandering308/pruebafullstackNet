namespace Prueba.Domain.Services;

public sealed class IntervalCostCalculator : ICostCalculator
{
    public double CalculateUsd(double distanceKm)
    {
        // Nota: DistanceRules ya valida que esté entre 1 y 1000 km.
        if (distanceKm <= 50) return 100;
        if (distanceKm <= 200) return 300;
        if (distanceKm <= 500) return 1000;
        return 1500; // 501..1000
    }
}
