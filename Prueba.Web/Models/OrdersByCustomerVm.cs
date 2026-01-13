namespace Prueba.Web.Models;

public sealed class OrdersByCustomerVm
{
    public string Customer { get; set; } = "";
    public List<OrderVm> Orders { get; set; } = new();
    public string? Error { get; set; }
}
