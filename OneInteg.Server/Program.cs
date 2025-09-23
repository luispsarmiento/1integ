using Microsoft.AspNetCore.Mvc;
using OneInteg.Server.IoCConfig;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddCustomMongoDbService();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseCors();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");


RouteGroupBuilder api = app.MapGroup("/api/v1");
api.MapPost("/subscription/checkout-url", CheckoutUrl);

app.Run();

static async Task<IResult> CheckoutUrl([FromQuery(Name = "t_id")] Guid? tenantId)
{
    if (!tenantId.HasValue)
    {
        return TypedResults.BadRequest();
    }

    return TypedResults.Ok(new { tId = tenantId });
}

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}