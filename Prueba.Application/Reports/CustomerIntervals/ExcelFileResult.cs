namespace Prueba.Application.Reports.CustomerIntervals;

public sealed record ExcelFileResult(byte[] Content, string ContentType, string FileName);
