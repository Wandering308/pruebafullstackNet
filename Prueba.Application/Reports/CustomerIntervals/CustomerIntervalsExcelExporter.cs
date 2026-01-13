using ClosedXML.Excel;

namespace Prueba.Application.Reports.CustomerIntervals;

public static class CustomerIntervalsExcelExporter
{
    public static byte[] Export(IReadOnlyList<CustomerIntervalsReportDto> rows)
    {
        using var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("CustomerIntervals");

        // Headers
        ws.Cell(1, 1).Value = "Customer";
        ws.Cell(1, 2).Value = "1-50";
        ws.Cell(1, 3).Value = "51-200";
        ws.Cell(1, 4).Value = "201-500";
        ws.Cell(1, 5).Value = "501-1000";
        ws.Cell(1, 6).Value = "Total";

        // Data
        for (int i = 0; i < rows.Count; i++)
        {
            var r = rows[i];
            int row = i + 2;

            ws.Cell(row, 1).Value = r.Customer;
            ws.Cell(row, 2).Value = r.Orders_1_50;
            ws.Cell(row, 3).Value = r.Orders_51_200;
            ws.Cell(row, 4).Value = r.Orders_201_500;
            ws.Cell(row, 5).Value = r.Orders_501_1000;
            ws.Cell(row, 6).Value = r.Total;
        }

        ws.Columns().AdjustToContents();

        using var ms = new MemoryStream();
        wb.SaveAs(ms);
        return ms.ToArray();
    }
}
