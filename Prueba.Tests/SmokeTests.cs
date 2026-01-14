using Prueba.Domain.Services;
using Prueba.Domain.ValueObjects;
using Xunit;

namespace Prueba.Tests;

public class SmokeTests
{
    [Fact]
    public void HaversineDistanceCalculator_ReturnsPositiveDistance()
    {
        var calc = new HaversineDistanceCalculator();
        var a = new GeoPoint(4.7110, -74.0721);
        var b = new GeoPoint(4.7300, -74.0500);

        var km = calc.CalculateKm(a, b);

        Assert.True(km > 0);
    }

    [Fact]
    public void IntervalCostCalculator_ReturnsExpectedCost()
    {
        var cost = new IntervalCostCalculator();

        var usd = cost.CalculateUsd(10);

        Assert.True(usd > 0);
    }
}
