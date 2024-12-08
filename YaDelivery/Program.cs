using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using YaDelivery.Models.ModelForGetPriceYa;
using YaDelivery.Models.ModelsForGetOffer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
string apiKey = "y0_AgAAAAB6SuOjAAc6MQAAAAEZkhFSAAB7npIbzgBPt7oF_BzdMmnOMLv9UA";


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapGet("/getPriceForOrder", async ([FromServices] IHttpClientFactory httpClientFactory, [FromBody] GetPriceModel request) =>
{
    string apiUrl = "b2b.taxi.tst.yandex.net/api/b2b/platform/pricing-calculator";// «аменить смотрите в доках разница в том что это тестовый
    try
    {
        var httpClient = httpClientFactory.CreateClient();
        var yandexResponse = await httpClient.PostAsJsonAsync(apiUrl, request);

        if (!yandexResponse.IsSuccessStatusCode)
        {
            return Results.Problem($"Error: {yandexResponse.ReasonPhrase}", statusCode: (int)yandexResponse.StatusCode);
        }

       
        var responseContent = await yandexResponse.Content.ReadAsStringAsync();
       

       
        return Results.Ok(yandexResponse);
    }
    catch (Exception ex)
    {
        return Results.Problem($"Unexpected error: {ex.Message}");
    }


})
.WithName("getPriceForOrder")
.WithOpenApi();
app.MapPost("/addOrder", async ([FromServices] IHttpClientFactory httpClientFactory, [FromBody] AddOrderRequest request) =>
{
    string apiUrlForAdd = "b2b.taxi.tst.yandex.net/api/b2b/platform/offers/create";// «аменить смотрите в доках разница в том что это тестовый
    string apiUrlForConfirm = "b2b.taxi.tst.yandex.net/api/b2b/platform/offers/confirm";// «аменить смотрите в доках разница в том что это тестовый
    try
    {
        var httpClient = httpClientFactory.CreateClient();
        var yandexResponse = await httpClient.PostAsJsonAsync(apiUrlForAdd, request);

        if (!yandexResponse.IsSuccessStatusCode)
        {
            return Results.Problem($"Error: {yandexResponse.ReasonPhrase}", statusCode: (int)yandexResponse.StatusCode);
        }


        var responseContent = await yandexResponse.Content.ReadAsStringAsync();
         var yandexData = JsonSerializer.Deserialize<List<Offer>>(responseContent);
        if (yandexData == null || yandexData.Count == 0)
            throw new InvalidOperationException("No offers found.");

        // ѕоиск минимальной цены и соответствующего объекта Offer
        Offer minOffer = yandexData
            .OrderBy(o => decimal.TryParse(o.pricing.Split(' ')[0], out var price) ? price : decimal.MaxValue)
            .FirstOrDefault();
        ConfirmOffer confirmOffer = new ConfirmOffer();
        confirmOffer.offer_id = minOffer.offer_id;
        try
        {
            var confirmResponse = await httpClient.PostAsJsonAsync(apiUrlForConfirm, JsonSerializer.Serialize<ConfirmOffer>(confirmOffer));
            if (!confirmResponse.IsSuccessStatusCode) { return Results.Problem($"Error: {confirmResponse.ReasonPhrase}", statusCode: (int)confirmResponse.StatusCode); }
            return Results.Ok(confirmResponse);
        }
        catch (Exception ex)
        {
            return Results.Problem($"Unexpected error: {ex.Message}");
        }
        if (minOffer == null)
            throw new InvalidOperationException("No valid pricing found.");
        if (yandexData == null)
        {
            return Results.Problem("Failed to parse Yandex API response.");
        }
       

        
    }
    catch (Exception ex)
    {
        return Results.Problem($"Unexpected error: {ex.Message}");
    }


})
.WithName("addOrder")
.WithOpenApi();

app.MapPost("/getStatusOffer", async ([FromServices] IHttpClientFactory httpClientFactory, [FromBody] GetStatusReq request) =>
{
    string apiUrl = "b2b.taxi.tst.yandex.net/api/b2b/platform/request/info";// «аменить смотрите в доках разница в том что это тестовый
   
    try
    {
        var httpClient = httpClientFactory.CreateClient();
        var yandexResponse = await httpClient.PostAsJsonAsync(apiUrl, request);

        if (!yandexResponse.IsSuccessStatusCode)
        {
            return Results.Problem($"Error: {yandexResponse.ReasonPhrase}", statusCode: (int)yandexResponse.StatusCode);
        }


        var responseContent = await yandexResponse.Content.ReadAsStringAsync();
        
        


        return Results.Ok(yandexResponse);
    }
    catch (Exception ex)
    {
        return Results.Problem($"Unexpected error: {ex.Message}");
    }


})
.WithName("getStatusOffer")
.WithOpenApi();
app.Run();

