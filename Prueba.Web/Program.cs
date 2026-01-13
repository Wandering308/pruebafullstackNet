var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

// HttpClient hacia la API
builder.Services.AddHttpClient("Api", client =>
{
    var baseUrl = builder.Configuration.GetValue<string>("Api:BaseUrl");

    // fallback para que no se rompa si algo no lee config
    if (string.IsNullOrWhiteSpace(baseUrl))
        baseUrl = "http://localhost:5000";

    client.BaseAddress = new Uri(baseUrl);
});

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Orders}/{action=Index}/{id?}");

app.Run();
