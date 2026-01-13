namespace Prueba.Domain.Services;

public static class DistanceRules
{
    public const double MinKm = 1.0;
    public const double MaxKm = 1000.0;

    public static void EnsureValid(double km)
    {
        if (km < MinKm)
            throw new ArgumentOutOfRangeException(nameof(km), $"Distance must be >= {MinKm} km.");

        if (km > MaxKm)
            throw new ArgumentOutOfRangeException(nameof(km), $"Distance must be <= {MaxKm} km.");
    }
}
