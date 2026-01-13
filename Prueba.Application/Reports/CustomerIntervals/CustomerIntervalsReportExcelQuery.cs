using MediatR;

namespace Prueba.Application.Reports.CustomerIntervals;

public sealed record CustomerIntervalsReportExcelQuery() : IRequest<ExcelFileResult>;
