using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using OneInteg.Server.Domain.Repositories;
using OneInteg.Server.Domain.Services;
using OneInteg.Server.IoCConfig;
using System.Net.Mail;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddCustomMongoDbService();

builder.Services.AddServiceAndRepositories();

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


RouteGroupBuilder link = app.MapGroup("/link");
link.MapGet("/subscription/checkout-url", CheckoutUrl);

RouteGroupBuilder backUrl = app.MapGroup("/back-url");

backUrl.MapGet("/subscription/mp", BackUrlSubscriptionMP);

app.Run();


static async Task<IResult> CheckoutUrl(
    [FromQuery(Name = "t_id")] Guid? tenantId,
    [FromQuery(Name = "customer")] string? customer,
    [FromQuery(Name = "plan_id")] string? planId,
    //SERVICES
    [FromServices] ITenantRepository tenantRepository,
    [FromServices] ISubscriptionService subscriptionService)
{

    if (!tenantId.HasValue)
    {
        return Results.Redirect("/error");
    }

    var tenant = (await tenantRepository.Find(doc => doc.TenantId == tenantId)).FirstOrDefault();

    if (tenant == null)
    {
        return Results.Redirect("/error");
    }

    var subscriptionLink = await subscriptionService.GetCheckoutUrl(new OneInteg.Server.DataAccess.Customer
    {
        TenantId = tenant.TenantId,
        Email = Encoding.UTF8.GetString(Convert.FromBase64String(customer))
    }, planId);

    if (string.IsNullOrEmpty(subscriptionLink))
    {
        return Results.Redirect("/error");
    }

    return Results.Redirect(subscriptionLink);
}

static async Task<IResult> BackUrlSubscriptionMP(HttpContext contex)
{
    var queryParams = contex.Request.Query;
    var preapprolvaId = queryParams["preapproval_id"];
    Console.WriteLine(queryParams.ToString());

    return TypedResults.Ok();
}

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}