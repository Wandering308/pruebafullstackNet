namespace Prueba.Application.Reports.CustomerIntervals;

public sealed record CustomerIntervalsReportDto(
    string Customer,
    int From1To50,
    int From51To200,
    int From201To500,
    int From501To1000,
    int Total
);
