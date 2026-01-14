using Prueba.Domain.Services;
using Prueba.Domain.ValueObjects;
using Xunit;

namespace Prueba.Tests.Domain;

public sealed class HaversineDistanceCalculatorTests
{
    [Fact]
    public void CalculateKm_WhenPointsAreDifferent_ShouldBeGreaterThanZero()
    {
        // Bogota Centro -> Otro punto en Bogota
        var origin = new GeoPoint(4.7110, -74.0721);
        var destination = new GeoPoint(4.7300, -74.0500);

        var sut = new HaversineDistanceCalculator();

        var km = sut.CalculateKm(origin, destination);

        Assert.True(km > 0);
    }
}
