namespace Prueba.Application.Reports.CustomerIntervals;

public sealed record CustomerIntervalsReportDto(
    string Customer,
    int Orders_1_50,
    int Orders_51_200,
    int Orders_201_500,
    int Orders_501_1000
)
{
    public int Total => Orders_1_50 + Orders_51_200 + Orders_201_500 + Orders_501_1000;
}
