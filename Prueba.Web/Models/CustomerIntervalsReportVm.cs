namespace Prueba.Web.Models;

public sealed class CustomerIntervalsReportVm
{
    public string Customer { get; set; } = "";
    public int Orders_1_50 { get; set; }
    public int Orders_51_200 { get; set; }
    public int Orders_201_500 { get; set; }
    public int Orders_501_1000 { get; set; }
    public int Total { get; set; }
}
