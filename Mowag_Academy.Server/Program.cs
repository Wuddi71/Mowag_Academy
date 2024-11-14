    using Microsoft.Extensions.Options;
using Mowag_Academy.Server;
using Mowag_Academy.Server.Services;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Dienste konfigurieren
builder.Services.AddControllers();
builder.Services.AddRazorPages();


// Konfiguration der MongoDB-Einstellungen aus der appsettings.json Datei
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDBSettings"));

// Registrieren des MongoClient als Singleton für die Wiederverwendung in der gesamten Anwendung
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDBSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

// Registrieren der MongoDB-Datenbank als Scoped, damit für jede Anforderung eine eigene Instanz bereitgestellt wird
builder.Services.AddScoped<IMongoDatabase>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDBSettings>>().Value;
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(settings.DatabaseName);
});

// Registrieren des UsersService als Scoped, damit eine Instanz für jede Anforderung zur Verfügung steht
builder.Services.AddScoped<UsersService>();

// CORS-Konfiguration hinzufügen, um Anfragen vom Blazor-Client zuzulassen
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(corsBuilder =>
    {
        corsBuilder.WithOrigins("https://localhost:7091") // Die URL des Blazor WebAssembly Projekts
                   .AllowAnyMethod()
                   .AllowAnyHeader();
    });
});

// Hinzufügen der Razor Pages und Controller-Dienste
builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.PropertyNamingPolicy = null);
builder.Services.AddRazorPages();

// Swagger/OpenAPI konfigurieren, aber nur bei Bedarf im Development-Modus aktivieren
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// HTTP-Request-Pipeline konfigurieren
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection(); // HTTPS-Umleitung aktivieren
app.UseStaticFiles();      // Statische Dateien (z. B. für das Blazor-Frontend) aktivieren
app.UseRouting();          // Routing aktivieren
app.UseCors();             // CORS-Konfiguration anwenden
app.UseAuthorization();    // Autorisierung aktivieren

// Endpunkte registrieren
app.MapControllers();      // API-Endpunkte
app.MapRazorPages();       // Razor Pages

// Optionaler Fallback für Blazor-Frontend (wenn aktiv, dann am Ende der Pipeline)
app.MapFallbackToFile("index.html");

// Starten der Anwendung
await app.RunAsync();
