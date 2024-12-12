
using Eshop.Basket;
using Eshop.Basket.Handlers;
using Eshop.Basket.Store;
using Eshop.Contracts;
using Eshop.Contracts.Basket;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OpenTelemetry;
using StackExchange.Redis;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();


//builder.Services.AddStackExchangeRedisCache(options =>
//{
//    options.Configuration = builder.Configuration.GetConnectionString("basket-store");
//    options.InstanceName = "BasketStore";
//});

builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("basket-store")));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

builder.Services.AddSingleton<IBasketStore, BasketStore>();

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration.GetConnectionString("messaging"));
        cfg.Publish<OrderCheckedEvent>();
    });
});


builder.Services.AddHttpClient<LogisticsClient>(x => {
    x.BaseAddress = new("https+http://logistics");
});


builder.Services.AddHttpLogging();


// Add services to the container.
builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();




var app = builder.Build();

app.UseHttpLogging();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.Use((context, next) =>
{
    var parameters = context.Request.Path.ToString().Split('/');
    Activity.Current?.AddBaggage("basket.id", parameters[3]);
    return next(context);
});

app.MapPost("v1/baskets/{id:guid}/products/add", async (IMediator mediator, [FromBody] AddProductCommand command) =>
{
    await mediator.Send(command);
    return Results.Ok();
});

app.MapPost("v1/baskets/{id:guid}/checkout", async (IMediator mediator, [FromBody] CheckoutCommand command) =>
{
    await mediator.Send(command);
    return Results.Ok();
});

app.Run();
