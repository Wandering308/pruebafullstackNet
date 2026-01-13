using ClosedXML.Excel;

namespace Prueba.Application.Reports.CustomerIntervals;

public sealed class CustomerIntervalsExcelExporter
{
    public ExcelFileResult Export(CustomerIntervalsResults report)
    {
        using var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("CustomerIntervals");

        // Header
        ws.Cell(1, 1).Value = "Customer";
        ws.Cell(1, 2).Value = "1-50";
        ws.Cell(1, 3).Value = "51-200";
        ws.Cell(1, 4).Value = "201-500";
        ws.Cell(1, 5).Value = "501-1000";
        ws.Cell(1, 6).Value = "Total";

        ws.Range(1, 1, 1, 6).Style.Font.Bold = true;

        // Rows
        var row = 2;
        foreach (var item in report.Items)
        {
            ws.Cell(row, 1).Value = item.Customer;
            ws.Cell(row, 2).Value = item.From1To50;
            ws.Cell(row, 3).Value = item.From51To200;
            ws.Cell(row, 4).Value = item.From201To500;
            ws.Cell(row, 5).Value = item.From501To1000;
            ws.Cell(row, 6).Value = item.Total;
            row++;
        }

        ws.Columns().AdjustToContents();

        using var ms = new MemoryStream();
        wb.SaveAs(ms);

        var fileName = $"customer-intervals-{DateTime.UtcNow:yyyyMMdd-HHmmss}.xlsx";

        return new ExcelFileResult(
            ms.ToArray(),
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            fileName
        );
    }
}
