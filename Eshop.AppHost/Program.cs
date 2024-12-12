using IdentityModel.OidcClient;

var builder = DistributedApplication.CreateBuilder(args);

var messaging = builder.AddRabbitMQ("messaging")
    .WithManagementPlugin()
    .WithDataVolume();

var basketStore = builder.AddRedis("basket-store")
    .WithRedisCommander()
    .WithDataVolume();

//var orderStore = builder.AddSqlServer("order-store")
//    .WithDataVolume();


var logistics = builder.AddProject<Projects.Eshop_Logistics>("logistics")
    .WithReference(messaging)
    .WaitFor(messaging);

builder.AddProject<Projects.Eshop_Basket>("basket")
    .WithReference(messaging)
    .WaitFor(messaging)
    .WithReference(basketStore)
    .WaitFor(basketStore)
    .WithReference(logistics)
    .WaitFor(logistics);

builder.AddProject<Projects.Eshop_Order>("order")
    .WithReference(messaging)
    .WaitFor(messaging);
    //.WithReference(orderStore)
    //.WaitFor(orderStore);

builder.AddProject<Projects.Eshop_Payment>("payment")
    .WithReference(messaging)
    .WaitFor(messaging);


builder.Build().Run();
