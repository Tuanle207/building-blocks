using Caching.Redis;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

var redisCacheSettings = new RedisCacheSettings();
builder.Configuration.GetSection(nameof(RedisCacheSettings)).Bind(redisCacheSettings);
builder.Services.AddStackExchangeRedisCache(options => options.Configuration = redisCacheSettings.ConnectionString);
builder.Services.AddSingleton<IResponseCacheService, ResponseCacheService>();
builder.Services.AddSingleton(redisCacheSettings);

builder.Services.AddControllers(options =>
{
    //options.Filters.Add<CachedAttribute>();
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/api/long-request-endpoint", [Cached(600)] async () =>
{
    await Task.Delay(5000);
    return Results.Ok("big query data");
});

app.MapControllers();


app.Run();
