using Eshop.Contracts.Basket;
using Eshop.Order.Saga;
using Eshop.Order.Store;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("order-store")));

builder.Services.AddMassTransit(x =>
{

    x.AddSagaStateMachine<OrderStateMachine, OrderState>();
    x.SetInMemorySagaRepositoryProvider();
    //.EntityFrameworkRepository(r =>
    //{
    //    r.ExistingDbContext<AppDbContext>();
    //    r.UseSqlServer();
    //});

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration.GetConnectionString("messaging"));
        
        cfg.UseInMemoryOutbox(context);
        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.Run();

