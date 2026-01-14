using Prueba.Domain.Services;
using Xunit;

namespace Prueba.Tests.Domain;

public sealed class DistanceRulesTests
{
    [Fact]
    public void EnsureValid_WhenDistanceIsGreaterThan1000_ShouldThrow()
    {
        var tooFar = 1000.01;

        Assert.Throws<ArgumentOutOfRangeException>(() => DistanceRules.EnsureValid(tooFar));
    }

    [Fact]
    public void EnsureValid_WhenDistanceIsLessOrEqualThan1000_ShouldNotThrow()
    {
        var ok = 1000.0;

        var ex = Record.Exception(() => DistanceRules.EnsureValid(ok));

        Assert.Null(ex);
    }
}
