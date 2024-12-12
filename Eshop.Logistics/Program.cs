using Eshop.Contracts.Basket;
using Eshop.Contracts.Logistics;
using Eshop.Logistics.Consumers;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<OrderCompletedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration.GetConnectionString("messaging"));

        cfg.Publish<OrderDeliveredEvent>();

        cfg.ReceiveEndpoint("Logistics", c =>
        {
            c.Consumer<OrderCompletedConsumer>();
        });
    });
});


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/products/{id}/available", (Guid id) => {
    return Results.Ok();
});

app.Run();
