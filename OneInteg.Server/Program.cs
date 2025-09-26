using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using OneInteg.Server.Domain.Repositories;
using OneInteg.Server.Domain.Services;
using OneInteg.Server.IoCConfig;
using OneInteg.Server.Services;
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
builder.Services.AddPaymentProviders();

builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".1Integ.Session";
    options.IdleTimeout = TimeSpan.FromSeconds(300);
    options.Cookie.IsEssential = true;
});

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

app.UseSession();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");


RouteGroupBuilder link = app.MapGroup("/link");
link.MapGet("/{t_id}/subscription/checkout-url", CheckoutUrl);

RouteGroupBuilder backUrl = app.MapGroup("/back-url");

backUrl.MapGet("/{t_id}/subscription/mp", BackUrlSubscriptionMP);

app.Run();


static async Task<IResult> CheckoutUrl(
    HttpContext contex,
    [FromRoute(Name = "t_id")] Guid tenantId,
    [FromQuery(Name = "customer")] string? customer,
    [FromQuery(Name = "plan_id")] string? planId,
    //SERVICES
    [FromServices] ITenantRepository tenantRepository,
    [FromServices] ISubscriptionService subscriptionService)
{
    var tenant = (await tenantRepository.Find(doc => doc.TenantId == tenantId)).FirstOrDefault();

    if (tenant == null)
    {
        return Results.Redirect("/error");
    }

    contex.Session.SetString("ce", customer);
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

static async Task<IResult> BackUrlSubscriptionMP(
    HttpContext contex, 
    [FromRoute(Name = "t_id")] Guid tenantId, 
    [FromServices] IPaymentProvider paymentProvider)
{
    Console.WriteLine(contex.Session.GetString("ce"));
    var queryParams = contex.Request.Query;
    var preapprolvaId = queryParams["preapproval_id"];
    Console.WriteLine("Preapproval_id: ", preapprolvaId);

    if (string.IsNullOrEmpty(preapprolvaId))
    {
        return TypedResults.BadRequest();
    }

    await paymentProvider.HandleBackUrlSubscription(tenantId, preapprolvaId, Encoding.UTF8.GetString(Convert.FromBase64String(contex.Session.GetString("ce"))));

    return TypedResults.Ok();
}

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}