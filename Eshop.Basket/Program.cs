
using Eshop.Basket.Handlers;
using Eshop.Basket.Store;
using Eshop.Contracts;
using Eshop.Contracts.Basket;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();


builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("basket-store");
    options.InstanceName = "BasketStore";
});

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

builder.Services.AddSingleton<IBasketStore, BasketStore>();

builder.Services.AddMassTransit(x =>
{

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration.GetConnectionString("messaging"));

        //cfg.Message<OrderCheckedEvent>(/*e => e.SetEntityName("order-checkout")*/);
        cfg.Publish<OrderCheckedEvent>();

        //cfg.UseRawJsonSerializer(RawSerializerOptions.AddTransportHeaders | RawSerializerOptions.CopyHeaders);

    });
});


// Add services to the container.
builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapPost("v1/baskets/{id:guid}/products/add", async (IMediator mediator, [FromBody] AddProductCommand command) =>
{
    await mediator.Publish(command);
    return Results.Ok();
});
//app.MapPost("v1/baskets/{id:guid}/products/remove", AddProduct.HandleAsync);
//app.MapGet("v1/baskets/{id:guid}", AddProduct.HandleAsync);
app.MapPost("v1/baskets/{id:guid}/checkout", async (IMediator mediator, [FromBody] CheckoutCommand command) =>
{
    await mediator.Publish(command);
    return Results.Ok();
});

app.MapPost("/test", async (IBus bus) => {
    await bus.Publish(new OrderCheckedEvent(Guid.NewGuid()));
});

app.Run();
