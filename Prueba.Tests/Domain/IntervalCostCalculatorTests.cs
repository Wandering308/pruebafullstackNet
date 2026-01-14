using Prueba.Domain.Services;
using Xunit;

namespace Prueba.Tests.Domain;

public sealed class IntervalCostCalculatorTests
{
    [Fact]
    public void CalculateUsd_ShouldBePositive()
    {
        var sut = new IntervalCostCalculator();

        var usd = sut.CalculateUsd(10);

        Assert.True(usd > 0);
    }

    [Fact]
    public void CalculateUsd_ShouldNotDecrease_WhenDistanceIncreases()
    {
        var sut = new IntervalCostCalculator();

        var usd1 = sut.CalculateUsd(10);
        var usd2 = sut.CalculateUsd(60);
        var usd3 = sut.CalculateUsd(300);

        Assert.True(usd2 >= usd1);
        Assert.True(usd3 >= usd2);
    }
}
